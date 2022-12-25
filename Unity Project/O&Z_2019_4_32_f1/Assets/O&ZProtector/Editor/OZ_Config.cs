using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public enum SupportVerison
{
    V24_4,
    V28
};
public class OZ_Config : ScriptableObject
{
    [Header("启用混淆")]
    public bool Enable = true;
    [Header("O&Z_IL2CPP加密密钥")]
    [Tooltip("使用它对您的Metadata文件进行加密")]
    public int key = 114514;
    [Header("Unity对应的metadata版本")]
    [Tooltip("目前支持24.4和28")]
    public SupportVerison Version = SupportVerison.V24_4;
    [Header("O&Z MonoObfuscator配置")]
    public ObfusConfig Obfus;
    [Header("Obfuscate Function配置")]
    public ignore Keyfunc;
}
[Serializable]
public class ObfusConfig
{
    [Header("控制流程加密")]
    [Tooltip("使用本方法将对您的程序函数方法进行流程混淆，不改变执行流程，但是可以打乱编译流程以及IL码的顺序，可以做到干扰破解")]
    public bool ControlFlow = true;
    [Header("Skip ControlFlow FuncNames")]
    [Tooltip("不对以下函数进行ControlFlow混淆")]
    public string[] ignore_ControlFlow_Method;
    [Header("整数预设混淆")]
    [Tooltip("使用本方法将加密您程序集中的所有int类型预设常量（即明文数据，例如int num = 8, 8即为预设常量）")]
    public bool NumObfus = true;
    [Header("局部变量混淆为字段")]
    [Tooltip("使用本方法对您的程序函数的局部变量进行混淆加密，进一步降低代码可读性")]
    public bool LocalVariables2Field = true;
    [Header("字符串加密")]
    [Tooltip("使用本方法加密您游戏内的字符串常量，每一个字符串都单独对应一个单独和密码和单独的解密函数，使破解难度上升")]
    public bool StrCrypter = true;
    [Header("类&方法&字段混淆)")]
    [Tooltip("使用本方法加密您项目中所有的函数，类，甚至是参数，使程序的不可读性达到最高（我们采用了Unity函数名堆积作为字典，使得这种方法混淆的函数难以被反混淆器识别为Obfuscated或JunkFunc）")]
    public bool Obfusfunc = true;
    [Header("Anti De4dot")]
    [Tooltip("通过添加特殊属性使得de4dot无法正确识别被混淆的名称，从而使其无法还原正确反混淆名称")]
    public bool AntiDe4dot = true;
    [Header("Anti ILdecomplier")]
    [Tooltip("通过MS提供的SuppressIldasmAttribute使反编译器无法正常工作")]
    public bool FuckILdasm = true;
    [Header("PEPacker(Only For MonoBuild)")]
    [Tooltip("修改NET程序的特征码使得反编译器无法正确识别")]
    public bool PEPacker = true;
}
[Serializable]
public class ignore
{
    [Header("Unity Runtime Function&Field")]
    [Tooltip("Unity的生命周期方法和回调方法不能混淆,忽略列表包含了大多数常用的生命周期和回调方法，如果有遗漏，可以自行添加")]
    public string[] ignoreMethod;
    public string[] ignoreField;
    [Header("需要忽略的方法")]
    [Tooltip("此项采用白名单的模式，如果您不需要混淆某个方法或者某个方法不能被混淆(例如使用了反射)，可以将方法名称添加到这里")]
    public string[] custom_ignore_Method;
    [Header("需要忽略的字段")]
    [Tooltip("此项采用白名单的模式，如果您不需要混淆某个字段或者某个字段不能被混淆(例如使用了反射)，可以将字段名称添加到这里")]
    public string[] custom_ignore_Field;
    [Header("需要混淆的类")]
    [Tooltip("此项采用黑名单的模式,因为涉及到Unity预制体的影响,类名一般来说不能够轻易混淆(*详细原因见下方*),如果你需要混淆某个类名，可以将类名添加到这里")]
    public string[] custom_obfus_Class;
}

public class OZ_Config_Manager : UnityEditor.Editor
{
    /*
    [MenuItem("O&Z Manager/CreateConfig")]
    static void CreateConfig()
    {
        OZ_Config _Config = ScriptableObject.CreateInstance<OZ_Config>();
        if (File.Exists(Application.dataPath + "/Assets/O&ZProtector/Config.asset"))
            Debug.LogError("Config Exists!");
        else
            AssetDatabase.CreateAsset(_Config, "Assets/O&ZProtector/Config.asset");
    }
    [MenuItem("O&Z Manager/UnlockASM")]
    static void UnlockASM()
    {
        EditorApplication.UnlockReloadAssemblies();
    }
}
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
    [MenuItem("O-Z Manager/PEPacker")]
    public static void Pack()
    {
        string path = EditorUtility.OpenFilePanel("选择您的Assembly-CSharp.dll", Application.dataPath,"dll");
    }
}