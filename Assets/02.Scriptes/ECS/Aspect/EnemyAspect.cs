using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct EnemyAspect : IAspect
    {
        private readonly RefRO<EnemyTag> _tag;
     

    }
}
