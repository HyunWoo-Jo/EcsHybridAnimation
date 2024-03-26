using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Game.Utils;
namespace Game.Ecs.AuthoringsAndMono
{
    public class InputEventMono : MonoBehaviour
    {
        [SerializeField] private GameObject _clickMovePoint;
        private class InputEventBaker : Baker<InputEventMono> {
            public override void Bake(InputEventMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new InputEventProperties {
                    clickMovePointEntity = GetEntity(authoring._clickMovePoint, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
