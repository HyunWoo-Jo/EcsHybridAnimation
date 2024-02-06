using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using UnityEngine.Experimental.AI;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
namespace Game.Ecs {
    /// <summary>
    /// Nav Agent 컨트롤 시스템
    /// </summary>
    [BurstCompile]
    public partial struct NavAgentSystem : ISystem {
        private int count;
        [BurstCompile]
        private partial struct NavAgentMoveJob : IJobEntity {
            public float deltaTime;
 
            [BurstCompile]
            private void Execute(NavAgentAspect aspect) {
                if (aspect.IsStop()) return;


            }
            [BurstCompile]
            private void Move(NavAgentAspect aspect) {
                // 회전

                // 이동

            }
            
        }
        
        [BurstCompile]
        private void OnCreate(ref SystemState state) {
            count = 0;
        }
        [BurstCompile]
        private void OnDestroy(ref SystemState state) {

        }
        [BurstCompile]
        private void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;

            //new NavAgentMoveJob {
            //    deltaTime = deltaTime
            //}.ScheduleParallel();
            count++;
            FindPath(ref state);

        }

 //       [BurstCompile]
        private void FindPath(ref SystemState state) {
            if (count > 1) return;
            var navQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.TempJob, 1000);

            foreach (var navAspect in SystemAPI.Query<NavAgentAspect>()) {
                float3 fromPos = navAspect.GetPosition();
                float3 toPos = SystemAPI.GetComponent<LocalTransform>(navAspect.GetTargetEntity()).Position;
                Debug.Log(toPos);
            }

            navQuery.Dispose();

        }
    }
}

