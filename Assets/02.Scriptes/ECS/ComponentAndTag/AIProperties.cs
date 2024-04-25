using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Game.Ecs.ComponentAndTag
{
    public partial struct NewAITag : IComponentData { }
    public partial struct AI_AttackRangeProperties : IComponentData, IEnableableComponent
    {
        public float attackRange;
        public float attackDelayTime; // ���� ���� ���� �ð�
        public float currentTime;
        public Entity targetEntity;
    }
    public partial struct AI_TraceRangeProperties : IComponentData, IEnableableComponent {
        public float traceRange;
    }
}
