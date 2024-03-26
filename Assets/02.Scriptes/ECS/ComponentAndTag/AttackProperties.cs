using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct AttackBuffer : IComponentData {
        public DynamicBuffer<AttackRayElement> attackRayBuffer;
    }

    public partial struct AttackRayElement : IBufferElementData{
        public RaycastInput rayInput;
        public float attackPoint;
        public bool isNewRay; // ture 새로운 레이, false 트루 레이가 나올때 까지 같은 Entite에 대한 컨택이 없음 
    }
}
