using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using PlasticPipe.PlasticProtocol.Client;
namespace Game.Ecs.AuthoringsAndMono
{
    public class ObjectSplitMono : MonoBehaviour
    {
        private class ObjectSplitBaker : Baker<ObjectSplitMono> {
            public override void Bake(ObjectSplitMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new ObjectSplitProperties { });
            }
        }
    }
}
