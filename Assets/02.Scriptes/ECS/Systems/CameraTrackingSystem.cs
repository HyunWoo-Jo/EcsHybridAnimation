using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using System;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{
    public partial class CameraTrackingSystem : SystemBase {
        // Mono -> CameraTracking.cs
        private Action<float3> _targetPositionUpdate_listener;

        public void SetTargetPositionListener(Action<float3> targetPosition) {
            if (_targetPositionUpdate_listener != null) _targetPositionUpdate_listener = null;
            _targetPositionUpdate_listener = targetPosition;
        }
        protected override void OnCreate() {
            base.OnCreate();
        }

        protected override void OnUpdate() {
            if(_targetPositionUpdate_listener == null) return;
           foreach(var aspect in SystemAPI.Query<CameraTargetAspect>()) {
                _targetPositionUpdate_listener?.Invoke(aspect.GetPosition());
            }
        }

        
    }
}
