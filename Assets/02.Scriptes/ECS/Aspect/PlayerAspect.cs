using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct PlayerAspect : IAspect
    {
        private readonly RefRO<PlayerTag> _playerTag;
        private readonly RefRW<NavAgentProperties> _navAgentPropert;


        public void IsMoveStop(bool isStop) {
            _navAgentPropert.ValueRW.isStop = isStop;
        }
    }
}
