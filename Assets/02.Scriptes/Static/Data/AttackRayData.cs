using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Physics;
using Unity.Mathematics;
using Game.Utils;
namespace Game.Data
{

    [CreateAssetMenu(fileName = "RayData", menuName = "Scritable Object/RayDataList")]
    public class RayDataList : ScriptableObject {
        public List<RayData> rayDataList;
    }


    [Serializable]
    public struct RayData {
        public LayerName belongTo;
        public LayerName withIn;
        public float3 rayStart;
        public float3 rayEnd;
        public float attackMagnification;
        public bool isNewRay; // ture 새로운 레이, false 트루 레이가 나올때 까지 같은 Entite에 대한 컨택이 없음 
    }
}
