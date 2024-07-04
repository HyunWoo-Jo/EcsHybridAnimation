using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Game.Data;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct ObjectSplitProperties : IComponentData
    {
        public Int2MapDataKey currentKey;
        public Int2MapDataKey preKey;
        
    }

    public partial struct ObjectSplitBuffer : IComponentData {
        public DynamicBuffer<ObjectSplitElement> objectSplitElements;
    }

    public partial struct ObjectSplitElement : IBufferElementData {
        public Entity entity;
    }
}
