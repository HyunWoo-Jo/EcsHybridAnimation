using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Game.Ecs.ComponentAndTag
{
    public partial struct AI_AttackRangeProperties : IComponentData
    {
        public float attackRange;
        public float attackDelayTime; // ���� ���� ���� �ð�
        public float currentTime;
        public Entity targetEntity;
    }
}
