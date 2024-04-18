using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct StatusProperties : IComponentData
    {
        public float maxHp;
        public float currentHp;
        public float aggressiveStrength;
        public float defensivePower;
    }

    public partial struct HpCalculatorElement : IBufferElementData {
        public float value;
    }
}
