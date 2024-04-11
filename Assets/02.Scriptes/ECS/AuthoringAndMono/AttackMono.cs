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


                // AttackData 持失採
                // Blob 持失
                List<BlobAssetReference<BlobArray<RayData>>> rayDataBlob = new List<BlobAssetReference<BlobArray<RayData>>>();
                // sub Blob
                foreach(var dataList in authoring._attackRayDataList) {
                    var subBuilder = new BlobBuilder(Allocator.Temp);
                    ref var subRoot = ref subBuilder.ConstructRoot<BlobArray<RayData>>();
                    var subBlobArray = subBuilder.Allocate(ref subRoot, dataList.rayDataList.Count);
                    for(int i =0;i< dataList.rayDataList.Count; i++) {
                        subBlobArray[i] = dataList.rayDataList[i];
                    }
                    rayDataBlob.Add(subBuilder.CreateBlobAssetReference<BlobArray<RayData>>(Allocator.Persistent));
                }
                // main Blob
                var mainBuilder = new BlobBuilder(Allocator.Temp);
                ref var mainRoot = ref mainBuilder.ConstructRoot<BlobArray<BlobAssetReference<BlobArray<RayData>>>>();
                var mainBlobArray = mainBuilder.Allocate(ref mainRoot, authoring._attackRayDataList.Count);
                for(int i = 0; i < mainBlobArray.Length; i++) {
                    mainBlobArray[i] = rayDataBlob[i];
                }
                AddComponent(entity, new AttackRayBlobReference { blobRef = mainBuilder.CreateBlobAssetReference<RayBlob>(Allocator.Persistent) });
            }
        }
    }
}
