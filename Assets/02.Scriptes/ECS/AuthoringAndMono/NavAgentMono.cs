using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    public class NavAgentMono : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _traceRange;
#if UNITY_EDITOR
        private void Awake() {
            if (_targetTransform == null) Debug.Log(gameObject.name + ": Not Target");
        }
#endif
        private class NavAgentBaker : Baker<NavAgentMono> {
            public override void Bake(NavAgentMono authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                Entity targetEntity = GetEntity(authoring._targetTransform, TransformUsageFlags.Dynamic);
                AddComponent(entity, new NavAgentProperties {
                    moveSpeed = authoring._moveSpeed,
                    targetEntity = targetEntity,
                    traceRange = authoring._traceRange,
                    rotationSpeed = authoring._rotationSpeed
                });
                AddBuffer<WaypointBuffer>(entity);
            }
        }
    }

    
}
