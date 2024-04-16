using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Ecs.ComponentAndTag;
using Unity.Entities;
namespace Game.Ecs.AuthoringsAndMono
{
    public class MoveMono : MonoBehaviour
    {
        [SerializeField] private float _accelerateSpeed;
        [SerializeField] private float _maxSpeed;
        private class MoveBaker : Baker<MoveMono> {
            public override void Bake(MoveMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(entity, new MoveProperties { maxSpeed = authoring._maxSpeed, accelerateSpeed = authoring._accelerateSpeed });
            }
        }
    }
}
