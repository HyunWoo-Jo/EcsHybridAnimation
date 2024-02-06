using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace Game.Ecs
{
    public readonly partial struct NavAgentAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<NavAgentProperties> _navAgentProperties;
        private readonly DynamicBuffer<WaypointBuffer> _waypointBuffer;

        public bool IsStop() {
            return _navAgentProperties.ValueRO.isStop;
        }

        public float3 GetPosition() {
            return _localTransform.ValueRO.Position;
        }

        public Entity GetTargetEntity() {

            return _navAgentProperties.ValueRO.targetEntity;
        }
    }
}
