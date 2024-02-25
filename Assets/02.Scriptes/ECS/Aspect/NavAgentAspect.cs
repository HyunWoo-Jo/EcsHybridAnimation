using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace Game.Ecs
{
    public readonly partial struct NavAgentAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<NavAgentProperties> _navAgentProperties;
        private readonly DynamicBuffer<WaypointBuffer> _waypointBuffer;

        public float3 Position {
            get { return _localTransform.ValueRO.Position; }
            set { _localTransform.ValueRW.Position = value; }
        }
        public quaternion Rotation {
            get { return _localTransform.ValueRO.Rotation; }
            set { _localTransform.ValueRW.Rotation = value; }
        }
        public float Timer {
            get { return _navAgentProperties.ValueRO.timer; }
            set { _navAgentProperties.ValueRW.timer = value; }
        }
        public bool IsStop() {
            return _navAgentProperties.ValueRO.isStop;
        }
        public bool IsFinded() {
            return _navAgentProperties.ValueRO.isPathFinded;
        }

        public Entity GetTargetEntity() {
            return _navAgentProperties.ValueRO.targetEntity;
        }

        public float GetTimer() {
            return _navAgentProperties.ValueRO.timer;
        }
        
        public float GetMoveSpeed() {
            return _navAgentProperties.ValueRO.moveSpeed;
        }
        
        /// <summary>
        /// 검색 완료 후 Nav Properties 리셋
        /// </summary>
        public void FindedNavAgent() {
            _navAgentProperties.ValueRW.curretWaypoint = 0;
            _navAgentProperties.ValueRW.isPathFinded = true;
        }

        public void ClearWaypointBuffer() {
            _waypointBuffer.Clear();
        }
        public void AddWaypointBuffer(float3 position) {
            _waypointBuffer.Add(new WaypointBuffer { waypoint = position });
        }
        public float3 GetCurrentWaypointPosition() {
            return _waypointBuffer[_navAgentProperties.ValueRO.curretWaypoint].waypoint + new float3(0, _navAgentProperties.ValueRO.distanceFromCenter2Floor, 0);
        }
        public void NextWaypoint() {
            if(_navAgentProperties.ValueRO.curretWaypoint + 1 < _waypointBuffer.Length) {
                _navAgentProperties.ValueRW.curretWaypoint++;
            }
        }
    }
}
