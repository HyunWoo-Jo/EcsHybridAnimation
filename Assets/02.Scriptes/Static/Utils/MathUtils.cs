using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;


namespace Game.Utils
{
    public static class MathUtils 
    {
        /// InversePoint·Î ±³Ã¼
       public static float3 ChangePositionLocalToWorld(this LocalTransform parentTransform ,float3 localPosition) {
            Matrix4x4 parentMatrix = parentTransform.ToMatrix();
            var childMatrix = new float4 { x = localPosition.x, y = localPosition.y, z = localPosition.z, w = 1 };
            float4 resultMatrix = parentMatrix *  childMatrix;
            float3 result = new float3 { x = resultMatrix.x, y = resultMatrix.y, z = resultMatrix.z };
            return result;  
        }
    }
}
