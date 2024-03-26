using System.Collections;
using System.Collections.Generic;
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
        private PhysicsWorld _physicsWorld;
            
        void OnCreate(ref SystemState state) {

        }
        void OnDestroy(ref SystemState state) {

        }
        void OnUpdate(ref SystemState state) {
            _physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;

            new AttackJob {
                physicsWorld = _physicsWorld
            }.ScheduleParallel();
            
        }

        private partial struct AttackJob : IJobEntity {

            public PhysicsWorld physicsWorld;
            public void Execute(AttackAspect attackAspect) {
                NativeList<RaycastHit> hitList = new();
                NativeHashSet<Entity> hitEntitySet = new();
                foreach(var ray in attackAspect.GetRays()) {
                    if(physicsWorld.CastRay(ray.rayInput, ref hitList)) {
                        if(ray.isNewRay) {
                            hitEntitySet.Clear();
                        }
                        foreach (var hit in hitList) {
                            if (!hitEntitySet.Contains(hit.Entity)) {
                                // entity hit buffer에 어택 추가 구현

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
