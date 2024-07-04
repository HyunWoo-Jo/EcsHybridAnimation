using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.Aspect;
using Game.Ecs.ComponentAndTag;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Game.Data;
namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct AIStandardMeleeSystem : ISystem
    {
        private int _attackHash;
        private ComponentLookup<LocalTransform> _localTransformLookup;
        void OnCreate(ref SystemState state) {
            _attackHash = AnimationHash.Attack;
            _localTransformLookup = state.GetComponentLookup<LocalTransform>(true);
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            _localTransformLookup.Update(ref state);
            


            // Attack
            foreach (var (ai_attackProperties, navAspect, attackAspect)in SystemAPI.Query<RefRW<AI_AttackRangeProperties>, NavAgentAspect, AttackAspect>()) {
                Entity targetEntity = navAspect.GetTargetEntity();
                if (_localTransformLookup.HasComponent(targetEntity)) {
                    float3 targetPosition = _localTransformLookup[targetEntity].Position;
                    float distance = math.distance(navAspect.Position, targetPosition);
                    if (ai_attackProperties.ValueRO.currentTime < ai_attackProperties.ValueRO.attackDelayTime) {
                        ai_attackProperties.ValueRW.currentTime += SystemAPI.Time.DeltaTime;
                    }
                    int animationTagHash = attackAspect.GetCurrentAnimationTagHash();
                    if (distance < ai_attackProperties.ValueRO.attackRange) { // 어택 거리 확인
                        if (ai_attackProperties.ValueRO.currentTime >= ai_attackProperties.ValueRO.attackDelayTime) { // 공격 가능이면 공격 모션 실행
                            attackAspect.SetAttackAnimation(0);
                        }
                    } else if(animationTagHash != _attackHash){
                            navAspect.IsMoveStop = false;
                    }
                    // 공격 모션중 회전 중지, 딜레이 초기화
                    if (animationTagHash == _attackHash) {
                        navAspect.IsTurnStop = true;
                        ai_attackProperties.ValueRW.currentTime = 0;
                    } else {
                        navAspect.IsTurnStop = false;
                    }

                }
            }
        }
    }
}
