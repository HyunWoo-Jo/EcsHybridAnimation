using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.Aspect;
using Unity.Mathematics;
namespace Game.Ecs.System
{
    public partial struct MoveSystem : ISystem {

        void OnCreate(ref SystemState state) {

        }

        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            new MoveJob { deltaTime = SystemAPI.Time.DeltaTime }.ScheduleParallel();
        }
        [BurstCompile]
        private partial struct MoveJob : IJobEntity {
            public float deltaTime;
            [BurstCompile]
            private void Execute(MoveAspect moveAsepct) {
                if (moveAsepct.IsStop) return;
                float3 acc = moveAsepct.Direction * deltaTime * moveAsepct.AccelerateSpeed;
                float3 moveValue = moveAsepct.Velocity + acc;
                moveAsepct.Velocity = new float3(math.min(moveValue.x, moveAsepct.MaxSpeed), moveValue.y, math.min(moveValue.z, moveAsepct.MaxSpeed));

            }
        }
    }
}
