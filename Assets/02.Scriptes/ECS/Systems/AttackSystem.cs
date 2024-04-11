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
                    // rayDataElement 형식으로 변환
                    RaycastInput rayInput = new RaycastInput {
                        Filter = new CollisionFilter {
                            BelongsTo = (uint)ray.attackRayBuffer.belongTo,
                            CollidesWith = (uint)ray.attackRayBuffer.withIn
                        },
                        Start = attackAspect.LocalToGlobal(ray.attackRayBuffer.rayStart), // local 좌표 월드로 수정
                        End = attackAspect.TransformDirection(ray.attackRayBuffer.rayEnd) 
                    };

#if UNITY_EDITOR
                    Debug.Log(rayInput.End);
                    Debug.DrawRay(rayInput.Start, rayInput.End, Color.green, 1f);
#endif
                    if (physicsWorld.CastRay(rayInput, ref hitList)) {
                        if (ray.attackRayBuffer.isNewRay) {
                            hitEntitySet.Clear();
                        }
                        // 중복 hit 처리
                        foreach (var hit in hitList) {
                            if (!hitEntitySet.Contains(hit.Entity)) {
                                // 데미지 처리
                                StatusAspect statusAspect = SystemAPI.GetAspect<StatusAspect>(hit.Entity);
                                statusAspect.AddBuffer(ray.attackRayBuffer.attackMagnification);
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
