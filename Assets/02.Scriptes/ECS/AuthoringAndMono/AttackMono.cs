using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Game.Data;
using Game.Ecs.ComponentAndTag;
namespace Game.Ecs.AuthoringsAndMono
{
    [RequireComponent(typeof(StatusMono))]
    [RequireComponent(typeof(AnimationMono))]
    public class AttackMono : MonoBehaviour
    {
        [SerializeField] private List<RayDataList> _attackRayDataList;
        [SerializeField] private List<RayDataList> _skillRayDataList;

        private class AttackBaker : Baker<AttackMono> {
            public override void Bake(AttackMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddBuffer<AttackRayElement>(entity);

                // init
                var rayDataBlobBuilder = new BlobBuilder(Allocator.Temp);
                var startIndexBlobBuilder = new BlobBuilder(Allocator.Temp);
                var endIndexBlobBuilder = new BlobBuilder(Allocator.Temp);
                ref var rayDataRoot = ref rayDataBlobBuilder.ConstructRoot<BlobArray<RayData>>();
                ref var startIndexRoot = ref startIndexBlobBuilder.ConstructRoot<BlobArray<int>>();
                ref var endIndexRoot = ref endIndexBlobBuilder.ConstructRoot<BlobArray<int>>();
                // get size
                int raySize = 0; // total rayDataSize
                foreach(var dataList in authoring._attackRayDataList) {
                    raySize += dataList.rayDataList.Count;
                }
                int indexSize = authoring._attackRayDataList.Count; // indexSize

                var rayDataBlobArray = rayDataBlobBuilder.Allocate(ref rayDataRoot, raySize);
                var startIndexBlobArray = startIndexBlobBuilder.Allocate(ref startIndexRoot, indexSize);
                var endIndexBlobArray = endIndexBlobBuilder.Allocate(ref endIndexRoot, indexSize);
                int rayCount = 0;
                int indexCount = 0;
                // allocate
                foreach(var dataList in authoring._attackRayDataList) {
                    startIndexBlobArray[indexCount] = rayCount;
                    foreach(var rayData in dataList.rayDataList) {
                        rayDataBlobArray[rayCount] = rayData;
                        rayCount++;
                    }
                    endIndexBlobArray[indexCount] = rayCount;
                    indexCount++;
                }
                // create Blob and Add Component
                AddComponent(entity, new AttackRayBlobReference {
                    attackBlobRef = rayDataBlobBuilder.CreateBlobAssetReference<RayBlob>(Allocator.Persistent),
                    startIndexBlobRef = startIndexBlobBuilder.CreateBlobAssetReference<IntBlob>(Allocator.Persistent),
                    endIndexBlobRef = endIndexBlobBuilder.CreateBlobAssetReference<IntBlob>(Allocator.Persistent)
                });
                // Dipose
                rayDataBlobBuilder.Dispose();
                startIndexBlobBuilder.Dispose();
                endIndexBlobBuilder.Dispose();

            }
        }
    }
}
