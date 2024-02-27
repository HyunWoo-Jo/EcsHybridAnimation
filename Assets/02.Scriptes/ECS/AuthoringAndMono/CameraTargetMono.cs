using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class CameraTargetMono : MonoBehaviour
    {

        private class CameraTargetBaker : Baker<CameraTargetMono> {
            public override void Bake(CameraTargetMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent<CameraTargetTag>(entity);
            }
        }
    }
}
