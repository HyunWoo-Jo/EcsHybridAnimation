using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class StatusMono : MonoBehaviour
    {
        [SerializeField] private float _hp;
        [SerializeField] private float _aggressiveStrength;
        [SerializeField] private float _defensivePower;
        public class StatusBaker : Baker<StatusMono> {
            public override void Bake(StatusMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new StatusProperties{
                    entity = entity,
                    maxHp = authoring._hp,
                    currentHp = authoring._hp,
                    aggressiveStrength = authoring._aggressiveStrength,
                    defensivePower = authoring._defensivePower,
                });
                AddBuffer<HpCalculatorElement>(entity);
            }
        }

    }
}
