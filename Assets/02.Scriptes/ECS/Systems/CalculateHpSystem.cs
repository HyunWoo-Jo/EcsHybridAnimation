using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Unity.Mathematics;
using Unity.Burst;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct CalculateHpSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }

        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            new CalculateHpJob().ScheduleParallel();
        }

        private partial struct CalculateHpJob : IJobEntity {
            private void Execute(StatusAspect statusAspect) {
                foreach(var item in statusAspect.GetDynamicBuffer()) {
                    statusAspect.Hp += item.value;
                }
                statusAspect.ClearHpBuffer();
            }
        }
    }
}
