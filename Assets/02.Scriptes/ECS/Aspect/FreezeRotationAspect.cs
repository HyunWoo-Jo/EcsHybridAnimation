using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct FreezeRotationAspect : IAspect
    {
        private readonly RefRW<PhysicsMass> _physicsMass;
        private readonly RefRO<FreezeRotationProperties> _freezeRotationProperties;

        public float3 InverseInertia {
            get { return _physicsMass.ValueRO.InverseInertia; }
            set { _physicsMass.ValueRW.InverseInertia = value; }

        }
        public bool3 GetIsFreezeRotation() {
            return _freezeRotationProperties.ValueRO.freezeRotation;
        }

    }
}
