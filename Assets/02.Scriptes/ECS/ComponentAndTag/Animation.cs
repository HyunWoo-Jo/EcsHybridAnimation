using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


namespace Game.Ecs.ComponentAndTag
{
    public class AnimationReference : ICleanupComponentData
    {
        public Animator animator;
    }
}
