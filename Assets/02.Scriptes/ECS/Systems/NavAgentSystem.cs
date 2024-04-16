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
using UnityEngine.AI;
namespace Game.Ecs.System {
    /// <summary>
    /// Nav Agent 컨트롤 시스템
    /// </summary>
    [BurstCompile]
    public partial struct NavAgentSystem : ISystem {

        private NavMeshQuery _navQuery;
        private float3 _extents;
        private int _maxPathSize;


        [BurstCompile]
        private void OnCreate(ref SystemState state) {
            CreateNavMesh();
            _extents = new float3(1, 1, 1);
            _maxPathSize = 100;
        }
        [BurstCompile]
        private void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var navAspect in SystemAPI.Query<NavAgentAspect>()) {
                if (!World.DefaultGameObjectInjectionWorld.EntityManager.Exists(navAspect.GetTargetEntity())) continue;
                FindPath(ref state, navAspect);
                if (navAspect.IsStop) continue;
                if (navAspect.IsFinded) {
                    Move(navAspect, deltaTime);
                }
            }
        }

        private void CreateNavMesh() {
            _navQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent, 1000);
        }

        //[BurstCompile]
        private void FindPath(ref SystemState state, NavAgentAspect navAspect) {
            float3 startPosition = navAspect.Position;
            float3 endPosition = SystemAPI.GetComponentRO<LocalTransform>(navAspect.GetTargetEntity()).ValueRO.Position;
            NavMeshLocation startLocation = _navQuery.MapLocation(startPosition, _extents, 0);
            NavMeshLocation endLocation = _navQuery.MapLocation(endPosition, _extents, 0);
         
            PathQueryStatus status;
            PathQueryStatus returningStatus;
            if (_navQuery.IsValid(startLocation) && _navQuery.IsValid(endLocation)) {
                status = _navQuery.BeginFindPath(startLocation, endLocation);
                if (status == PathQueryStatus.InProgress) {
                    status = _navQuery.UpdateFindPath(_maxPathSize, out int iterationsPerformed);
                }
                if (status == PathQueryStatus.Success) {
                    status = _navQuery.EndFindPath(out int pathSize);

                    NativeArray<NavMeshLocation> result = new NativeArray<NavMeshLocation>(pathSize + 1, Allocator.Temp);
                    NativeArray<StraightPathFlags> straightPathFlags = new NativeArray<StraightPathFlags>(_maxPathSize, Allocator.Temp);
                    NativeArray<float> vertexSize = new NativeArray<float>(_maxPathSize, Allocator.Temp);
                    NativeArray<PolygonId> polygonIds = new NativeArray<PolygonId>(pathSize + 1, Allocator.Temp);
                    int straightPathCount = 0;

                    _navQuery.GetPathResult(polygonIds);

                    returningStatus = PathUtils.FindStraightPath(
                        _navQuery,
                        startPosition,
                        endPosition,
                        polygonIds,
                        pathSize,
                        ref result,
                        ref straightPathFlags,
                        ref vertexSize,
                        ref straightPathCount,
                        _maxPathSize
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
            while(math.distance(position, currentWayPosition) < navAspect.GetTraceRange()) {
                if (navAspect.NextWaypoint()) {
                    navAspect.IsStop = true;
                    break;
                }
                currentWayPosition = navAspect.GetCurrentWaypointPosition();
            }
            navAspect.SetTragetPosition(currentWayPosition);
            float3 direction = currentWayPosition - position;
            if (direction.x == 0f && direction.z == 0f) return;
            navAspect.IsTurnStop = false;
            // 이동
            navAspect.Position += math.normalize(direction) * navAspect.GetMoveSpeed() * deltaTime;
        }
    }
}

