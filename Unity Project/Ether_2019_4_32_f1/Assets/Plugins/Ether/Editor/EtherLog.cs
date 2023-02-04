using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
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
[Serializable]
public class Message
{
    
}
public enum MessgaeType
{
    
}

[CustomEditor(typeof(EtherLog))]
public class EtherLogInspector : Editor
{
    private AnimBool fadeGroup;
    private void OnEnable()
    {
        this.fadeGroup = new AnimBool(true);
        this.fadeGroup.valueChanged.AddListener(this.Repaint);
    }
    private void OnDisable()
    {
        this.fadeGroup.valueChanged.RemoveListener(this.Repaint);
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        var maps = this.serializedObject.FindProperty("Map");
        GUILayout.Label("Num:" + maps.arraySize.ToString());
        this.fadeGroup.target = EditorGUILayout.Foldout(this.fadeGroup.target, "MonoBehaviourMap", true);
        if (EditorGUILayout.BeginFadeGroup(this.fadeGroup.faded))
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
        EditorGUILayout.EndFadeGroup();
    }
}
