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
        private readonly RefRO<PlayerTag> _playerTag;
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

        public int GetAttackCount() {
            return _animationProperties.ValueRO.attack;
        }
        public void Attack(int value) {
            _animationProperties.ValueRW.attack = value;
            switch (value) {
                case 0:

                break;
                case 1:
                    break;
                case 2:
                break;
                case 3:
                break;
            }
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
