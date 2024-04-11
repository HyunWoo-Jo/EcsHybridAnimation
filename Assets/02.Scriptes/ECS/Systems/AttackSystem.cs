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
                    // rayDataElement �������� ��ȯ
                    RaycastInput rayInput = new RaycastInput {
                        Filter = new CollisionFilter {
                            BelongsTo = (uint)ray.attackRayBuffer.belongTo,
                            CollidesWith = (uint)ray.attackRayBuffer.withIn
                        },
                        Start = attackAspect.LocalToGlobal(ray.attackRayBuffer.rayStart), // local ��ǥ ����� ����
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
                        // �ߺ� hit ó��
                        foreach (var hit in hitList) {
                            if (!hitEntitySet.Contains(hit.Entity)) {
                                // ������ ó��
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
