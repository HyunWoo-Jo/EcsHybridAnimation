using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAdder : MonoBehaviour
{
    [SerializeField] private List<NavMeshData> _navMeshDataList = new();
    private List<NavMeshDataInstance> _navMeshDataInsList = new();

    void Awake()
    {
        foreach(var navData in _navMeshDataList) {
            var navIns = NavMesh.AddNavMeshData(navData);
            _navMeshDataInsList.Add(navIns);
        }
    }
    private void OnDisable() {
        foreach(var navIns in _navMeshDataInsList) {
            navIns.Remove();
        }
    }
}
