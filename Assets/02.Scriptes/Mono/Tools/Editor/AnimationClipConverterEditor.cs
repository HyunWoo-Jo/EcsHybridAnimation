using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Tools {
    public class AnimationClipConverterEditor : EditorWindow {

        [MenuItem("Tools/Animation Clip Converter",false,0)]
        public static void Init() {
            // 윈도우 창 출력
            var window = GetWindow(typeof(AnimationClipConverterEditor));
            window.titleContent = new GUIContent("Animation Clip Converter");
        }

       


        private string _saveName;
        private void DrawGeneralSettings() {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("저장 이름"));
            _saveName = EditorGUILayout.TextField(_saveName, GUILayout.Width(50));
            GUILayout.EndHorizontal();
        }


    }
}
