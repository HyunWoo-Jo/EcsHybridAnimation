using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using UnityEngine.Experimental.AI;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Game.Ecs.Aspect;

namespace Game.Ecs.System {
    /// <summary>
    /// Nav Agent 컨트롤 시스템
    /// </summary>
    [BurstCompile]
    public partial struct NavAgentSystem : ISystem {

        private NavMeshQuery navQuery;
        float3 extents;
        int maxPathSize;
        [BurstCompile]
        private void OnCreate(ref SystemState state) {
            CreateNavMesh();
            extents = new float3(1, 1, 1);
            maxPathSize = 100;
        }
        [BurstCompile]
        private void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var navAspect in SystemAPI.Query<NavAgentAspect>()) {
                FindPath(ref state, navAspect, deltaTime);
                if (navAspect.IsStop()) return;
                if (navAspect.IsFinded()) {
                    Move(navAspect, deltaTime);
                }
            }
        }

        private void CreateNavMesh() {
            navQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent, 1000);
        }

        [BurstCompile]
        private void FindPath(ref SystemState state, NavAgentAspect navAspect, float deltaTime) {
            if (navAspect.IsFinded()) return;
           
            // 검색
            float3 startPosition = navAspect.Position;
            float3 endPosition = SystemAPI.GetComponent<LocalTransform>(navAspect.GetTargetEntity()).Position;

            NavMeshLocation startLocation = navQuery.MapLocation(startPosition, extents, 0);
            NavMeshLocation endLocation = navQuery.MapLocation(endPosition, extents, 0);
            PathQueryStatus status;
            PathQueryStatus returningStatus;
            if (navQuery.IsValid(startLocation) && navQuery.IsValid(endLocation)) {
                status = navQuery.BeginFindPath(startLocation, endLocation);
                if (status == PathQueryStatus.InProgress) {
                    status = navQuery.UpdateFindPath(maxPathSize, out int iterationsPerformed);  
                }
                if (status == PathQueryStatus.Success) {
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

                        foreach (NavMeshLocation location in result) {
                            if (location.position != Vector3.zero) {
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
        private void Move(NavAgentAspect navAspect, float deltaTime) {
            float3 position = navAspect.Position;
            float3 currentWayPosition = navAspect.GetCurrentWaypointPosition();
            position.y = 0f;
            currentWayPosition.y = 0.02f;
            if (math.distance(position, currentWayPosition) < navAspect.GetTraceRange()) {
                navAspect.NextWaypoint();
            }
            float3 direction = currentWayPosition - position;
            if (direction.x == 0f && direction.z == 0f) return;
            // 회전
            quaternion targetRotation = quaternion.LookRotation(direction, new float3(0, 1, 0));
            float angle = math.degrees(math.atan2(direction.z, direction.x));
            navAspect.Rotation = math.slerp(navAspect.Rotation, targetRotation, navAspect.GetRotationSpeed() *  deltaTime);
            // 이동
            navAspect.Position += math.normalize(direction) * navAspect.GetMoveSpeed() * deltaTime;
        }
    }
}

