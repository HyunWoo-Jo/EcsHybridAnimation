using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace Game.Ecs.ComponentAndTag 
{
    /// <summary>
    /// NavAgent ÄÄÆ÷³ÍÆ®
    /// </summary>
    public struct NavAgentProperties : IComponentData, IEnableableComponent
    {
        public Entity entity;
        public Entity targetEntity;
        public bool isPathFinded;
        public bool isStop;
        public int curretWaypoint;
        public float moveSpeed;
        public float pivotHight;
        public float timer;
    }


    public struct WaypointBuffer : IBufferElementData {
        public float3 waypoint;
    }
}
