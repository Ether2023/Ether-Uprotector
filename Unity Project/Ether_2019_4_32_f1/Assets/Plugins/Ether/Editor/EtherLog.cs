using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class EtherLog : ScriptableObject
{
    public int tmp = 0;
    public SwapMap[] Map;
}
[Serializable]
public class SwapMap
{
    public string OriginName;
    public string ObfusName;
}
[CustomEditor(typeof(EtherLog))]
public class EtherLogInspector : Editor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        var maps = this.serializedObject.FindProperty("Map");
        GUILayout.Label("Num:" + maps.arraySize.ToString());
        for (int i = 0; i < maps.arraySize; i++)
        {
            var map = maps.GetArrayElementAtIndex(i);
            var OriName = map.FindPropertyRelative("OriginName");
            var ObfusName = map.FindPropertyRelative("ObfusName");
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("item[" + i.ToString() + "]");
            EditorGUILayout.PropertyField(OriName);
            EditorGUILayout.PropertyField(ObfusName);
            EditorGUILayout.EndVertical();
        }
    }
}
