using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct AttackAspect : IAspect
    {
        private readonly RefRW<AttackBuffer> _attackBuffer;



        public DynamicBuffer<AttackRayElement> GetRays() {
            return _attackBuffer.ValueRO.attackRayBuffer;
        }

        public void AddBuffer(AttackRayElement attackRay) {
            _attackBuffer.ValueRW.attackRayBuffer.Add(attackRay);
        }

        public void ClearBuffer() {
            _attackBuffer.ValueRW.attackRayBuffer.Clear();
        }
    }
}
