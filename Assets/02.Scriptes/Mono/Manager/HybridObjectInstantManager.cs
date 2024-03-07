using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using UnityEngine.SceneManagement;

namespace Game.Mono
{
    /// <summary>
    /// animation gpu Instancing으로 변경하여 지금은 사용하지 않음
    /// //
    /// 
    /// Hybrid로 오브젝트를 생성할때 관리하는 매니저
    /// Subscene에 Hybrid로 생성하는 오브젝트는 자동으로 해제 처리하도록 만들어짐
    /// </summary>
    /// 

    public class HybridObjectInstantManager : Singleton<HybridObjectInstantManager>
    {
        private readonly List<GameObject> _instantiatedList = new List<GameObject>();
        private Scene _subScene;

        protected override void Awake() {
            base.Awake();
            _subScene = SceneManager.GetSceneAt(1);
        }
        private void OnApplicationQuit() {
            DeestroyAllHybridObject();
        }

        public GameObject InstantiateObject(GameObject gameObj) {
            var obj = Object.Instantiate(gameObj);
            //SceneManager.MoveGameObjectToScene(obj, _subScene);
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
