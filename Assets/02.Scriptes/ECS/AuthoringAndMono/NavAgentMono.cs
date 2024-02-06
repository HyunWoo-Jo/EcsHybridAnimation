using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
namespace Game.Ecs
{
    public class NavAgentMono : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _moveSpeed;
        private class NavAgentBaker : Baker<NavAgentMono> {
            public override void Bake(NavAgentMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                Entity targetEntity = GetEntity(authoring._targetTransform, TransformUsageFlags.Dynamic);
                AddComponent(entity, new NavAgentProperties {
                    moveSpeed = authoring._moveSpeed,
                    targetEntity = targetEntity
                });
                AddBuffer<WaypointBuffer>(entity);
            }
        }
    }

    
}
