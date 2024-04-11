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
        public bool isNewRay; // ture ���ο� ����, false Ʈ�� ���̰� ���ö� ���� ���� Entite�� ���� ������ ���� 
    }
}
