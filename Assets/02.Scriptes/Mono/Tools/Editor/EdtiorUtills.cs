using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Tools
{
    public static class EditorUtills
    {
        public static void DrawDivisionLine(this Editor editor, float width = 1000, float hight = 1.5f) {
            Rect boxRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
            boxRect.width = width;
            boxRect.height = hight;
            EditorGUI.DrawRect(boxRect, Color.gray);
            GUILayout.EndHorizontal();
        }
       
    }
}
