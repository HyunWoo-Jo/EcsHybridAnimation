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

    public class AnimationReference : ICleanupComponentData {
        public Animator animator;
        public Transform transform;
    }

    public partial struct AnimationProperties : IComponentData {
        public int attack;
        public int preAttack;
        public bool isContinueousAttack;

        public int triggerHash;
        public bool isTrigger;
        public bool isMove;
        public bool preMove;
    }
}
