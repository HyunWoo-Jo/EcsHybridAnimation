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
    public partial struct NavAgentProperties : IComponentData
    {
        public Entity targetEntity;
        public bool isPathFinded;
        public bool isStop;
        public int curretWaypoint;
        public float traceRange;
    }

    public partial struct WaypointBuffer : IBufferElementData {
        public float3 waypoint;
    }
}
