using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils {
    public class Singleton<T> : MonoBehaviour {
        protected static T _instance;
        public static T Instance {
            get { return _instance; }

        }

        protected virtual void Awake() {
            if (_instance is null) {
                _instance = gameObject.GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
