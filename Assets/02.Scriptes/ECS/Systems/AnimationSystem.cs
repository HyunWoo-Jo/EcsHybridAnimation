using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Game.Ecs.ComponentAndTag;
using Unity.Transforms;
namespace Game.Ecs.System
{
    public partial struct AnimationSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {

            // Hybrid Object¿Í Entity Position Rotation ¸ÅÄ¡ 
            foreach(var (animRef, transform) in SystemAPI.Query<AnimationReference, LocalTransform>()) {
                animRef.transform.position = transform.Position;
                animRef.transform.rotation = transform.Rotation;
            }

        }
    }
}
