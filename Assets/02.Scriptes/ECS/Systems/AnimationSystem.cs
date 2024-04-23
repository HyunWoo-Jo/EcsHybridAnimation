using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.ComponentAndTag;
using Unity.Transforms;
using Game.Data;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{   
    /// <summary>
    /// Hybrid Object�� Entity�� ���� 
    /// Animation Properties�� ������ Hybrid Animator�� �����ϴ� ����
    /// </summary>
    public partial struct AnimationSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {
            foreach (var (animRef, animPro, transform) in SystemAPI.Query<AnimationReference, RefRW<AnimationProperties>, RefRO<LocalTransform>>()) {
                // Hybrid Object�� Entity Position Rotation ����
                animRef.transform.position = transform.ValueRO.Position;
                animRef.transform.rotation = transform.ValueRO.Rotation;

                /// Animator
                // attack ����� ������ �ʱ�ȭ
                int currentAnimationHash = animRef.animator.GetCurrentAnimatorStateInfo(0).tagHash;
                int nextAnimationHash = animRef.animator.GetNextAnimatorStateInfo(0).tagHash;

                // Animation Properties�� animtionState ����
                animPro.ValueRW.currentAnimationTagHash = currentAnimationHash;
                animPro.ValueRW.currentAnimationNormalizedTime = animRef.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animPro.ValueRO.preAttack >= 0 && animPro.ValueRO.currentAnimationNormalizedTime > 0.9f && !(nextAnimationHash == AnimationHash.Attack)) {
                    animPro.ValueRW.attack = -2;
                }
                // animation 1���� ���� �ǵ��� ����
                if (animPro.ValueRW.attack == -2 && (currentAnimationHash == AnimationHash.Idle || currentAnimationHash == AnimationHash.Walk) && !(nextAnimationHash == AnimationHash.Attack)) {
                    animPro.ValueRW.attack = -1;
                }

                // Move ����
                if (animPro.ValueRO.preMove != animPro.ValueRO.isMove) {
                    animPro.ValueRW.preMove = animPro.ValueRO.isMove;
                    animRef.animator.SetBool(AnimationHash.Walk, animPro.ValueRO.isMove);
                }
                // Attack ����
                if (animPro.ValueRO.preAttack != animPro.ValueRO.attack) {
                    animPro.ValueRW.preAttack = animPro.ValueRO.attack;
                    animRef.animator.SetInteger(AnimationHash.Attack, animPro.ValueRO.attack);
                }
                // Trigger ����
                if (animPro.ValueRO.isTrigger) {
                    animPro.ValueRW.isTrigger = false;
                    animRef.animator.SetTrigger(animPro.ValueRO.triggerHash);
                }
            }

            new WalkAnimationJob { }.ScheduleParallel();
            

        }

        // �̵� animation
        [BurstCompile]
        private partial struct WalkAnimationJob : IJobEntity {
            [BurstCompile]
            private void Execute(NavAgentAspect navAspect, RefRW<AnimationProperties> animPro, WalkAnimationTag tag) {
                if (navAspect.GetIsStop()) {
                    animPro.ValueRW.isMove = false;
                } else {
                    animPro.ValueRW.isMove = true;
                }
            }
        }
    }
}
