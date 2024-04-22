using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace Game.Ecs.ComponentAndTag 
{
    public struct CameraTargetTag : IComponentData { };
    public struct MapTag : IComponentData { };
    public struct PlayerTag : IComponentData { };
    public struct WalkAnimationTag : IComponentData { };
}
