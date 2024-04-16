using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct MoveProperties : IComponentData
    {
        public bool isStop;
        public float accelerateSpeed;
        public float maxSpeed;
        public float3 direction;
    }
}
