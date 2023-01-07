using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.Il2Cpp;
using UnityEditor.Callbacks;
using OZ_Obfuscator.Obfuscators;
using System.IO;
using O_Z_IL2CPP_Security.LitJson;
using System;
using System.Text.RegularExpressions;
using OZ_Obfuscator;
using System.Linq;

public class ObfusDLLs : IPostBuildPlayerScriptDLLs, IPreprocessBuildWithReport , IPostprocessBuildWithReport
{
    private static _OZ_Obfuscator _OZ = new _OZ_Obfuscator();
    private static OZ_Config config;
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        config = AssetDatabase.LoadAssetAtPath<OZ_Config>("Assets/O&ZProtector/Config.asset");
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
        if (!BuildPipeline.isBuildingPlayer) return;
        if (config.Enable)
        {
            _OZ.obfus();
        }
    }
    public void OnPostprocessBuild (BuildReport report)
    {
        if(report.summary.platform == BuildTarget.StandaloneWindows || report.summary.platform == BuildTarget.StandaloneWindows64)
        {
            if (config.Enable)
            {
                //Debug.Log(report.summary.outputPath);
                string path = Path.GetDirectoryName(report.summary.outputPath)+"\\";
                path = path + Path.GetFileNameWithoutExtension(report.summary.outputPath) + "_Data\\Managed\\Assembly-CSharp.dll";
            
                _OZ.Pack(path);
            }
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
        List<Obfuscator> obfuscators = new List<Obfuscator>();
        if (config.Obfus.ControlFlow)
        {
            obfuscators.Add(new ControlFlow(loader.Module, config.Obfus.ignore_ControlFlow_Method));
        }
        if (config.Obfus.Obfusfunc)
        {
            obfuscators.Add(new ObfusFunc(loader.Module));
        }
        if (config.Obfus.NumObfus)
        {
            obfuscators.Add(new NumObfus(loader.Module));
        }
        if (config.Obfus.LocalVariables2Field)
        {
            obfuscators.Add(new LocalVariables2Field(loader.Module));
        }
        if (config.Obfus.StrCrypter)
        {
            obfuscators.Add(new StrCrypter(loader.Module));
        }
        if (config.Obfus.AntiDe4dot)
        {
            obfuscators.Add(new Antide4dot(loader.Module));
        }
        if (config.Obfus.FuckILdasm)
        {
            obfuscators.Add(new FuckILdasm(loader.Module));
        }
        foreach (var obfuscator in obfuscators)
        {
            string outstr = obfuscator.ToString();
            int i = outstr.IndexOf("Obfuscators.");
            outstr = outstr.Substring(i + 12, outstr.Length - i - 12);
            Debug.Log(outstr + " Executing...");
            obfuscator.Execute();
        }
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
        foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x=>!x.IsDynamic))
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