using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Game.Data;
using Game.Ecs.ComponentAndTag;

namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct NewBehaviourScript : ISystem
    {
        private int dieHash;
        void OnCreate(ref SystemState state) {
            dieHash = AnimationHash.Die;
        }
        void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecbEnity = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var job = new DieJob {
                ecb = ecbEnity.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                dieHash = dieHash
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
            [BurstCompile]
            private void Execute(RefRO<StatusProperties> statusPro, RefRW<AnimationProperties> animPro, [EntityIndexInQuery] int sortKey)
            {
                if (statusPro.ValueRO.currentHp <= 0 && animPro.ValueRO.currentAnimationTagHash != dieHash)
                {
                    ecb.SetComponentEnabled<MoveProperties>(sortKey, statusPro.ValueRO.entity, false);
                    ecb.SetComponentEnabled<RotationProperties>(sortKey, statusPro.ValueRO.entity, false);
                    animPro.ValueRW.triggerHash = dieHash;
                    animPro.ValueRW.isTrigger = true;
                }
            }
        }
    }
}
