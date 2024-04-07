using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct AttackAspect : IAspect
    {
        private readonly DynamicBuffer<AttackRayElement> _attackBuffer;
        private readonly RefRW<AnimationProperties> _animationProperties;
        private readonly RefRW<StatusProperties> _statusProperties;

        public DynamicBuffer<AttackRayElement> GetRays() {
            return _attackBuffer;
        }

        public void AddBuffer(AttackRayElement attackRay) {
            _attackBuffer.Add(attackRay);
        }

        public void ClearBuffer() {
            _attackBuffer.Clear();
        }
        public int GetAttackAnimationCount() {
            return _animationProperties.ValueRO.attack;
        }
        public void Attack(int value) {
            _animationProperties.ValueRW.attack = value;
        }
    }
}
