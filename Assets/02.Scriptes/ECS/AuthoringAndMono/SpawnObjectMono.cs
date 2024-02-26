using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono 
{
    public class SpawnObjectMono : MonoBehaviour
    {
        [SerializeField] TransformUsageFlags type;
        private class SpawnObjectBaker : Baker<SpawnObjectMono> {
            public override void Bake(SpawnObjectMono authoring) {
                var entity = GetEntity(authoring.gameObject, authoring.type);
                AddComponent<MapTag>(entity);
            }
        }

    }
    
   
 
}
