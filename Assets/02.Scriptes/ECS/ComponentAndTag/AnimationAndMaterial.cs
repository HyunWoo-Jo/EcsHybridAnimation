using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


namespace Game.Ecs.ComponentAndTag
{
    public class AnimGameObjectReference : IComponentData {
        public GameObject prefab;
        public Entity animatorEntity;
    }

    public class AnimationReference : ICleanupComponentData
    {
        public Animator animator;
        public Transform transform;
    }
    public class MaterialReference : IComponentData {
        public Material material;
    }

    public partial struct AnimationProperties : IComponentData {
        public int currentAnimationTagHash;
        public float currentAnimationNormalizedTime;

        public int attack;
        public int preAttack;// Ʈ���Ÿ� �ѹ����� �����ϱ� ���� ���
        public bool isContinueousAttack;

        public int triggerHash;
        public bool isTrigger;
        public bool isMove;
        public bool preMove; // Ʈ���Ÿ� �ѹ����� �����ϱ� ���� ���
    }
    
    public partial struct  FloatMaterialElement : IBufferElementData
    {
        public int nameId;
        public float value;
    }
}
