using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Game.Ecs.ComponentAndTag;
using Unity.Mathematics;
using Game.Ecs.Aspect;
namespace Game.Ecs.System {
    [BurstCompile]
    
    public partial struct FreezeRotationSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state) {

        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            new FreezeJob { }.ScheduleParallel();
        }

        [BurstCompile]
        private partial struct FreezeJob : IJobEntity {
            [BurstCompile]
            private void Execute(FreezeRotationAspect freezeAspect) {
                float3 inverseInertia = freezeAspect.InverseInertia;
                bool3 isFreezeRotation = freezeAspect.GetIsFreezeRotation();
                if (isFreezeRotation.x) {
                    inverseInertia.x = 0;
                }
                if (isFreezeRotation.y) {
                    inverseInertia.y = 0;
                }
                if (isFreezeRotation.z) {
                    inverseInertia.z = 0;
                }
                freezeAspect.InverseInertia = inverseInertia;
            }
        }
    }
}
