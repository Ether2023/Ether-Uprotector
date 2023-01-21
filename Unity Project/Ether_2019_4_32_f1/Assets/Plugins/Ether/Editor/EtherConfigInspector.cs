using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EtherConfig))]
public class EtherConfigInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Box("Ether Uprotector");
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
    }
}
