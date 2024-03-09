using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Tools
{
    public abstract class EditorTab
    {
        protected ToolsMainMenu _oner; 
        public EditorTab(ToolsMainMenu oner) {
            _oner = oner;
        }


        /// <summary>
        /// ToolsMainMenu에서 Draw 호출
        /// </summary>
        public abstract void DrawUI();


        protected void DrawDivisionLine(Color color, float hight = 1.5f, float width = -1) {
            Rect boxRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
            boxRect.width = width == -1 ? _oner.position.width : width - 10;
            boxRect.height = hight;
            EditorGUI.DrawRect(boxRect, color);

            GUILayout.EndHorizontal();
        }

    }
}
