using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.Il2Cpp;
using UnityEditor.Callbacks;
using OZ_Obfuscator.Ofbuscators;
using OZ_Obfus;
using OZ_Obfus.obfuscators;
using System.IO;
using O_Z_IL2CPP_Security.LitJson;
using System;
using System.Text.RegularExpressions;

public class ObfusDLLs : IIl2CppProcessor , IPostBuildPlayerScriptDLLs, IPreprocessBuildWithReport , IPostprocessBuildWithReport
{
    private static _OZ_Obfuscator _OZ = new _OZ_Obfuscator();
    public int callbackOrder { get { return 0; } }
    public void OnBeforeConvertRun(BuildReport report, Il2CppBuildPipelineData data)
    {
        /*
        if (!File.Exists(Application.dataPath + "/Assets/O&ZProtector/Config.asset"))
            Debug.LogError("Config Exists!");
        else
        {
            OZ_Config _Config = ScriptableObject.CreateInstance<OZ_Config>();
            AssetDatabase.CreateAsset(_Config, "Assets/O&ZProtector/Config.asset");
            Debug.LogError("Not Found Confgi,New Config Generated!Please reconfigure the Config ");
            return;
        }
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
        */
    }
    public void OnPreprocessBuild(BuildReport report)
    {
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        _OZ = new _OZ_Obfuscator();
    }
    public void OnPostBuildPlayerScriptDLLs(BuildReport report)
    {
        //Debug.Log("Platform:"+ report.summary.platform);
        //Debug.Log("Out:" + report.summary.outputPath);
        try
        {
            if (_OZ.IsSuccess)
            {
                return;
            }
        }
        finally
        {
            _Update();
        }
    }
    public static void UpdateProj()
    {
        _Update();
        EditorApplication.update -= UpdateProj;
    }
    private static void _Update()
    {
        _OZ = new _OZ_Obfuscator();
    }
    [PostProcessScene(1)]
    public static void obfuscate()
    {
        _OZ.obfus();
    }
    public void OnPostprocessBuild (BuildReport report)
    {
        if(report.summary.platform == BuildTarget.StandaloneWindows || report.summary.platform == BuildTarget.StandaloneWindows64)
        {
            Debug.Log(report.summary.outputPath);
            string path = Path.GetDirectoryName(report.summary.outputPath)+"\\";
            path = path + Path.GetFileNameWithoutExtension(report.summary.outputPath) + "_Data\\Managed\\Assembly-CSharp.dll";
            
            _OZ.Pack(path);
            //Debug.Log(path);
        }
    }
}
public class _OZ_Obfuscator
{
    OZ_Config config;
    private bool Obfuscated;
    private string asmpath;
    public bool Error;
    public bool IsSuccess = false;
    public void obfus()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode && !Obfuscated && BuildPipeline.isBuildingPlayer && !Error)
        {
            try
            {
                EditorApplication.LockReloadAssemblies();
                Obfusdll();
            }
            catch (Exception e)
            {
                Debug.LogError("Obfuscation Failed: " + e);
                Error = true;
                throw new OperationCanceledException("Obfuscation failed", e);
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }
        }
    }
    public void Obfusdll()
    {
        Debug.Log("[O&Z Obfuscation]:Obfuscating " + System.Environment.CurrentDirectory.Replace("\\","/") + "/Library/PlayerScriptAssemblies/Assembly-CSharp.dll");
        config = AssetDatabase.LoadAssetAtPath<OZ_Config>("Assets/O&ZProtector/Config.asset");
        asmpath = ConvertToPlayerAssemblyLocationIfPresent(GetCompiledAssemblyLocation("Assembly-CSharp.dll"));
        if(asmpath == null)
        {
            if(File.Exists(System.Environment.CurrentDirectory.Replace("\\", "/") + "/Library/PlayerScriptAssemblies/Assembly-CSharp.dll"))
            {
                asmpath = System.Environment.CurrentDirectory.Replace("\\", "/") + "/Library/PlayerScriptAssemblies/Assembly-CSharp.dll";
            }
            else 
            {
                Error = true;
                Obfuscated = false;
                Debug.LogError("Assembly-CSharp.dll was not found!");
                return; 
            }
            
        }
        AssemblyLoader loader = new AssemblyLoader(File.ReadAllBytes(asmpath));
        OZ_Obfus.OZ_Obfuscator obfuscator = new OZ_Obfus.OZ_Obfuscator(loader.Module, config.Obfus, config.Keyfunc);
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
        if (config.Obfus.AntiDe4dot)
        {
            obfuscator.antide4dot.Execute();
        }
        if (config.Obfus.FuckILdasm)
        {
            obfuscator.fuckILdasm.Execute();
        }
        //File.Delete(asmpath);
        loader.Save(asmpath);
        IsSuccess = true;
        Obfuscated = true;
        Debug.Log("[O&Z Obfuscator]: Obfuscated Successfully!");
    }
    public void Pack(string path)
    {
        config = AssetDatabase.LoadAssetAtPath<OZ_Config>("Assets/O&ZProtector/Config.asset");
        if (config.Obfus.PEPacker && PlayerSettings.GetScriptingBackend(GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.Mono2x)
        {
            Debug.Log("[PEPacker]:Packing " + path);
            PEPacker.pack(path);
        }
        else
        {
            Debug.LogWarning("[PEPacker]:PEPacker功能仅适用于Mono编译方式，且自动构建仅对Windows有效");
            Debug.LogWarning("[PEPacker]:如果您构建的是Android平台，请使用下拉栏中的PEPacker/Pack功能对Assembly-CSharp.dll自行封装");
        }
    }
    private static string ConvertToPlayerAssemblyLocationIfPresent(string location)
    {
        if (location == null)
        {
            return null;
        }
        string playerScriptAssemblyLocation = Regex.Replace(location, "/ScriptAssemblies/", "/PlayerScriptAssemblies/");
        return File.Exists(playerScriptAssemblyLocation) ? playerScriptAssemblyLocation : location;
    }
    private static string GetCompiledAssemblyLocation(string suffix)
    {
        foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            //Debug.Log(assembly.Location);
            if (assembly.Location.EndsWith(suffix))
            {
                return assembly.Location.Replace('\\', '/');
            }
        }
        return null;
    }
    static BuildTargetGroup GetBuildTargetGroupByBuildTarget(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.Android:
                return BuildTargetGroup.Android;

            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;

            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return BuildTargetGroup.Standalone;
            default:
                return BuildTargetGroup.Standalone;
        }
    }
}