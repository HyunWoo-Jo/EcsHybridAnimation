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
        private NativeList<Int2KeyEntity> _tempChangedEntity_list;
        [BurstCompile]
        void OnCreate(ref SystemState state) {
            _objectSplitData_map = new NativeParallelHashMap<Int2MapDataKey, Entity>(1000, Allocator.Persistent);
            _newAreaBuffer_set = new NativeParallelHashSet<Int2MapDataKey>(100, Allocator.Persistent);
            _tempChangedEntity_list = new NativeList<Int2KeyEntity>(1000, Allocator.Persistent);
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state) {
            

        }
        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var ecbEnity = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbEnity.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            // ������Ʈ�� ���ο� ��ġ Ȯ��, ������ ����
            new SplitReadJob {
                ecb = ecb,
                objetSplitData_read = _objectSplitData_map.AsReadOnly(),
                tempEntity_writer = _tempChangedEntity_list.AsParallelWriter(),
                newBuffer_writer = _newAreaBuffer_set.AsParallelWriter(),
            }.ScheduleParallel(state.Dependency);
            state.CompleteDependency();
            // ���� ��ġ Entity ����


        }

        [BurstCompile]
        private partial struct SplitReadJob : IJobEntity {
            public EntityCommandBuffer.ParallelWriter ecb;
            public NativeParallelHashMap<Int2MapDataKey, Entity>.ReadOnly objetSplitData_read;
            public NativeParallelHashSet<Int2MapDataKey>.ParallelWriter newBuffer_writer;
            public NativeList<Int2KeyEntity>.ParallelWriter tempEntity_writer;
            [BurstCompile]
            public void Execute(RefRO<LocalTransform> localTr, RefRW<ObjectSplitProperties> splitProperties, Entity entity, [EntityIndexInQuery] int sortKey) {
                Int2MapDataKey int2MapDataKey = new Int2MapDataKey();            
                int2MapDataKey.ChangeValueFromPosition(localTr.ValueRO.Position);
                // ���ο� ��ġ�� ���
                if (!objetSplitData_read.ContainsKey(int2MapDataKey)) {
                    newBuffer_writer.Add(int2MapDataKey);
                }
                // ��ġ�� ���� �Ǿ��� ���
                if(splitProperties.ValueRO.currentKey != int2MapDataKey) {
                    splitProperties.ValueRW.preKey = splitProperties.ValueRO.currentKey;
                    splitProperties.ValueRW.currentKey = int2MapDataKey;
                    Int2KeyEntity intkeyEntity = new Int2KeyEntity { entity = entity, key = int2MapDataKey };
                    tempEntity_writer.AddNoResize(intkeyEntity);

                }
                
            }
        }

        [BurstCompile]
        private struct DeleteAddKeyJob : IJobParallelFor {
            public NativeList<Int2KeyEntity> int2KeyEntities;

            public void Execute(int index) {

            }

        }

       
    }
}
