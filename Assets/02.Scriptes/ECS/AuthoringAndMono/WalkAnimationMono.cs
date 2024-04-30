using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    [RequireComponent(typeof(AnimationMaterialMono))]
    public class WalkAnimationMono : MonoBehaviour
    {
        
        private class WalkAnimationBaker : Baker<WalkAnimationMono> {
            public override void Bake(WalkAnimationMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new WalkAnimationTag { });
            }
        }
    }
}
