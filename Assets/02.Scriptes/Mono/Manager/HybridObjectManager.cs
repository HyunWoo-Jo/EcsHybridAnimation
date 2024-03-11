using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;

namespace Game.Mono
{
    /// <summary>
    /// Hybrid로 오브젝트를 생성할때 관리하는 매니저
    /// Subscene에 Hybrid로 생성하는 오브젝트는 자동으로 해제 처리하도록 만들어짐
    /// </summary>
    /// 

    public class HybridObjectManager : Singleton<HybridObjectManager>
    {
        private readonly List<GameObject> _instantiatedList = new List<GameObject>();

        protected override void Awake() {
            base.Awake();
        }
        private void OnApplicationQuit() {
            DeestroyAllHybridObject();
        }

        public GameObject InstantiateObject(GameObject gameObj) {
            var obj = Object.Instantiate(gameObj);
            _instantiatedList.Add(obj);
            return obj;
        }

        private void DeestroyAllHybridObject() {
            foreach (var item in _instantiatedList) {
                DestroyImmediate(item);
            }
            _instantiatedList.Clear();
        }
    }
}
