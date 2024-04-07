using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class AttackMono : MonoBehaviour
    {
        private class AttackBaker : Baker<AttackMono> {
            public override void Bake(AttackMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddBuffer<AttackRayElement>(entity);
            }
        }
    }
}
