using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;

namespace Game.Ecs.AuthoringsAndMono
{
    public class AIMono : MonoBehaviour
    {
        internal bool isAttackRange;
        [HideInInspector] internal float attackRange;

        private class AIBaker : Baker<AIMono> {
            public override void Bake(AIMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                if(authoring.isAttackRange) {
                    AddComponent(entity, new AI_AttackRangeProperties { attackRange = authoring.attackRange });
                }
            }
        }
    }
}
