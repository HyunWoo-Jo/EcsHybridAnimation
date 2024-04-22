using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.Aspect;
using Game.Ecs.ComponentAndTag;
using Unity.Burst;

namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct AIStandardMeleeSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state) {

        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {

        }
    }
}
