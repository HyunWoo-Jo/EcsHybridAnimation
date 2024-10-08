using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;

namespace Game.Ecs.AuthoringsAndMono
{
    public class AI_AttackMono : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackDelayTime;
        [SerializeField] private GameObject _attackTargetObject;

        [Header("Trace")]
        [SerializeField] private float _traceRange;
        private class AIBaker : Baker<AI_AttackMono> {
            public override void Bake(AI_AttackMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new NewAITag { });

                AddComponent(entity, new AI_AttackRangeProperties {
                    attackRange = authoring._attackRange,
                    attackDelayTime = authoring._attackDelayTime,
                    targetEntity = GetEntity(authoring._attackTargetObject, TransformUsageFlags.Dynamic)
                });

                AddComponent(entity, new AI_TraceRangeProperties {
                    traceRange = authoring._traceRange
                });
            }
        }
    }
}
