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
}
