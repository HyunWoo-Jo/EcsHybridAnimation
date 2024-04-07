using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.Aspect;
using Game.Ecs.ComponentAndTag;

namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct PlayerSystem : ISystem {
        [BurstCompile]
        void OnCreate(ref SystemState state) {

        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            new PlayerJob { }.ScheduleParallel();
        }

        [BurstCompile]
        private partial struct PlayerJob : IJobEntity {
            [BurstCompile]
            private void Execute(PlayerAspect playerAspect) {
                if (playerAspect.GetIsStop()) {
                    playerAspect.Walk(false);
                } else {
                    playerAspect.Walk(true);
                }

                
            }
        }
    }
}
