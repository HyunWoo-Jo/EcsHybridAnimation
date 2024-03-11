using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Unity.Burst;
using Unity.Entities.Conversion;
using UnityEngine.SceneManagement;
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
                // 생성
                GameObject obj = HybridObjectManager.Instance.InstantiateObject(animRef.prefab);          
                ecb.AddComponent(entity, new AnimationReference {
                    animator = obj.GetComponent<Animator>(),
                    transform = obj.GetComponent<Transform>()
                });
                // 삭제
                ecb.RemoveComponent<AnimGameObjectReference>(entity);
                ecb.DestroyEntity(animRef.animatorEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
