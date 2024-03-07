using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;
using Game.Utils;
using Game.Ecs.ComponentAndTag;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{

    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class ClickEventSystem : SystemBase
    {
        private Camera _mainCamera;
        private PhysicsWorld _physicsWorld;
        protected override void OnCreate() {
            base.OnCreate();
            _mainCamera = Camera.main;
        }
        protected override void OnDestroy() {
            base.OnDestroy();
        }
        protected override void OnUpdate() {
            // click event
            if (Input.GetMouseButton(0)) {
                ClickEvent();
            }
        }
        private void ClickEvent() {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
            if (RayCast(rayStart, rayEnd, out var raycastHit)) {
                foreach (var clickProperties in SystemAPI.Query<ClickEventProperties>()) {
                    foreach (var (tag, playerEntity) in SystemAPI.Query<PlayerTag>().WithEntityAccess()) {
                        SystemAPI.GetComponentRW<LocalTransform>(clickProperties.clickMovePointEntity).ValueRW.Position = raycastHit.Position;
                        var navAgentPropertiesRW = SystemAPI.GetComponentRW<NavAgentProperties>(playerEntity);
                        navAgentPropertiesRW.ValueRW.isStop = false;
                        navAgentPropertiesRW.ValueRW.isPathFinded = false;
                    }
                }
            }

        }

        private bool RayCast(float3 rayStart, float3 rayEnd, out Unity.Physics.RaycastHit raycastHit) {
            var raycastInput = new RaycastInput {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter {
                    GroupIndex = 0,
                    BelongsTo = (uint)LayerName.Ground,
                    CollidesWith = (uint)LayerName.All
                }
            };
            return _physicsWorld.CastRay(raycastInput, out raycastHit);
        }
        
    }
}
