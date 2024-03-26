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
        public bool isNewRay; // ture ���ο� ����, false Ʈ�� ���̰� ���ö� ���� ���� Entite�� ���� ������ ���� 
    }
}
