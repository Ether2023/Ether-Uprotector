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
using JetBrains.Annotations;

public enum SupportVerison
{
    V24_4,
    V28
};
public class EtherConfig : ScriptableObject
{
    public bool Enable = true;
    [Header("Ether_IL2CPP Key")]
    public int key = 114514;
    [Header("MetadataVersion")]
    public SupportVerison Metadata_Version = SupportVerison.V24_4;
    [Header("Ether Obfuscator")]
    public ObfusConfig Obfus = new ObfusConfig();
}
[Serializable]
public class ObfusConfig
{
    public Obfuscations Obfuscations;
    [Header("Skip Config")]
    public ignore Keyfunc = new ignore();
}
[Serializable]
public class ignore
{
    [Header("Unity Runtime Function&Field")]
    public ignore_UnityRuntime UnityRuntime = new ignore_UnityRuntime();
    public ignore_GUIComponent GUIComponent = new ignore_GUIComponent();
    public ignore_AnimationComponent AnimationComponent = new ignore_AnimationComponent();
    public ignore_Reflection Reflection = new ignore_Reflection();
    [Header("Custom Function&Field")]
    public string[] custom_ignore_Method;
    public string[] custom_ignore_Field;
    public string[] custom_obfus_Class;
    [HideInInspector]
    public List<MonoClass> MonoBehavior = new List<MonoClass>();
}
[Serializable]
public class ignore_UnityRuntime
{
    [Header("Auto Generate")]
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
    [Header("Auto Generate")]
    public string[] ignoreMethod;
}
[Serializable]
public class ignore_AnimationComponent
{
    [Header("Auto Generate")]
    public string[] ignoreMethod;
}
[Serializable]
public class ignore_Reflection
{
    [Header("Auto Generate")]
    public string[] ignoreNamespace;
    public string[] ignoreType;
    public string[] ignoreMethod;
}
[Serializable]
public class Obfuscations
{
    public bool ControlFlow = true;
    [Header("Skip ControlFlow FuncNames")]
    public string[] ignore_ControlFlow_Method;
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
        ComponentResolver.ProcessAnimation();
        EtherConfig _Config = new EtherConfig();
        _Config.Obfus.Keyfunc.AnimationComponent.ignoreMethod = ComponentResolver.AnimationResolver.ReferencedAnimationMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.GUIComponent.ignoreMethod = ComponentResolver.GUIComponetResolver.ReferencedGuiMethodHashSet.ToArray();
        _Config.Obfus.Keyfunc.Reflection = new ignore_Reflection
        {
            ignoreMethod = reflectionResolver.RelectionMethodList.ToArray(),
            ignoreType = reflectionResolver.RelectionTypeList.ToArray(),
            ignoreNamespace = reflectionResolver.RelectionNamespaceList.ToArray()
        };
        _Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList;
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
        _Config.Obfus.Keyfunc.MonoBehavior = ComponentResolver.ScriptsResolver.ReferencedMonoScriptTypeList;
        AssetDatabase.CreateAsset(_Config, "Assets/Plugins/Ether/Config.asset");
    }
    [MenuItem("Ether/UnlockASM")]
    static void UnlockASM()
    {
        EditorApplication.UnlockReloadAssemblies();
    }
    /*
public class MainWindow : EditorWindow
{
    /*
    private Editor editor;
    Vector3 scrollPos = Vector2.zero;

    private void Awake()
    {

    }

    void Update()
    {
//窗口弹出时候每帧调用

    }

    [MenuItem("O&Z Manager/Main")]
    static void Init()
    {
var window = EditorWindow.GetWindow<MainWindow>(true, "Main", true);
window.editor = Editor.CreateEditor(AssetDatabase.LoadAssetAtPath<OZ_Config>("Assets/O&ZProtector/Config.asset"));
    }

    void OnGUI()
    {

this.editor.OnInspectorGUI();
    }
    */
    [MenuItem("Ether/PEPacker")]
    public static void Pack()
    {
        string path = EditorUtility.OpenFilePanel("选择您的Assembly-CSharp.dll", UnityEngine.Application.dataPath, "dll");
    }
    [MenuItem("Ether/Test")]
    public static void test()
    {
        AssemblyResolver resolver = new AssemblyResolver();
        foreach(var asm in resolver.BuildAssemblies.Where(x => AssemblyResolver.IsAssetsAssembly(x) && !AssemblyResolver.IsPackageAssembly(x)))
        {
            Debug.Log("Name:" + asm.name + " outputPath:" + asm.outputPath);
        }
    }

}