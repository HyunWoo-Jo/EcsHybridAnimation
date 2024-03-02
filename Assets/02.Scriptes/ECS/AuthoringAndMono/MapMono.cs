using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class MapMono : MonoBehaviour {

        private class MapBaker : Baker<MapMono> {
            public override void Bake(MapMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
                AddComponent<MapTag>(entity);
            }
        }
    }
}
