using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using Game.Data;
using Unity.Transforms;
using System.Linq;
using Game.Ecs.ComponentAndTag;
using PlasticGui.WorkspaceWindow.PendingChanges;
using UnityEngine.Assertions.Must;
using Unity.Jobs;
namespace Game.Ecs.System
{
    public partial struct ObjectSplitSystem : ISystem
    {
        private NativeParallelHashMap<Int2MapDataKey, Entity> _objectSplitData_map;
        private NativeParallelHashSet<Int2MapDataKey> _newAreaBuffer_set;
        private NativeList<Int2KeyEntity> _changedEntity_list;
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            _objectSplitData_map = new NativeParallelHashMap<Int2MapDataKey, Entity>(1000, Allocator.Persistent);
            _newAreaBuffer_set = new NativeParallelHashSet<Int2MapDataKey>(100, Allocator.Persistent);
            _changedEntity_list = new NativeList<Int2KeyEntity>(1000, Allocator.Persistent);
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {
            

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecbEnity = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbEnity.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            // 오브젝트의 새로운 위치 확인, 데이터 변경
            //new SplitReadJob {
            //    objetSplitData_read = _objectSplitData_map.AsReadOnly(),
            //    changedEntity_writer = _changedEntity_list.AsParallelWriter(),
            //    newBuffer_writer = _newAreaBuffer_set.AsParallelWriter(),
            //}.ScheduleParallel(state.Dependency);
            state.CompleteDependency();
            // 기존 위치 Entity 삭제


        }

        [BurstCompile]
        private partial struct SplitReadJob : IJobEntity {
        
            public NativeParallelHashMap<Int2MapDataKey, Entity>.ReadOnly objetSplitData_read;
            public NativeList<Int2KeyEntity>.ParallelWriter changedEntity_writer;
            public NativeParallelHashSet<Int2MapDataKey>.ParallelWriter newBuffer_writer;
            
            [BurstCompile]
            public void Execute(RefRO<LocalTransform> localTr, RefRW<ObjectSplitProperties> splitProperties, Entity entity, [EntityIndexInQuery] int sortKey) {
                Int2MapDataKey int2MapDataKey = new Int2MapDataKey();            
                int2MapDataKey.ChangeValueFromPosition(localTr.ValueRO.Position);
                // 새로운 위치의 경우
                if (!objetSplitData_read.ContainsKey(int2MapDataKey)) {
                    newBuffer_writer.Add(int2MapDataKey); // 새로 생성 버퍼에 추가
                }
                // 위치가 변경 되었을 경우
                if(splitProperties.ValueRO.currentKey != int2MapDataKey) {
                    splitProperties.ValueRW.preKey = splitProperties.ValueRO.currentKey;
                    splitProperties.ValueRW.currentKey = int2MapDataKey;
                    Int2KeyEntity intkeyEntity = new Int2KeyEntity { entity = entity, key = int2MapDataKey };
                    changedEntity_writer.AddNoResize(intkeyEntity); // 변경 버퍼에 추가

                }
                
            }
        }

        [BurstCompile]
        private struct DeleteAddKeyJob : IJobParallelFor {
            public EntityCommandBuffer.ParallelWriter ecb;
            public NativeList<Int2KeyEntity> changedEntity_list;
            public NativeParallelHashMap<Int2MapDataKey, Entity>.ReadOnly objetSplitData_read;

            public void Execute(int index) {
                Int2MapDataKey key = changedEntity_list[index].key;
                //delete pre
            }

        }

       
    }
}
