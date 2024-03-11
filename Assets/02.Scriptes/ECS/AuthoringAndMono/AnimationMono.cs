using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AnimationMono : MonoBehaviour
    {
        [SerializeField] private GameObject _animatorObject;
        private class AnimationBaker : Baker<AnimationMono> {
            public override void Bake(AnimationMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimGameObjectReference { 
                    prefab = authoring._animatorObject,
                    animatorEntity = GetEntity(authoring._animatorObject, TransformUsageFlags.Dynamic)
                });
                
            }
        }
    }
}
