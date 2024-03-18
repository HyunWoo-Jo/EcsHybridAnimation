using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class RotationMono : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        private class RotationBaker : Baker<RotationMono> {
            public override void Bake(RotationMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new RotationProperties {
                    isStop = true,
                    rotationSpeed = authoring._rotationSpeed,
                });
            }
        }

    }
}
