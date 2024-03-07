using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AnimationMono : MonoBehaviour
    {
        public GameObject obj;
        private class AnimationBaker : Baker<AnimationMono> {
            public override void Bake(AnimationMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimGameObjectReference { prefab = authoring.obj });
            }
        }
    }
}
