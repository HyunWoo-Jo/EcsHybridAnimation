using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct RotationProperties : IComponentData
    {
        public bool isStop;
        public float3 targetPosition;
        public float rotationSpeed;
    }
}
