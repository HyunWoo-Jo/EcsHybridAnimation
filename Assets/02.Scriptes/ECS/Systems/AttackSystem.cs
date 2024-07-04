using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Burst;
using Game.Ecs.ComponentAndTag;
using Game.Ecs.Aspect;
using Unity.Mathematics;
using Game.Data;
using Unity.Transforms;
namespace Game.Ecs.System
{
    [BurstCompile]
    public partial struct AttackSystem : ISystem
    {
        private int _attackHash;
        void OnCreate(ref SystemState state) {
            _attackHash = AnimationHash.Attack;
        }
        void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            PhysicsWorld physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            foreach (var attackAspect in SystemAPI.Query<AttackAspect>()) {
                // Animation중에 공격이 진행되도록 처리
                int animationTagHash = attackAspect.GetCurrentAnimationTagHash();
                float normalizedTime = attackAspect.GetCurrentAnimationNormalizedTime();
                if (animationTagHash == _attackHash && normalizedTime > 0.9f) {
                    attackAspect.IsAttackAble = true;
                } else if(animationTagHash == _attackHash && normalizedTime > 0.4f && attackAspect.IsAttackAble) {
                    attackAspect.IsAttackAble = false;
                    attackAspect.Attack(attackAspect.GetAttackAnimationCount());
                }




                // Ray 처리
                NativeList<Unity.Physics.RaycastHit> hitList = new NativeList<Unity.Physics.RaycastHit>(Allocator.Temp);
                NativeHashSet<Entity> hitEntitySet = new NativeHashSet<Entity>(10, Allocator.Temp);
                RefRO<LocalTransform> localTransform = SystemAPI.GetComponentRO<LocalTransform>(attackAspect.entity);
                foreach (var ray in attackAspect.GetRays()) {
                    
                    float3 start = localTransform.ValueRO.TransformPoint(ray.attackRayBuffer.rayStart); // local To global
                    // rayDataElement 형식으로 변환
                    RaycastInput rayInput = new RaycastInput {
                        Filter = new CollisionFilter {
                            BelongsTo = ((uint)ray.attackRayBuffer.belongTo),
                            CollidesWith = ((uint)ray.attackRayBuffer.withIn),
                            GroupIndex = 0
                        },
                        Start = start, // local 좌표 월드로 수정
                        End = start + localTransform.ValueRO.TransformDirection(ray.attackRayBuffer.rayEnd)             
                    };
                    float damage = ray.attackRayBuffer.attackMagnification * attackAspect.GetAggressiveStrength();
#if UNITY_EDITOR
                    Debug.DrawRay(rayInput.Start, rayInput.End - rayInput.Start, Color.green, 1f);
#endif

                    if (physicsWorld.CastRay(rayInput, ref hitList)) {   
                        if (ray.attackRayBuffer.isNewRay) {
                            hitEntitySet.Clear();
                        }
                        // 중복 hit 처리
                        foreach (var hit in hitList) {
#if UNITY_EDITOR
                            Debug.DrawRay(hit.Position, new float3(0.1f, 0f, 0f), Color.yellow, 2f);
                            Debug.DrawRay(hit.Position, new float3(-0.1f, 0f, 0f), Color.yellow, 2f);
                            Debug.DrawRay(hit.Position, new float3(0, 0f, 0.1f), Color.yellow, 2f);
                            Debug.DrawRay(hit.Position, new float3(0, 0f, -0.1f), Color.yellow, 2f);
                            Debug.DrawRay(hit.Position, new float3(0, 0.1f, 0), Color.yellow, 2f);
                            Debug.DrawRay(hit.Position, new float3(0, -0.1f, 0), Color.yellow, 2f);
#endif
                            if (!SystemAPI.HasComponent<StatusProperties>(hit.Entity)) continue;
                            if (!hitEntitySet.Contains(hit.Entity)) {
                                // 데미지 처리                   
                                StatusAspect statusAspect = SystemAPI.GetAspect<StatusAspect>(hit.Entity);
                                statusAspect.AddBuffer(-damage);
                                hitEntitySet.Add(hit.Entity);
                            }
                        }
                        hitList.Clear();
                    }
                }
                hitList.Dispose();
                hitEntitySet.Dispose();
                attackAspect.ClearBuffer();
            }

        }


    }
}
