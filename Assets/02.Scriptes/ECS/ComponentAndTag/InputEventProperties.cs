using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Utils;
using Game.Data;
namespace Game.Ecs.ComponentAndTag
{
    public partial struct InputEventProperties : IComponentData    {
        public Entity clickMovePointEntity;
    }
}
