using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
namespace Game.Ecs
{
    [DisallowMultipleComponent,  RequireComponent(typeof(Rigidbody))]
    public class RigidbodyConversionMono : MonoBehaviour
    {
        [SerializeField] Rigidbody _rigidbody;

        private class RigidbodyConversionBaker : Baker<Rigidbody> {
            public override void Bake(Rigidbody authoring) {
                Entity entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                PhysicsConstrainedBodyPair pcbp = new PhysicsConstrainedBodyPair(entity, default, false);
                AddComponent(entity, pcbp);
            }
        }
 
    }
}
