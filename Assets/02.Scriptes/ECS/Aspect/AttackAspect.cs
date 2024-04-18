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
        private readonly RefRW<AttackProperties> _attackProperties;

        public bool IsAttackAble {
            get { return _attackProperties.ValueRO.isAttackAble; }
            set { _attackProperties.ValueRW.isAttackAble = value; }
        }
       
        public DynamicBuffer<AttackRayElement> GetRays() {
            return _attackBuffer;
        }

        private void AddBuffer(AttackRayElement attackRay) {
            _attackBuffer.Add(attackRay);
        }

        public void ClearBuffer() {
            _attackBuffer.Clear();
        }

        public float GetAggressiveStrength() {
            return _statusProperties.ValueRO.aggressiveStrength;
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

        public void SetAttackAnimation(int value) {
            _animationProperties.ValueRW.attack = value;
        }
        public float GetCurrentAnimationNormalizedTime() {
            return _animationProperties.ValueRO.currentAnimationNormalizedTime;
        }
        public int GetCurrentAnimationTagHash() {
            return _animationProperties.ValueRO.currentAnimationTagHash;
        }

        public void Attack(int value) {
            int index = _attackRayBlobRef.ValueRO.startIndexBlobRef.Value.intBlob[value];
            int endIndex = _attackRayBlobRef.ValueRO.endIndexBlobRef.Value.intBlob[value];
            for (; index < endIndex; index++) {
                AddBuffer(new AttackRayElement { attackRayBuffer = _attackRayBlobRef.ValueRO.attackBlobRef.Value.attackBlob[index] });
            }
        }
      
    }
}
