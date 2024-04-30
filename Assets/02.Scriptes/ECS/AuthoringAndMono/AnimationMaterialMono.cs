using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AnimationMaterialMono : MonoBehaviour
    {
        [SerializeField] private GameObject _animatorModel_prefab;
        [SerializeField] private GameObject _animatorModel; // 삭제될 자식 오브젝트
        private class AnimationMaterialBaker : Baker<AnimationMaterialMono> {
            public override void Bake(AnimationMaterialMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new AnimGameObjectReference { 
                    prefab = authoring._animatorModel_prefab,
                    animatorEntity = GetEntity(authoring._animatorModel, TransformUsageFlags.Dynamic)
                });
                AddComponent(entity, new AnimationProperties {
                    attack = -1,
                    preAttack = -1
                    
                });
                AddComponentObject(entity, new MaterialReference { material = null});
            }
        }
    }
}
