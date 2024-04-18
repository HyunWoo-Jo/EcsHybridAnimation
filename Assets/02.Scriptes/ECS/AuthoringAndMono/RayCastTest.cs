using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Game.Data;
using Game.Ecs.System;
namespace Game.Ecs.AuthoringsAndMono
{
    public class RayCastTest : MonoBehaviour
    {
        public LayerName be;
        public LayerName wi;

        public float3 rayStart;
        public float3 rayEnd;

        private class Ba : Baker<RayCastTest> {
            public override void Bake(RayCastTest authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new RayTestPro { be = authoring.be, co = authoring.wi, rayStart = authoring.rayStart, rayEnd = authoring.rayEnd });
            }
        }
    }
}
