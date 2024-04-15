using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct PlayerAspect : IAspect
    {
#pragma warning disable CS0414 // warning ����
        private readonly RefRO<PlayerTag> _playerTag;
#pragma warning restore
        private readonly RefRW<NavAgentProperties> _navAgentPropert;
        private readonly RefRW<AnimationProperties> _animationProperties;
        private readonly RefRW<RotationProperties> _rotationProperties;
        private readonly RefRW<StatusProperties> _statusProperties;

        public bool GetIsStop() {
            return _navAgentPropert.ValueRO.isStop || !_navAgentPropert.ValueRO.isPathFinded;

        }

        public void Walk(bool isWalk) {
            _animationProperties.ValueRW.isMove = isWalk;
        }

        public void SetStop(bool isValue) {
            _navAgentPropert.ValueRW.isStop = isValue;
        }

        public void SetPathFinded(bool isValue) {
            _navAgentPropert.ValueRW.isPathFinded = isValue;
        }

        public bool IsContinueousAttack {
            get { return _animationProperties.ValueRO.isContinueousAttack; }
            set { _animationProperties.ValueRW.isContinueousAttack = value; }
        }

        public void SetTargetPosition(float3 value) {
            _rotationProperties.ValueRW.targetPosition = value;
        }

    }
}
