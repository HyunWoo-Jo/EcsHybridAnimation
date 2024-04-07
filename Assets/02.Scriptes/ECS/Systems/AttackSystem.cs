using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Burst;
using Game.Ecs.ComponentAndTag;
using Game.Ecs.Aspect;
namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct AttackSystem : ISystem
    {
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {
            PhysicsWorld physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
           
            foreach (var attackAspect in SystemAPI.Query<AttackAspect>()) {
                NativeList<Unity.Physics.RaycastHit> hitList = new();
                NativeHashSet<Entity> hitEntitySet = new();
                foreach (var ray in attackAspect.GetRays()) {
#if UNITY_EDITOR
                    Debug.DrawRay(ray.rayInput.Start, ray.rayInput.End, Color.blue, 1f);
#endif
                    if (physicsWorld.CastRay(ray.rayInput, ref hitList)) {
                        if (ray.isNewRay) {
                            hitEntitySet.Clear();
                        }
                        // 吝汗 hit 贸府
                        foreach (var hit in hitList) {
                            if (!hitEntitySet.Contains(hit.Entity)) {
                                // 单固瘤 贸府
                                StatusAspect statusAspect = SystemAPI.GetAspect<StatusAspect>(hit.Entity);
                                statusAspect.AddBuffer(ray.attackPoint);
                                hitEntitySet.Add(hit.Entity);
                            }
                        }
                        hitList.Clear();
                    }
                }
                hitList.Dispose();
                hitEntitySet.Dispose();
            }
            

        }


    }
}
