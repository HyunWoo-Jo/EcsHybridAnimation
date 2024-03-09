using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Tools {
    public class ToolsMainMenu : EditorWindow {

        private List<EditorTab> _tabList = new List<EditorTab>();
        private int _tabIndex = 0;

        [MenuItem("Tools/Game Tools",false,0)]
        public static void Init() {
            // 윈도우 창 출력
            var window = GetWindow(typeof(ToolsMainMenu));
            window.titleContent = new GUIContent("Game Tools");
        }

        private void OnEnable() {
            _tabList.Add(new AnimationClipConverterTab(this));
        }

        private void OnGUI() {

            // Toolbars
            GUILayout.Space(5);
            _tabIndex = GUILayout.Toolbar(_tabIndex, 
                new[] { "Animation Clip Converter" 
                }, GUILayout.Width(EditorGUIUtility.labelWidth + 10));

            if(_tabIndex > -1) {
                EditorTab selectedTab = _tabList[_tabIndex];
                selectedTab.DrawUI();
            }
            
        }


    }
}
