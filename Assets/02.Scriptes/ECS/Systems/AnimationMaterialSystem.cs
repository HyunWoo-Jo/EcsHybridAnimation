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
    public partial struct AnimationMaterialSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {
            //Hybrid Object와 Entity를 맵핑
            // Animation Properties의 정보를 Hybrid Animator에 전달
            foreach (var (animRef, animPro, transform) in SystemAPI.Query<AnimationReference, RefRW<AnimationProperties>, RefRO<LocalTransform>>()) {
                // Hybrid Object와 Entity Position Rotation 맵핑
                animRef.transform.position = transform.ValueRO.Position;
                animRef.transform.rotation = transform.ValueRO.Rotation;

                /// Animator
                // attack 모션이 끝나면 초기화
                int currentAnimationHash = animRef.animator.GetCurrentAnimatorStateInfo(0).tagHash;
                int nextAnimationHash = animRef.animator.GetNextAnimatorStateInfo(0).tagHash;

                // Animation Properties에 animtionState 저장
                animPro.ValueRW.currentAnimationTagHash = currentAnimationHash;
                animPro.ValueRW.currentAnimationNormalizedTime = animRef.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                if (animPro.ValueRO.preAttack >= 0 && animPro.ValueRO.currentAnimationNormalizedTime > 0.9f && !(nextAnimationHash == AnimationHash.Attack)) {
                    animPro.ValueRW.attack = -2;
                }
                // animation 1번만 실행 되도록 설정
                if (animPro.ValueRW.attack == -2 && (currentAnimationHash == AnimationHash.Idle || currentAnimationHash == AnimationHash.Walk) && !(nextAnimationHash == AnimationHash.Attack)) {
                    animPro.ValueRW.attack = -1;
                }

                // Move 전달
                if (animPro.ValueRO.preMove != animPro.ValueRO.isMove) {
                    animPro.ValueRW.preMove = animPro.ValueRO.isMove;
                    animRef.animator.SetBool(AnimationHash.Walk, animPro.ValueRO.isMove);
                }
                // Attack 전달
                if (animPro.ValueRO.preAttack != animPro.ValueRO.attack) {
                    animPro.ValueRW.preAttack = animPro.ValueRO.attack;
                    animRef.animator.SetInteger(AnimationHash.Attack, animPro.ValueRO.attack);
                }
                // Trigger 전달
                if (animPro.ValueRO.isTrigger) {
                    animPro.ValueRW.isTrigger = false;
                    animRef.animator.SetTrigger(animPro.ValueRO.triggerHash);
                }
            }
            // Material 정보 전송
            foreach(var (materialRef, floatBuffer) in SystemAPI.Query<MaterialReference, DynamicBuffer<FloatMaterialElement>>())
            {
                if (materialRef.material is null) continue;
                foreach(var element in floatBuffer)
                {
                    materialRef.material.SetFloat(element.nameId, element.value);
                }
            }

            new WalkAnimationJob { }.ScheduleParallel();
            

        }

        // 이동 animation
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
