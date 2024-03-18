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
        private readonly RefRW<StatusProperties> _statusProperties;

        public bool IsStop() {
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

        public void Attack() {
            _animationProperties.ValueRW.attack = 0;
        }

    }
}
