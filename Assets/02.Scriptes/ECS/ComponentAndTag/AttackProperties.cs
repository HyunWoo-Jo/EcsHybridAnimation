using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Game.Utils;
using Game.Data;
using Unity.Collections;
namespace Game.Ecs.ComponentAndTag
{

    public partial struct AttackRayElement : IBufferElementData {
        public RayData attackRayBuffer;
    }

    public partial struct AttackRayBlobReference : IComponentData{
        public BlobAssetReference<RayBlob> blobRef;
    }
    public struct RayBlob {
        public BlobArray<BlobAssetReference<BlobArray<RayData>>> attackBlobTwoArray;
    }
}
