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
            foreach(var (navAspect, animPro) in SystemAPI.Query<NavAgentAspect, RefRW<AnimationProperties>>().WithAll<PlayerTag>()) {
                // ¿Ãµø animation 
                //if (navAspect.GetIsStop()) {
                //    animPro.ValueRW.isMove = false;
                //} else {
                //    animPro.ValueRW.isMove = true;
                //}
            }
        }
    }
}
