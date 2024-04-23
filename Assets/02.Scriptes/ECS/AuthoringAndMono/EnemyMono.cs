using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    [RequireComponent(typeof(AttackMono))]
    [RequireComponent(typeof(NavAgentMono))]
    [RequireComponent(typeof(AnimationMono))]
    [RequireComponent(typeof(StatusMono))]
    
    public class EnemyMono : MonoBehaviour
    {
        private class EnemyBaker : Baker<EnemyMono> {
            public override void Bake(EnemyMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyTag { });
            }
        }
    }
}
