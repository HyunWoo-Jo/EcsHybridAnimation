using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AnimationMono : MonoBehaviour
    {
        [SerializeField] private GameObject _animatorModel_prefab;
        [SerializeField] private GameObject _animatorModel; // 삭제될 자식 오브젝트
        private class AnimationBaker : Baker<AnimationMono> {
            public override void Bake(AnimationMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimGameObjectReference { 
                    prefab = authoring._animatorModel_prefab,
                    animatorEntity = GetEntity(authoring._animatorModel, TransformUsageFlags.Dynamic)
                });
                AddComponent(entity, new AnimationProperties {
                    attack = -1,
                    preAttack = -1
                    
                });
                
            }
        }
    }
}
