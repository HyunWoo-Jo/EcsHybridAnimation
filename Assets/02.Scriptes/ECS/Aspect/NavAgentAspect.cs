using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.Aspect
{
    public readonly partial struct NavAgentAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<NavAgentProperties> _navAgentProperties;
        private readonly DynamicBuffer<WaypointBuffer> _waypointBuffer;
        private readonly RefRW<RotationProperties> _rotationProperties;
        private readonly RefRW<MoveProperties> _moveProperties;


        public bool GetIsStop() {
            return _navAgentProperties.ValueRO.isStop || !_navAgentProperties.ValueRO.isPathFinded || _moveProperties.ValueRO.isStop;
        }
        public float3 Position {
            get { return _localTransform.ValueRO.Position; }
            set { _localTransform.ValueRW.Position = value; }
        }
        public float3 GetPosition() {
            return _localTransform.ValueRO.Position;
        }
        public quaternion Rotation {
            get { return _localTransform.ValueRO.Rotation; }
            set { _localTransform.ValueRW.Rotation = value; }
        }
        /// <summary>
        /// 검색 정지
        /// </summary>
        public bool IsStop {
            get { return _navAgentProperties.ValueRO.isStop; }
            set { _navAgentProperties.ValueRW.isStop = value; }
        }
        /// <summary>
        /// 이동 정지
        /// </summary>
        public bool IsMoveStop {
            get { return _moveProperties.ValueRO.isStop; }
            set { _moveProperties.ValueRW.isStop = value; }
        }


        public bool IsFinded {
            get { return _navAgentProperties.ValueRO.isPathFinded; }
            set { _navAgentProperties.ValueRW.isPathFinded = value; }
        }

        public float3 Dirction {
            set { _moveProperties.ValueRW.direction = value; }
        }

        public float3 TurnTargetPosition {
            set { _rotationProperties.ValueRW.targetPosition = value; }
        }

        public Entity GetTargetEntity() {
            return _navAgentProperties.ValueRO.targetEntity;
        }

       
        public float TraceRange {
            get { return _navAgentProperties.ValueRO.traceRange; }
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
            return _waypointBuffer[_navAgentProperties.ValueRO.curretWaypoint].waypoint * new float3(1,0,1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NextWaypoint() {
            if(_navAgentProperties.ValueRO.curretWaypoint + 1 < _waypointBuffer.Length) {
                _navAgentProperties.ValueRW.curretWaypoint++;
                return false;
            } else {
                return true;
            }
        }
            
        public void SetTragetPosition(float3 value) {
            _rotationProperties.ValueRW.targetPosition = value;
        }

        public bool IsTurnStop {
            get { return _rotationProperties.ValueRO.isStop; }
            set { _rotationProperties.ValueRW.isStop = value; }
        }
    }
}
