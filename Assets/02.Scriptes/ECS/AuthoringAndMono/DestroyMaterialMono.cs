using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Ecs.ComponentAndTag;
using Game.Data;
namespace Game.Ecs.AuthoringsAndMono
{
    public class DestroyMaterialMono : MonoBehaviour
    {
        private class DestroyBaker : Baker<DestroyMaterialMono>
        {
            public override void Bake(DestroyMaterialMono authoring)
            {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                var buffer = AddBuffer<FloatMaterialElement>(entity);
                buffer.Add(new FloatMaterialElement
                {
                    nameId = ShaderPropertyId.DestroyAlpha,
                    value = 0
                });
            }
        }
    }
}
