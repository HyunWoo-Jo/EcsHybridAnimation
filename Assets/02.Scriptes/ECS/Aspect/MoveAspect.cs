using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct MoveAspect : IAspect 
    {
        private readonly RefRW<MoveProperties> _moveProperties;
        private readonly RefRW<PhysicsVelocity> _physicsVelocity;

        public bool IsStop {
            get { return _moveProperties.ValueRO.isStop; }
            set { _moveProperties.ValueRW.isStop = value; }
        }

        public float3 Direction {
            get { return _moveProperties.ValueRO.direction; }
            set { _moveProperties.ValueRW.direction = value; }
        }
        public float AccelerateSpeed {
            get { return _moveProperties.ValueRO.accelerateSpeed; }
            set { _moveProperties.ValueRW.accelerateSpeed = value; }
        }

        public float MaxSpeed {
            get { return _moveProperties.ValueRO.maxSpeed; }
            set { _moveProperties.ValueRW.maxSpeed = value; }
        }
        public float3 Velocity {
            get { return _physicsVelocity.ValueRO.Linear; }
            set { _physicsVelocity.ValueRW.Linear = value; }
        }
    }
}
