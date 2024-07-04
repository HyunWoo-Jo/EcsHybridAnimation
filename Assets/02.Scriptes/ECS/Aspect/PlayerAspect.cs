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
#pragma warning disable CS0414 // warning Á¦°Å
        private readonly RefRO<PlayerTag> _playerTag;
#pragma warning restore
        private readonly RefRW<NavAgentProperties> _navAgentProperties;
        private readonly RefRW<AnimationProperties> _animationProperties;
        private readonly RefRW<RotationProperties> _rotationProperties;
        private readonly RefRW<StatusProperties> _statusProperties;
        private readonly RefRW<MoveProperties> _moveProperties;

     
        public bool IsContinueousAttack {
            get { return _animationProperties.ValueRO.isContinueousAttack; }
            set { _animationProperties.ValueRW.isContinueousAttack = value; }
        }



    }
}
