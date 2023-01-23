using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;    
using Ether_Obfuscator.Obfuscators.UnityMonoBehavior;
using System.Linq;
using System.Runtime.InteropServices;
using MenuItem = UnityEditor.MenuItem;
using System.Windows.Forms;

public class EtherConfig : ScriptableObject
{
    public bool Enable = true;
    public int key = 114514;
    public ObfusConfig Obfus = new ObfusConfig();
}
[Serializable]
public class ObfusConfig
{
    //public AssemblyConfig assembly;
    public Obfuscations Obfuscations;
    public ignore Keyfunc = new ignore();
}
/*
[Serializable]
public class AssemblyConfig
{
    public string Name;
    public bool NeedObfuscator;
}
*/
[Serializable]
public class ignore
{
    [Header("Skip ControlFlow Function")]
    public string[] ignore_ControlFlow_Method;
    [Header("Unity Runtime Function&Field")]
    public ignore_UnityRuntime UnityRuntime = new ignore_UnityRuntime();
    public ignore_GUIComponent GUIComponent = new ignore_GUIComponent();
    public ignore_AnimationComponent AnimationComponent = new ignore_AnimationComponent();
    public ignore_Reflection Reflection = new ignore_Reflection();
    [Header("Custom Function&Field")]
    public string[] custom_ignore_Method;
    public string[] custom_ignore_Field;
    public string[] custom_ignore_Class;
    [HideInInspector]
    public MonoClass[] MonoBehavior;
    [HideInInspector]
    public string[] ProjectSripts;
    public bool ObfusType = false;
    public bool OnlyForProjectScripts = true;
}
[Serializable]
public class ignore_UnityRuntime
{
    public string[] ignoreMethod = {"Awake","OnEnable","Start","FixedUpdate","Update","OnDisable","LateUpdate","Reset","OnValidate","FixedUpdate","OnTriggerEnter","OnTriggerEnter2D",
    "OnTriggerExit","OnTriggerExit2D","OnTriggerStay2D","OnCollisionEnter","OnCollisionEnter2D","OnCollisionExit","OnCollisionExit2D","OnCollisionStay","OnCollisionStay2D","OnMouseDown",
    "OnMouseDrag","OnMouseEnter","OnMouseExit","OnMouseOver","OnMouseUp","OnMouseUpAsButton","OnPreCull","OnBecameVisible","OnBecameInvisible","OnWillRenderObject","OnPreRender",
    "OnRenderObject","OnPostRender","OnRenderImage","OnGUI","OnDrawGizmos","OnDrawGizmosSelectedOnApplicationFocus","OnApplicationPause","OnApplicationQuit","OnDisable",
    "OnDestory","OnLevelWasLoaded","OnAnimatorIK","OnAnimatorMove","OnApplicationFocus","OnApplicationPause","OnApplicationQuit","OnAudioFilterRead","OnBecameInvisible",
    "OnBecameVisible","OnConnectedToServer","OnControllerColliderHit","OnEnable","OnFailedToConnect","OnDisconnectedFromServer","OnDrawGizmos","OnDrawGizmosSelected","OnEnable",
    "OnFailedToConnect","OnFailedToConnectToMasterServer","OnJointBreak","OnJointBreak2D","OnMasterServerEvent","OnNetworkInstantiate","OnParticleCollision","OnParticleSystemStopped","OnParticleTrigger",
    "OnParticleUpdateJobScheduled","OnPlayerConnected","OnPlayerDisconnected","OnPostRender","OnPreCull","OnPreRender","OnRenderImage","OnRenderObject","OnSerializeNetworkView",
    "OnServerInitialized","OnTransformChildrenChanged","OnTransformParentChanged","OnValidate","OnWillRenderObject","Reset"};
    public string[] ignoreField;
}
[Serializable]
public class ignore_GUIComponent
{
    public string[] ignoreMethod;
}
[Serializable]
public class ignore_AnimationComponent
{
    public string[] ignoreMethod;
}
[Serializable]
public class ignore_Reflection
{
    public string[] ignoreNamespace;
    public string[] ignoreType;
    public string[] ignoreMethod;
}
[Serializable]
public class Obfuscations
{
    public bool ControlFlow = true;
    public bool NumObfus = true;
    public bool LocalVariables2Field = true;
    public bool StrCrypter = true;
    public bool Obfusfunc = true;
    public bool AntiDe4dot = true;
    public bool FuckILdasm = true;
    public bool PEPacker = true;
    public bool MethodError = true;
}
[Serializable]
public class MonoType
{
    public string Assembly;
    public string Namespace;
    public string Name;
}

