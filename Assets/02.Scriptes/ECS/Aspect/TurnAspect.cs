using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;

namespace Game.Ecs.Aspect
{
    public readonly partial struct RotationAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<RotationProperties> _turnProperties;
        
        public quaternion Rotation {
            get { return _localTransform.ValueRO.Rotation; }
            set { _localTransform.ValueRW.Rotation = value; }
        }

        public float3 TargetPosition {
            get { return _turnProperties.ValueRO.targetPosition; }
            set { _turnProperties.ValueRW.targetPosition = value; }
        }

        public float3 Position {
            get { return _localTransform.ValueRO.Position; }
            set { _localTransform.ValueRW.Position = value; }
        }

        public bool IsStop {
            get { return _turnProperties.ValueRO.isStop; }
            set { _turnProperties.ValueRW.isStop = value; }
        }

        public float GetRotationSpeed() {
            return _turnProperties.ValueRO.rotationSpeed;
        }

    }
}
