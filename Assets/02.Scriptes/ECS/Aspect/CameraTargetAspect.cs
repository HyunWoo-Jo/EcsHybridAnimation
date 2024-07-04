using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct CameraTargetAspect : IAspect
    {
        private readonly RefRO<LocalTransform> _localTransform;
#pragma warning disable CS0414
        private readonly RefRO<CameraTargetTag> _targetTag;
#pragma warning restore CS0414
        public float3 GetPosition() {
            return _localTransform.ValueRO.Position;
        }
        
    }
}
