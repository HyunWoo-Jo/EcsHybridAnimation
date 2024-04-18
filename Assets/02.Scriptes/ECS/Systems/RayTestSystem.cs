using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Data;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
namespace Game.Ecs.System {
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class RayTestSystem : SystemBase {
        private Camera _mainCamera;
        private PhysicsWorld _physicsWorld;
        protected override void OnCreate() {
            base.OnCreate();
            _mainCamera = Camera.main;
        }

        protected override void OnUpdate() {
            foreach (var pro in SystemAPI.Query<RayTestPro>()) {
                _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
                //MouseRayTest(pro);
                RayTest(pro);
            }
        }

        private void RayTest(RayTestPro pro) {
            var raycastInput = new RaycastInput {
                Start = pro.rayStart,
                End = pro.rayEnd,
                Filter = new CollisionFilter {
                    GroupIndex = 0,
                    BelongsTo = (uint)pro.be,
                    CollidesWith = (uint)pro.co
                }

            };
#if UNITY_EDITOR
            Debug.DrawRay(raycastInput.Start, raycastInput.End, Color.red, 1f);
#endif
            if (_physicsWorld.CastRay(raycastInput, out var raycastHit)) {
                Debug.DrawRay(raycastHit.Position, new float3(0.1f, 0f, 0f), Color.yellow, 2f);
                Debug.DrawRay(raycastHit.Position, new float3(-0.1f, 0f, 0f), Color.yellow, 2f);
                Debug.DrawRay(raycastHit.Position, new float3(0, 0f, 0.1f), Color.yellow, 2f);
                Debug.DrawRay(raycastHit.Position, new float3(0, 0f, -0.1f), Color.yellow, 2f);
                Debug.DrawRay(raycastHit.Position, new float3(0, 0.1f, 0), Color.yellow, 2f);
                Debug.DrawRay(raycastHit.Position, new float3(0, -0.1f, 0), Color.yellow, 2f);
                Debug.Log("Hit : " + raycastHit.Entity.Index);
            }
        }

        private void MouseRayTest(RayTestPro pro) {
           
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(300f);
            Debug.Log((uint)pro.be + " / " + (uint)pro.co);
            var raycastInput = new RaycastInput {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter {
                    GroupIndex = 0,
                    BelongsTo = (uint)pro.be,
                    CollidesWith = (uint)pro.co
                }
            };
#if UNITY_EDITOR
            Debug.DrawRay(rayStart, rayEnd, Color.red, 1f);
#endif
            if (_physicsWorld.CastRay(raycastInput, out var raycastHit)) {
                Debug.Log("Hit : " + raycastHit.Entity.Index);
            }
        }
    }

    public partial struct RayTestPro : IComponentData{
        public LayerName be;
        public LayerName co;
        public float3 rayStart;
        public float3 rayEnd;
    }
}
