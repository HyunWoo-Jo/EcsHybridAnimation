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
    public partial class InputEventSystem : SystemBase
    {
        private Camera _mainCamera;
        private PhysicsWorld _physicsWorld;

        private Action _leftClick_listener;
        private Action _rightClick_listener;

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
        /// Ŭ�� �̵� 
        /// </summary>
        private void MoveClick() {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
            var raycastInput = new RaycastInput {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter {
                    GroupIndex = 0,
                    BelongsTo = (uint)LayerName.Ground,
                    CollidesWith = (uint)LayerName.All
                }
            };
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
            if (_physicsWorld.CastRay(raycastInput, out var raycastHit)) {
                foreach (var clickProperties in SystemAPI.Query<InputEventProperties>()) {
                    foreach (var (playerAspect, attackAspect) in SystemAPI.Query<PlayerAspect, AttackAspect>().WithAll<PlayerTag>()) {
                        SystemAPI.GetComponentRW<LocalTransform>(clickProperties.clickMovePointEntity).ValueRW.Position = raycastHit.Position;
                        int attackCount = attackAspect.GetAttackAnimationCount();
                        if (attackCount == -1) {
                            playerAspect.SetStop(false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Ŭ�� ����
        /// </summary>
        private void AttackClick() {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
            var raycastInput = new RaycastInput {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter {
                    GroupIndex = 0,
                    BelongsTo = (uint)LayerName.Ground,
                    CollidesWith = (uint)LayerName.All
                }
            };
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.blue, 1f);
#endif
            if (_physicsWorld.CastRay(raycastInput, out var raycastHit)) {
                foreach (var (playerAspect, attackAspect) in SystemAPI.Query<PlayerAspect, AttackAspect>().WithAll<PlayerTag>()) {
                    
                    playerAspect.SetStop(true);
                    int attackCount = attackAspect.GetAttackAnimationCount();
                    if(attackCount == -1) {
                        attackAspect.Attack(0);
                       
                    }
                    playerAspect.SetTargetPosition(raycastHit.Position);
                }
            }
        }
        // ���� ��ư�� ����ؼ� �����ֳ� Ȯ�� �� ����ؼ� ������������ ���� ���� ����
        private void ContinueousAttack() {
            foreach (var (animRef, playerAspect, attackAspect) in SystemAPI.Query<AnimationReference, PlayerAspect, AttackAspect>().WithAll<PlayerTag>()) {
                int attackCount = attackAspect.GetAttackAnimationCount();
                if (attackCount >= 0 && attackCount < 2) { // attack ���� 0���� 1�ϰ��
                    if (animRef.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_"+ attackCount.ToString())) { // ����� attack ������ ���
                        if(animRef.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7) { // ��� 70 �̻� 89 ���� ���¿���
                            if(_isRightClick) { // ������ Ŭ���� �Ǿ�������
                                playerAspect.IsContinueousAttack = true;
                                attackAspect.Attack(attackCount + 1);
                            }
                        } else {// ��� 70 ���� �� ���
                            playerAspect.IsContinueousAttack = false;
                        }
                    } 
                } else { // ������ ���ѹ��� ���ϰ��
                    playerAspect.IsContinueousAttack = false;
                }
            }
        }
        
    }
}
