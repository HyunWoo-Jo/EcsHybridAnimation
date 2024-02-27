using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Unity.Mathematics;
namespace Game.Ecs.AuthoringsAndMono
{
    public class FreezeRotationMono : MonoBehaviour
    {
        public bool3 freezeRotations;
        private class FreezeRotationBaker : Baker<FreezeRotationMono> {
            public override void Bake(FreezeRotationMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new FreezeRotationProperties { freezeRotation = authoring.freezeRotations });
            }
        }
    }
}
