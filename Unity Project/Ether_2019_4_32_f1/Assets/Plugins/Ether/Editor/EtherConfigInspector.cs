using Ether_Obfuscator.Obfuscators;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EtherConfig))]
public class EtherConfigInspector : Editor
{
    Texture ico;
    Vector2 scrollPos;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ico = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Plugins/Ether/ico/logo.png");
        GUI.skin.label.fontSize = 35;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Box(ico,GUILayout.Width(50),GUILayout.Height(50));
        GUILayout.Label("Ether Uprotector");
        EditorGUILayout.EndHorizontal();

        GUI.skin.label.fontSize = 25;
        #region Main Switch
        GUILayout.Label("Main Switch");
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Enable_Obfuscator"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Enable_Il2CPP"));
        this.serializedObject.FindProperty("Enable_Il2CPP").boolValue = false;
        EditorGUILayout.HelpBox("IL2CPP still has some bugs, so it will not take effect", MessageType.Warning);
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows && EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64 && EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorGUILayout.HelpBox("Ether IL2CPP only supports Windows and Android platforms", MessageType.Warning);
            if (this.serializedObject.FindProperty("Enable_Il2CPP").boolValue)
            {
                this.serializedObject.FindProperty("Enable_Il2CPP").boolValue = false;
            }
        }
        #endregion
        if (this.serializedObject.FindProperty("Enable_Il2CPP").boolValue)
        {
            #region IL2CPP
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Ether IL2CPP");
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("il2cpp.UnityVersion"));
            this.serializedObject.FindProperty("il2cpp.UnityVersion").stringValue = Application.unityVersion;
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("il2cpp.Key"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("il2cpp.EnableCheckSum"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("il2cpp.StringEncrypt"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("il2cpp.Il2cppAPIObfuscate"));
            EditorGUILayout.EndVertical();
            #endregion
        }
        EditorGUILayout.Separator();
        if (this.serializedObject.FindProperty("Enable_Obfuscator").boolValue)
        {
            #region Obfuscator
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Obfuscator Settings");
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.label.fontSize = 15;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    GUILayout.Label("Current active platform: Windows"); break;
                case BuildTarget.Android:
                    GUILayout.Label("Current active platform: Android"); break;
                case BuildTarget.iOS:
                    GUILayout.Label("Current active platform: iOS"); break;
                default:
                    GUILayout.Label("Current active platform: Other"); break;
            }
            switch (PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)))
            {
                case ScriptingImplementation.IL2CPP:
                    GUILayout.Label("Current ScriptingBackend: IL2CPP"); break;
                case ScriptingImplementation.Mono2x:
                    GUILayout.Label("Current ScriptingBackend: Mono"); break;
                default: break;
            }

            #region Obfuscations
            GUI.skin.label.fontSize = 23;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Obfuscations");
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.ControlFlow"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.NumObfus"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.LocalVariables2Field"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.StrCrypter"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.Obfusfunc"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.AntiDe4dot"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.FuckILdasm"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.MethodError"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Width(100));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Obfuscations.PEPacker"));
            if (GUILayout.Button("Pack Assembly"))
            {
                string path = EditorUtility.OpenFilePanel("选择您的Assembly-CSharp.dll", UnityEngine.Application.dataPath, "dll");
                if (File.Exists(path) && Path.GetExtension(path) == ".dll")
                    PEPacker.pack(path);
            }
            EditorGUILayout.EndHorizontal();
            if (PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.IL2CPP && serializedObject.FindProperty("Obfus.Obfuscations.PEPacker").boolValue)
            {
                EditorGUILayout.HelpBox("PEPacker does not support IL2CPP ScriptingBackend, so it will not take effect during the construction process", MessageType.Warning);
            }
            if ((EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows && EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64) && serializedObject.FindProperty("Obfus.Obfuscations.PEPacker").boolValue)
            {
                EditorGUILayout.HelpBox("PEPacker cannot automatically handle other target platforms except Windows, please handle them manually", MessageType.Warning);
            }
            if (PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.IL2CPP
                && serializedObject.FindProperty("Obfus.Obfuscations.MethodError").boolValue)
            {
                EditorGUILayout.HelpBox("When ScriptingBackend is IL2CPP, MethodError may cause errors! So it will not take effect during the construction process", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();
            #endregion

            if (this.serializedObject.FindProperty("Obfus.Obfuscations.ControlFlow").boolValue)
            {
                GUILayout.Space(15);
                GUI.skin.label.fontSize = 23;
                GUILayout.Label("ControlFlow Ignore Setting");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.ignore_ControlFlow_Method"));
                EditorGUILayout.HelpBox("You can add Method name there which you don't obfuscate it with ControlFlow", MessageType.Info);
            }
            if (this.serializedObject.FindProperty("Obfus.Obfuscations.Obfusfunc").boolValue)
            {
                GUILayout.Space(15);

                #region Rename Ignore Setting
                GUI.skin.label.fontSize = 23;
                GUILayout.Label("Rename Ignore Setting");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Space(5);
                GUI.skin.label.fontSize = 18;

                GUILayout.Label("Type");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.ObfusType"));
                EditorGUILayout.HelpBox("If this switch is turned on, the obfuscator will rename your class name (This is a test function)", MessageType.Info);
                EditorGUILayout.EndHorizontal();
                if (this.serializedObject.FindProperty("Obfus.Keyfunc.ObfusType").boolValue)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.OnlyForProjectScripts"));
                    if (this.serializedObject.FindProperty("Obfus.Keyfunc.OnlyForProjectScripts").boolValue)
                    {
                        EditorGUILayout.HelpBox("If this switch is turned on, the obfuscator will only obfuscate the script classes in your project (This is a test function)", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("We will obfuscate all types. This will confuse all your classes and may cause your project to not work properly", MessageType.Warning);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                #region IgnoreUnityRuntime
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Unity Runtime");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.UnityRuntime.ignoreMethod"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.UnityRuntime.ignoreField"));
                EditorGUILayout.HelpBox("Unity Runtime contains most of the key functions and callbacks of Unity. If there is any omission, please add it to Custom Ignore", MessageType.Info);
                EditorGUILayout.EndVertical();

                #endregion

                GUILayout.Space(20);

                #region GUIIgnore
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("GUI Component & EventSystem");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.GUIComponent.ignoreMethod"));
                EditorGUILayout.HelpBox("GUI Component&EventSystem contains the method of binding all GUI components (this item is automatically generated. If there are errors, you can modify them yourself)", MessageType.Info);
                if (GUILayout.Button("Analyze All GuiComponent and Generate"))
                {
                    AnalyzeAll();
                    EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
                    _Config.Obfus.Keyfunc.GUIComponent.ignoreMethod = ComponentResolver.GUIComponetResolver.ReferencedGuiMethodHashSet.ToArray();
                    //_Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList.ToArray();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndVertical();
                #endregion

                GUILayout.Space(20);

                #region AnimationIgnore
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Animation Component");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.AnimationComponent.ignoreMethod"));
                EditorGUILayout.HelpBox("Animation Component contains the method of binding all Animation components (this item is automatically generated. If there are errors, you can modify them yourself)", MessageType.Info);
                if (GUILayout.Button("Analyze All AnimationComponent and Generate"))
                {
                    AnalyzeAll();
                    EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
                    _Config.Obfus.Keyfunc.AnimationComponent.ignoreMethod = ComponentResolver.AnimationResolver.ReferencedAnimationMethodHashSet.ToArray();
                    //_Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList.ToArray();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndVertical();
                #endregion

                GUILayout.Space(20);

                #region ReflectionIgnore
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Reflections");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.Reflection.ignoreNamespace"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.Reflection.ignoreType"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.Reflection.ignoreMethod"));
                EditorGUILayout.HelpBox("Reflections contains all classes, methods and fields that may use reflection (this item is automatically generated. If there are errors, you can modify them yourself)", MessageType.Info);
                if (GUILayout.Button("Analyze All Reflections and Generate"))
                {
                    ReflectionResolver reflectionResolver = new ReflectionResolver();
                    EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset"); _Config.Obfus.Keyfunc.Reflection = new ignore_Reflection
                    {
                        ignoreMethod = reflectionResolver.RelectionMethodList.ToArray(),
                        ignoreType = reflectionResolver.RelectionTypeList.ToArray(),
                        ignoreNamespace = reflectionResolver.RelectionNamespaceList.ToArray()
                    };
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndVertical();
                #endregion

                GUILayout.Space(20);

                #region CustomIgnore
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Customs");
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.custom_ignore_Class"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Obfus.Keyfunc.custom_ignore_Field"));
                EditorGUILayout.HelpBox("For some classes and fields, if you don't want to confuse them, you can add them there", MessageType.Info);
                EditorGUILayout.EndVertical();
                #endregion
                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorGUILayout.EndVertical();
            #endregion
        }
        //base.DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
    public void AnalyzeAll()
    {
        ComponentResolver.ProcessComponentsInAllScenes();
        ComponentResolver.ProcessComponentsInAllPrefabs();
        ComponentResolver.ProcessAllAssetFiles();
    }
}
