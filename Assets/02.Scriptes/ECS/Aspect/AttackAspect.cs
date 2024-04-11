using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct AttackAspect : IAspect
    {
        private readonly RefRO<LocalTransform> _localTransform;
        private readonly DynamicBuffer<AttackRayElement> _attackBuffer;
        private readonly RefRW<AnimationProperties> _animationProperties;
        private readonly RefRW<StatusProperties> _statusProperties;
        private readonly RefRO<AttackRayBlobReference> _attackRayBlobRef;
        public DynamicBuffer<AttackRayElement> GetRays() {
            return _attackBuffer;
        }

        private void AddBuffer(AttackRayElement attackRay) {
            _attackBuffer.Add(attackRay);
        }

        public void ClearBuffer() {
            _attackBuffer.Clear();
        }
        public int GetAttackAnimationCount() {
            return _animationProperties.ValueRO.attack;
        }

        public float3 LocalToGlobal(float3 childPosition) {
            return _localTransform.ValueRO.TransformPoint(childPosition);
        }

        public float3 TransformDirection(float3 direciton) {
            return _localTransform.ValueRO.TransformDirection(direciton);
        }

        public void Attack(int value) {
            _animationProperties.ValueRW.attack = value;

            for(int i = 0; i < _attackRayBlobRef.ValueRO.blobRef.Value.attackBlobTwoArray[value].Value.Length; i++) {
                AddBuffer(new AttackRayElement { attackRayBuffer = _attackRayBlobRef.ValueRO.blobRef.Value.attackBlobTwoArray[value].Value[i] });
            }
        }
        
       
    }
}
