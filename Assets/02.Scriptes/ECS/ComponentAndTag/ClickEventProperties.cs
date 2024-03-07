using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Utils;

namespace Game.Ecs.ComponentAndTag
{
    public partial struct ClickEventProperties : IComponentData    {
        public Entity clickMovePointEntity;
    }
}
