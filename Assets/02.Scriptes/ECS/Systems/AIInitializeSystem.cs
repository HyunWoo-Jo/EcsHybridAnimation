using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.System
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [BurstCompile]
    public partial struct AIInitializeSystem : ISystem {
        void OnCreate(ref SystemState state) {

        }

        void OnDestroy(ref SystemState state) {

        }
      
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (AI_attackPro, entity) in SystemAPI.Query<AI_AttackRangeProperties>().WithEntityAccess().WithAll<NewAITag>()) {
                ecb.SetComponentEnabled<RotationProperties>(entity, false);
                ecb.SetComponentEnabled<NavAgentProperties>(entity, false);
                ecb.SetComponentEnabled<AI_AttackRangeProperties>(entity, false);
                ecb.RemoveComponent<NewAITag>(entity);
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
