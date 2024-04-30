using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Game.Ecs.ComponentAndTag
{
    public partial struct EnemySplitProperties : IComponentData, IEnableableComponent
    {
        public float distance;
        public int currentRenewCount;
    }

    public partial struct EnemySplitBuffer : IComponentData
    {
        public DynamicBuffer<EnemySplitElement> enemyBuffer;
    }

    public partial struct EnemySplitElement : IBufferElementData {
        public Entity entity;
    }
}
