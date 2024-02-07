using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using UnityEngine.Experimental.AI;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs {
    /// <summary>
    /// Nav Agent 컨트롤 시스템
    /// </summary>
    [BurstCompile]
    public partial struct NavAgentSystem : ISystem {

        private NavMeshQuery navQuery;
        private float queryTimer;
        private float queryUpdateTime;
        private float findUpdateTime;
        [BurstCompile]
        private void OnCreate(ref SystemState state) {
            CreateNavMesh();
            queryTimer = 0;
            queryUpdateTime = 1;
            findUpdateTime = 0.5f;
        }
        [BurstCompile]
        private void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;
            UpdateNavMeshQuery(deltaTime);
            FindPath(ref state, deltaTime);

            new NavAgentMoveJob {
                deltaTime = deltaTime 
            }.ScheduleParallel();
        }

        #region Method and Jobs

        private void CreateNavMesh() {
            navQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent, 1000);
        }
        private void UpdateNavMeshQuery(float deltaTime) {
            queryTimer += deltaTime;
            if (queryTimer > queryUpdateTime) {
                queryTimer = 0;
                navQuery.Dispose();
                CreateNavMesh();
            }
        }
        [BurstCompile]
        private void FindPath(ref SystemState state, float deltaTime) {
            float3 extents = new float3(1, 3, 1);
            int maxPathSize = 100;
            foreach (var navAspect in SystemAPI.Query<NavAgentAspect>()) {
                // 업데이트 간격
                navAspect.Timer += deltaTime;
                if (navAspect.Timer < findUpdateTime) continue;
                navAspect.Timer = 0f;
                // 검색
                float3 startPosition = navAspect.Position;
                float3 endPosition = SystemAPI.GetComponent<LocalTransform>(navAspect.GetTargetEntity()).Position;

                NavMeshLocation startLocation = navQuery.MapLocation(startPosition, extents, 0);
                NavMeshLocation endLocation = navQuery.MapLocation(endPosition, extents, 0);
                PathQueryStatus status;
                PathQueryStatus returningStatus;
                Debug.Log(navQuery.IsValid(startLocation) + " " + navQuery.IsValid(endLocation));
                if (navQuery.IsValid(startLocation) && navQuery.IsValid(endLocation)) {
                    Debug.Log("Chk");
                    status = navQuery.BeginFindPath(startLocation, endLocation);
                    if(status == PathQueryStatus.InProgress) {
                        status = navQuery.UpdateFindPath(maxPathSize, out int iterationsPerformed);
                        if(status == PathQueryStatus.Success) {
                            status = navQuery.EndFindPath(out int pathSize);

                            NativeArray<NavMeshLocation> result = new NativeArray<NavMeshLocation>(pathSize + 1, Allocator.Temp);
                            NativeArray<StraightPathFlags> straightPathFlags = new NativeArray<StraightPathFlags>(maxPathSize, Allocator.Temp);
                            NativeArray<float> vertexSize = new NativeArray<float>(maxPathSize, Allocator.Temp);
                            NativeArray<PolygonId> polygonIds = new NativeArray<PolygonId>(pathSize + 1, Allocator.Temp);
                            int straightPathCount = 0;

                            navQuery.GetPathResult(polygonIds);

                            returningStatus = PathUtils.FindStraightPath(
                                navQuery,
                                startPosition,
                                endPosition,
                                polygonIds,
                                pathSize,
                                ref result,
                                ref straightPathFlags,
                                ref vertexSize,
                                ref straightPathCount,
                                maxPathSize
                                );
                            if (returningStatus == PathQueryStatus.Success) {
                                navAspect.ClearWaypointBuffer();

                                foreach(NavMeshLocation location in result) {
                                    if(location.position != Vector3.zero) {
                                        navAspect.AddWaypointBuffer(location.position);
                                    }
                                }
                                navAspect.FindedNavAgent();
                            }
                            straightPathFlags.Dispose();
                            polygonIds.Dispose();
                            vertexSize.Dispose();
                        }
                        
                    }
                }
            }   
        }
        /// <summary>
        /// Move Job
        /// </summary>
        [BurstCompile]
        private partial struct NavAgentMoveJob : IJobEntity {
            public float deltaTime;

            [BurstCompile]
            private void Execute(NavAgentAspect aspect) {
                if (aspect.IsStop()) return;
                if (aspect.IsFinded()) {
                    Move(aspect);
                }
            }
            [BurstCompile]
            private void Move(NavAgentAspect aspect) {
                if (math.distance(aspect.Position, aspect.GetCurrentWaypointPosition()) < 0.3f) {
                    aspect.NextWaypoint();
                }
                // 회전
                float3 direction = aspect.GetCurrentWaypointPosition() - aspect.Position;
                float angle = math.degrees(math.atan2(direction.z, direction.x));
                aspect.Rotation = math.slerp(aspect.Rotation, quaternion.Euler(new float3(0, angle, 0)), deltaTime);
                // 이동
                aspect.Position += math.normalize(direction) * aspect.GetMoveSpeed() * deltaTime;
            }

        }
        #endregion
    }
}

