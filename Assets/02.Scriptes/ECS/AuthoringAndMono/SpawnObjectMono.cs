using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace Game.Ecs
{
    public class SpawnObjectMono : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public class SpawnObjectBaker : Baker<SpawnObjectMono> {
            public override void Bake(SpawnObjectMono authoring) {
                var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
    
            }
        }

    }
    
   
 
}
