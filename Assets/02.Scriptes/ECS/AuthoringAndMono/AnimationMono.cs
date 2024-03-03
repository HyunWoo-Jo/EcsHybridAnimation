using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AnimationMono : MonoBehaviour
    {
        private Animator _animator => GetComponent<Animator>();
        private class AnimationBaker : Baker<AnimationMono> {
            public override void Bake(AnimationMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimationReference { 
                    animator = authoring._animator 
                });
            }
        }
    }
}
