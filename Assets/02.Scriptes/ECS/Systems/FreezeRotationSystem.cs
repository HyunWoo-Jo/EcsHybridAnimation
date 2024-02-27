using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Game.Ecs.ComponentAndTag;
using Unity.Mathematics;
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

            foreach (var (properties, entity) in SystemAPI.Query<FreezeRotationProperties>().WithEntityAccess()) {
                RefRW<PhysicsMass> mass = SystemAPI.GetComponentRW<PhysicsMass>(entity);
                float3 InverseInertia = mass.ValueRO.InverseMass;
                if (properties.freezeRotation.x) {
                    InverseInertia.x = 0;
                }
                if (properties.freezeRotation.y) {
                    InverseInertia.y = 0;
                }
                if (properties.freezeRotation.z) {
                    InverseInertia.z = 0;
                }
                mass.ValueRW.InverseInertia = InverseInertia;
            }

        }
    }
}
