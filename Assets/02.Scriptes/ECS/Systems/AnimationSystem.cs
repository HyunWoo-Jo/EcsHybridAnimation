using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{
    public partial struct AnimationSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {

            // Hybrid Object�� Entity Position Rotation ��ġ 

            foreach(var animAspect in SystemAPI.Query<AnimationAspect>()) {

            }

        }
    }
}
