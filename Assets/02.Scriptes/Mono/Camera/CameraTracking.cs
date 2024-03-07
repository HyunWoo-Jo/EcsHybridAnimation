using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.System;
using Unity.Mathematics;
namespace Game.Mono
{
    public class CameraTracking : MonoBehaviour
    {
        private float followSpeed = 3f;
        [SerializeField] private float3 _addPosition;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            while (true) {
                var cameraSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CameraTrackingSystem>();
                if (cameraSystem != null) {
                    cameraSystem.SetTargetPositionListener(UpdatePosition);
                    break;
                }
                yield return null;
            }
        }

        public void UpdatePosition(float3 pos) {
           
            transform.position = math.lerp(transform.position, pos + _addPosition, Time.deltaTime * followSpeed);
        }
    }
}
