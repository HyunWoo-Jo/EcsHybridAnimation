using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Unity.Transforms;
namespace Game.Ecs.Aspect
{
    public readonly partial struct AnimationAspect : IAspect
    {
        private readonly RefRO<LocalTransform> _ownerTransform;
        private readonly AnimationReference _animRef;
    }
}
