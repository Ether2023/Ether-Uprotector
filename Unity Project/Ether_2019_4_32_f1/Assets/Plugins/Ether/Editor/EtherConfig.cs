using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;    
using System.Linq;
using System.Runtime.InteropServices;
using MenuItem = UnityEditor.MenuItem;
using System.Windows.Forms;
using Ether_Obfuscator.Obfuscators.Unity;
using Ether.Il2cpp;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class EtherConfig : ScriptableObject
{
    public bool Enable_Obfuscator = true;
    public bool Enable_Il2CPP = false;
    public ObfusConfig Obfus = new ObfusConfig();
    public Il2cppConfig il2cpp = new Il2cppConfig();
}
[Serializable]
public class Il2cppConfig
{
    public string UnityVersion = UnityEngine.Application.unityVersion;
    public string Key = "114514";
    public bool EnableCheckSum = true;
    public bool StringEncrypt = false;
    public bool Il2cppAPIObfuscate = false;
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
    //[HideInInspector]
    //public MonoType[] MonoBehavior;
    [HideInInspector]
    public string[] ProjectSripts;
    public bool ObfusType = true;
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
    public bool ControlFlow = false;
    public bool NumObfus = true;
    public bool LocalVariables2Field = true;
    public bool StrCrypter = true;
    public bool Obfusfunc = true;
    public bool AntiDe4dot = true;
    public bool FuckILdasm = true;
    public bool PEPacker = true;
    public bool MethodError = true;
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
        //_Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList.ToArray();
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
    [MenuItem("Ether/Test")]
    static void Test()
    {
        UnityEngine.Debug.Log(System.Windows.Forms.Application.ExecutablePath);
        Process[] processList = Process.GetProcesses();
        foreach (var process in processList)
        {
            if (process.ProcessName == "Unity.exe")
            {
                //Debug.Log(process.StartInfo.)
            }
        }
    }
    [MenuItem("Ether/uninstall Ether IL2CPP")]
    static void uninstallEtherIl2CPP()
    {
        try
        {
            Il2cppInstaller.UnInstall(System.Windows.Forms.Application.ExecutablePath);
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
}