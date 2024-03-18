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
using System;
namespace Game.Ecs.System
{

    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class ClickEventSystem : SystemBase
    {
        private Camera _mainCamera;
        private PhysicsWorld _physicsWorld;

        private Action _leftClick_listener;
        private Action _rightClick_listener;
        protected override void OnCreate() {
            base.OnCreate();
            _mainCamera = Camera.main;

            _leftClick_listener = MoveClick;
            _rightClick_listener = AttackClick;
        }
        protected override void OnDestroy() {
            base.OnDestroy();
        }
        protected override void OnUpdate() {
            // Left Click
            if (Input.GetMouseButton(0)) {
                _leftClick_listener?.Invoke();
            }
            // right Click
            if (Input.GetMouseButton(1)) {
                _rightClick_listener?.Invoke();
            }
        }
        /// <summary>
        /// 클릭 이동 
        /// </summary>
        private void MoveClick() {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
            if (RayCast(rayStart, rayEnd, out var raycastHit)) {
                foreach (var clickProperties in SystemAPI.Query<ClickEventProperties>()) {
                    foreach (var playerAsepct in SystemAPI.Query<PlayerAspect>()) {
                        SystemAPI.GetComponentRW<LocalTransform>(clickProperties.clickMovePointEntity).ValueRW.Position = raycastHit.Position;
                        playerAsepct.SetStop(false);
                        playerAsepct.SetPathFinded(false);
                    }
                }
            }
        }
        /// <summary>
        /// 클릭 공격
        /// </summary>
        private void AttackClick() {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
            if (RayCast(rayStart, rayEnd, out var raycastHit)) {
                foreach (var playerAspect in SystemAPI.Query<PlayerAspect>()) {
                    playerAspect.SetStop(true);
                    playerAspect.Attack();
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
