using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class AnimationHash 
    {
       
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Skill_0 = Animator.StringToHash("Skill_0");
        public static readonly int Skill_1 = Animator.StringToHash("Skill_1");
        public static readonly int Skill_2 = Animator.StringToHash("Skill_2");
        public static readonly int Skill_3 = Animator.StringToHash("Skill_3");
        public static readonly int Die = Animator.StringToHash("Die");
    }
}
