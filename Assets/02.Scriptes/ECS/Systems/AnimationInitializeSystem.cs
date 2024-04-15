using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Unity.Burst;
using Unity.Entities.Conversion;
using UnityEngine.SceneManagement;
using Unity.Transforms;
using Game.Mono;

namespace Game.Ecs.System
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    public partial struct AnimationInitializeSystem : ISystem
    {
        void OnCreate(ref SystemState state) { 
        }

        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp, PlaybackPolicy.SinglePlayback);

            foreach (var (animRef, entity) in SystemAPI.Query<AnimGameObjectReference>().WithNone<AnimationReference>().WithEntityAccess()) {
                // 积己
                GameObject obj = HybridObjectManager.Instance.InstantiateObject(animRef.prefab);
                ecb.AddComponent(entity, new AnimationReference {
                    animator = obj.GetComponent<Animator>(),
                    transform = obj.GetComponent<Transform>()
                });
                // 昏力
                ecb.RemoveComponent<AnimGameObjectReference>(entity);
                DestroyChild(ref state, ref ecb, animRef.animatorEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        // Child Entity 傈何 昏力
        private void DestroyChild(ref SystemState state, ref EntityCommandBuffer ecb, Entity entity) {
            bool isHas = SystemAPI.HasBuffer<Child>(entity);
            if (isHas) {
                DynamicBuffer<Child> buffers = SystemAPI.GetBuffer<Child>(entity);
                foreach (var child in buffers) {
                    DestroyChild(ref state, ref ecb, child.Value);
                }
            }
            ecb.DestroyEntity(entity);

        }
    }
}
