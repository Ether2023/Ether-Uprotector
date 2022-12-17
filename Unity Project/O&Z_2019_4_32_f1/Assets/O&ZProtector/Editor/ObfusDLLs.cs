using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.Il2Cpp;
using OZ_Obfuscator.Ofbuscators;
using OZ_Obfus;
using OZ_Obfus.obfuscators;
using System.IO;
using OZ_Obfus;
using O_Z_IL2CPP_Security.LitJson;

public class ObfusDLLs : IIl2CppProcessor
{
    public int callbackOrder { get { return 0; } }
    public void OnBeforeConvertRun(BuildReport report, Il2CppBuildPipelineData data)
    {
        OZ_Config config = AssetDatabase.LoadAssetAtPath<OZ_Config>("Assets/O&ZProtector/Config.asset");
        AssemblyLoader loader = new AssemblyLoader(data.inputDirectory+ "\\Assembly-CSharp.dll");
        OZ_Obfus.OZ_Obfuscator obfuscator = new OZ_Obfus.OZ_Obfuscator(loader.Module,config.Obfus, config.Keyfunc);
        if (config.Obfus.ControlFlow)
        {
            obfuscator.controlFlow.Execute();
        }
        if (config.Obfus.Obfusfunc)
        {
            obfuscator.obfusFunc.Excute();
        }
        if (config.Obfus.NumObfus)
        {
            obfuscator.numObfus.Execute();
        }
        if (config.Obfus.LocalVariables2Field)
        {
            obfuscator.localVariables2Field.Execute();
        }
        if (config.Obfus.StrCrypter)
        {
            obfuscator.strCrypter.Execute();
        }
        loader.Save();
        File.Delete(data.inputDirectory + "\\Assembly-CSharp.dll");
        File.Move(data.inputDirectory + "\\Assembly-CSharp.dllProtected", data.inputDirectory + "\\Assembly-CSharp.dll");
        Debug.Log("Protected Assembly-CSharp.dll Obfuscated!");
        Debug.Log("Obfuscation Done!");
    }
}
public class ObfusEditorWindow : EditorWindow
{
    /*
    [MenuItem("O&ZProtector/Obfuscate")]
    public static void ShowWindow()
    {
        GetWindow<ObfusEditorWindow>("Obfuscate");
    }
    void OnGUI()
    {
        if (GUILayout.Button("Obfuscate"))
        {
            Debug.Log(File.ReadAllText(Application.dataPath + "/O&ZProtector/Editor/keyfunc.json"));
        }
    }
    */
}