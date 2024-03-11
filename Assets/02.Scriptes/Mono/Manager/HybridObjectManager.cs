using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;

namespace Game.Mono
{
    /// <summary>
    /// Hybrid�� ������Ʈ�� �����Ҷ� �����ϴ� �Ŵ���
    /// Subscene�� Hybrid�� �����ϴ� ������Ʈ�� �ڵ����� ���� ó���ϵ��� �������
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
