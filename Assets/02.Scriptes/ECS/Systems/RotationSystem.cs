using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state) {

        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {
        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;
            new RotationJob { 
                deltaTime = deltaTime 
            }.ScheduleParallel();

        }

        private partial struct RotationJob : IJobEntity {
            public float deltaTime;
            private void Execute(RotationAspect turnAspect) {
                if (turnAspect.IsStop) return;

                float3 direction = turnAspect.TargetPosition - turnAspect.Position;
                direction.y = 0;
                if (direction.x == 0f && direction.z == 0f) return;
                // È¸Àü
                quaternion targetRotation = quaternion.LookRotation(direction, new float3(0, 1, 0));
                turnAspect.Rotation = math.slerp(turnAspect.Rotation, targetRotation, turnAspect.GetRotationSpeed() * deltaTime);
            }
        }
    }
}