public class EtherConfigManager : UnityEditor.Editor
{
    [MenuItem("Ether/Generate Config")]
    static void CreateConfig()
    {
        ReflectionResolver reflectionResolver = new ReflectionResolver();
        ComponentResolver.ProcessComponentsInAllScenes();
        ComponentResolver.ProcessComponentsInAllPrefabs();
        ComponentResolver.ProcessAllAssetFiles();
        EtherConfig _Config = CreateInstance<EtherConfig>();
        _Config.Obfus.Keyfunc.AnimationComponent.ignoreMethod = ComponentResolver.AnimationResolver.ReferencedAnimationMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.GUIComponent.ignoreMethod = ComponentResolver.GUIComponetResolver.ReferencedGuiMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.Reflection = new ignore_Reflection
        {
            ignoreMethod = reflectionResolver.RelectionMethodList.ToArray(),
            ignoreType = reflectionResolver.RelectionTypeList.ToArray(),
            ignoreNamespace = reflectionResolver.RelectionNamespaceList.ToArray()
        };
        List<MonoType> types = new List<MonoType>();
        _Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList.ToArray();
        _Config.Obfus.Keyfunc.ProjectSripts = ComponentResolver.ScriptsResolver.ProjectScripts.ToArray();
        if (File.Exists(UnityEngine.Application.dataPath + "/Plugins/Ether/Config.asset"))
        {
            DialogResult dr = MessageBox.Show("Config already exists. Do you want to overwrite it?", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(dr == DialogResult.Yes)
                AssetDatabase.CreateAsset(_Config, "Assets/Plugins/Ether/Config.asset");
            else
                Debug.LogError("Config Exists!");

        }
        else
            AssetDatabase.CreateAsset(_Config, "Assets/Plugins/Ether/Config.asset");
    }
    [MenuItem("Ether/Unlock Assemblies")]
    static void UnlockAssemblies()
    {
        EditorApplication.UnlockReloadAssemblies();
    }
    /*
    [MenuItem("Ether/Analyze and Update Config")]
    static void UpdateConfig()
    {
        DialogResult dr = MessageBox.Show("Are you sure you want to reanalyze your project and generate the corresponding configuration file?\nThis may cause you to lose the modification of Unity Runtime Function&Field section (Custom Function&Field section will not be affected)", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (dr == DialogResult.No) return;
        if (!File.Exists(UnityEngine.Application.dataPath + "/Plugins/Ether/Config.asset"))
        {    
            MessageBox.Show("You have no Config or Config has been lost! Regernate Config for you...","Error" ,MessageBoxButtons.OK ,MessageBoxIcon.Error);
            CreateConfig();
            return;
        }
        ReflectionResolver reflectionResolver = new ReflectionResolver();
        ComponentResolver.ProcessComponentsInAllScenes();
        ComponentResolver.ProcessComponentsInAllPrefabs();
        ComponentResolver.ProcessAnimation();
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        _Config.Obfus.Keyfunc.AnimationComponent.ignoreMethod = ComponentResolver.AnimationResolver.ReferencedAnimationMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.GUIComponent.ignoreMethod = ComponentResolver.GUIComponetResolver.ReferencedGuiMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.Reflection = new ignore_Reflection
        {
            ignoreMethod = reflectionResolver.RelectionMethodList.ToArray(),
            ignoreType = reflectionResolver.RelectionTypeList.ToArray(),
            ignoreNamespace = reflectionResolver.RelectionNamespaceList.ToArray()
        };
        _Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList.ToArray();
        AssetDatabase.SaveAssets();
    }
    */
}