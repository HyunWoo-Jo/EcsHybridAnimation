using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Game.Utils;
namespace Game.Ecs.AuthoringsAndMono
{
    public class ClickEventMono : MonoBehaviour
    {
        [SerializeField] private GameObject _clickMovePoint;
        [SerializeField] private GameObject _playerObject;
        private class ClickEventBaker : Baker<ClickEventMono> {
            public override void Bake(ClickEventMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new ClickEventProperties {
                    clickMovePointEntity = GetEntity(authoring._clickMovePoint, TransformUsageFlags.Dynamic),
                    playerEntity = GetEntity(authoring._playerObject, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
