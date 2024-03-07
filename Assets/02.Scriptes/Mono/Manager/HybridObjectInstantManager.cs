using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using UnityEngine.SceneManagement;

namespace Game.Mono
{
    /// <summary>
    /// animation gpu Instancing���� �����Ͽ� ������ ������� ����
    /// //
    /// 
    /// Hybrid�� ������Ʈ�� �����Ҷ� �����ϴ� �Ŵ���
    /// Subscene�� Hybrid�� �����ϴ� ������Ʈ�� �ڵ����� ���� ó���ϵ��� �������
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
