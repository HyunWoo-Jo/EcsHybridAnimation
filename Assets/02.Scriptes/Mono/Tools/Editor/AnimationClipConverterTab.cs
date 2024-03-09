using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace Game.Tools
{
    public class AnimationClipConverterTab : EditorTab {
        public AnimationClipConverterTab(ToolsMainMenu oner) : base(oner) {
        }

        private string _saveName;


        private GameObject _asset;
        private ModelImporter _modelImpoter;
        private ModelImporterClipAnimation[] _clips;

        private Texture2D _animationTexture;

        public override void DrawUI() {
            DrawButtonUI();
            DrawAnimationUI();
            DrawSettingsUI();
        }
        private void DrawButtonUI() {
            /// Button UI
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open File", GUILayout.Width(100), GUILayout.Height(50))) {
                OpenAnimationClipFile();
            }
            if (_asset != null) {
                if (GUILayout.Button("Convert", GUILayout.Width(100), GUILayout.Height(50))) {
                    Convert();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawAnimationUI() {
            /// Animtions UI
            if (_asset != null) {
                GUILayout.BeginVertical();
                GUILayout.Space(15);
                GUILayout.Label("Animations", EditorStyles.boldLabel);
                DrawDivisionLine(Color.gray, 1f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Fbx", GUILayout.Width(50));
                GUI.enabled = false;
                EditorGUILayout.ObjectField(_asset, typeof(GameObject), false, GUILayout.Width(220));
                GUI.enabled = true;
                GUILayout.EndHorizontal();


                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.alignment = TextAnchor.MiddleLeft;
                boxStyle.padding = new RectOffset(5, 0, 0, 0);
                int count = 0;
                foreach (var clip in _clips) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("  clip " + count.ToString(), GUILayout.Width(60));
                    GUILayout.Box(clip.name + ".anim", boxStyle, GUILayout.Width(220));
                    GUILayout.EndHorizontal();
                    count++;
                }
                GUILayout.EndVertical();
            }
        }


        private void DrawSettingsUI() {
            //// Settings
            GUILayout.Space(15);
            GUILayout.BeginVertical(GUILayout.Width(400));

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            DrawDivisionLine(Color.gray, 1f);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("저장 이름"), GUILayout.Width(60));
            _saveName = EditorGUILayout.TextField(_saveName, GUILayout.Width(70));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }


        private void OpenAnimationClipFile() {
            string path = EditorUtility.OpenFilePanel("AnimationClipList", "Assets/", "fbx");
            if (!string.IsNullOrEmpty(path)) {     
                path = path.Remove(0, Application.dataPath.Length - "Assets".Length);

                _asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                _modelImpoter = AssetImporter.GetAtPath(path) as ModelImporter;
                _clips = _modelImpoter.clipAnimations;

            }
        }

        private void Convert() {
            
            foreach(var clip in _clips) {

            }
        }

    }
}
