using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;
using Unity.AI.Navigation;

namespace Game.Ecs.AuthoringsAndMono
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class NavMeshMono : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;
        private class NavMeshBaker : Baker<NavMeshMono> {
            public override void Bake(NavMeshMono authoring) {
                NavMesh.AddNavMeshData(authoring._navMeshSurface.navMeshData);
            }
        }
    }
}
