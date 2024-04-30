using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Game.Data;
using Game.Ecs.ComponentAndTag;
using UnityEngine.Animations;

namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct NewBehaviourScript : ISystem
    {
        private int dieHash;
        private int destroyPropertyId;
        void OnCreate(ref SystemState state) {
            dieHash = AnimationHash.Die;
            destroyPropertyId = ShaderPropertyId.DestroyAlpha;
        }
        void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecbEnity = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var job = new DieJob {
                ecb = ecbEnity.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                dieHash = dieHash,
                destroyPropertyId = destroyPropertyId,
                deltaTime = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel(state.Dependency);
            job.Complete();
            state.CompleteDependency();
          
        }

        [BurstCompile]
        private partial struct DieJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            [ReadOnly]
            public int dieHash;
            [ReadOnly]
            public int destroyPropertyId;
            [ReadOnly]
            public float deltaTime;
            [BurstCompile]

            private void Execute(RefRO<StatusProperties> statusPro, RefRW<AnimationProperties> animPro, DynamicBuffer<FloatMaterialElement> floatBuffer, [EntityIndexInQuery] int sortKey)
            {
                if (statusPro.ValueRO.currentHp <= 0) {
                    if (animPro.ValueRO.currentAnimationTagHash != dieHash) {
                        ecb.SetComponentEnabled<MoveProperties>(sortKey, statusPro.ValueRO.entity, false);
                        ecb.SetComponentEnabled<RotationProperties>(sortKey, statusPro.ValueRO.entity, false);
                        animPro.ValueRW.triggerHash = dieHash;
                        animPro.ValueRW.isTrigger = true;
                    } else if (animPro.ValueRO.currentAnimationNormalizedTime > 0.9f) {
                        for(int i=0;i< floatBuffer.Length; i++) {
                            if (floatBuffer[i].nameId == destroyPropertyId) {
                                var element = floatBuffer[i];
                                element.value += deltaTime;
                                floatBuffer[i] = element;
                                break;
                            }
                        }
                    }
                }

            }
        }
    }
}
