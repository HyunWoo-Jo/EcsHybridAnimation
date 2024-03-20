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

        private int attackCount = -1;
        private bool _isLeftClick;
        private bool _isRightClick;

        
        protected override void OnCreate() {
            base.OnCreate();
            _mainCamera = Camera.main;

            _leftClick_listener = MoveClick;
            _rightClick_listener = AttackClick;

            _isLeftClick = false;
            _isRightClick = false;
        }
        protected override void OnDestroy() {
            base.OnDestroy();
        }
        protected override void OnUpdate() {
            // right Click
            if (Input.GetMouseButton(1)) {
                _rightClick_listener?.Invoke();
                _isRightClick = true;
            }
            // Left Click
            else if (Input.GetMouseButton(0)) {
                _leftClick_listener?.Invoke();
                _isLeftClick = true;
            }
            // right
            if (!Input.GetMouseButton(1)) {
                _isRightClick = false;
            }
            // left
            if (!Input.GetMouseButton(0)) {
                _isLeftClick = false;
            }

            ContinueousAttack();
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
                    foreach (var playerAspect in SystemAPI.Query<PlayerAspect>()) {
                        SystemAPI.GetComponentRW<LocalTransform>(clickProperties.clickMovePointEntity).ValueRW.Position = raycastHit.Position;
                        int attackCount = playerAspect.GetAttackCount();
                        if (attackCount == -1) {
                            playerAspect.SetStop(false);
                            playerAspect.SetPathFinded(false);
                        }
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
            Debug.DrawRay(rayStart, rayEnd, Color.blue, 1f);
#endif
            if (RayCast(rayStart, rayEnd, out var raycastHit)) {
                foreach (var playerAspect in SystemAPI.Query<PlayerAspect>()) {
                    
                    playerAspect.SetStop(true);
                    int attackCount = playerAspect.GetAttackCount();
                    if(attackCount == -1) {
                        playerAspect.Attack(0);
                    }
                    playerAspect.SetTargetPosition(raycastHit.Position);
                }
            }
        }
        // 공격 버튼이 계속해서 눌려있나 확인 후 계속해서 눌러져있으면 연속 공격 실행
        private void ContinueousAttack() {
            foreach (var (animRef, playAspect) in SystemAPI.Query<AnimationReference, PlayerAspect>().WithAll<PlayerTag>()) {
                int attackCount = playAspect.GetAttackCount();
                if (attackCount >= 0 && attackCount < 2) { // attack 동작 0부터 1일경우
                    if (animRef.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_"+ attackCount.ToString())) { // 모션이 attack 동작일 경우
                        if(animRef.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7) { // 모션 70 이상 89 이하 상태에서
                            if(_isRightClick) { // 오른쪽 클릭이 되어있으면
                                playAspect.IsContinueousAttack = true;
                                playAspect.Attack(attackCount + 1);
                            }
                        } else {// 모션 70 이하 일 경우
                            playAspect.IsContinueousAttack = false;
                        }
                    } 
                } else { // 동작이 제한범위 밖일경우
                    playAspect.IsContinueousAttack = false;
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
