using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Game.Ecs.ComponentAndTag
{
    public partial struct PlayerProperties : IComponentData
    {
        public bool isAttack;
    }
}
