using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct FreezeRotationProperties : IComponentData
    {
        public bool3 freezeRotation;
    }
}
