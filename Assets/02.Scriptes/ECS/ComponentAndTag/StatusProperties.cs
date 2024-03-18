using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct StatusProperties : IComponentData
    {
        public float hp;
        public float aggressiveStrength;
        public float defensivePower;
    }
}
