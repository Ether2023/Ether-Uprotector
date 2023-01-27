using Codice.Client.BaseCommands;
using Ether.Il2cpp;
using Ether_Obfuscator;
using Ether_Obfuscator.Obfuscators;
using Ether_Obfuscator.Obfuscators.Unity;
using Ether_UnityAsset.AssetFile;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Content;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
using UnityEditor.UnityLinker;
using UnityEngine;

public class EtherPipelineBuildProcessor : IPreprocessBuildWithReport, IFilterBuildAssemblies, IPostBuildPlayerScriptDLLs, IUnityLinkerProcessor, IPostprocessBuildWithReport, IPreprocessShaders
{
    static bool hasObfuscated = false;
    static bool hasPostAsset = false;
    Dictionary<TypeKey, TypeKey> monoSwapMaps = new Dictionary<TypeKey, TypeKey>();
    static BuildReport buildReport;
    public int callbackOrder
    {
        get { return 0; }
    }
    public void OnPreprocessBuild(BuildReport _Report)
    {
        buildReport = _Report;
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        hasObfuscated = false;
        hasPostAsset = false;
        if (_Config.Enable_Il2CPP && PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.IL2CPP)
        {
            EtherIl2cppConfig config = new EtherIl2cppConfig();
            config.UnityVersion = _Config.il2cpp.UnityVersion;
            config.EncryptKey = _Config.il2cpp.Key;
            config.EnableStringEncrypt = _Config.il2cpp.StringEncrypt;
            config.EnableCheckSum = _Config.il2cpp.EnableCheckSum;
            config.EnableIl2cppAPIObfuscate = _Config.il2cpp.Il2cppAPIObfuscate;
            try
            {
                Il2cppInstaller.Install(System.Windows.Forms.Application.ExecutablePath, config);
                Log("Ether Il2CPP install Successfully!",LogType.Info);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        if(_Config.Enable_Obfuscator)
        {
            ComponentResolver.ProcessComponentsInAllScenes();
            ComponentResolver.ProcessComponentsInAllPrefabs();
            ComponentResolver.ProcessAllAssetFiles();

        }
    }
    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        if (!hasPostAsset)
        {
            Log("Building Shader...", LogType.Info);
            EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
            if (hasObfuscated && _Config.Obfus.Keyfunc.ObfusType)
            {
                //if (!OverwriteAssetCheck(buildReport))
                //return;
                if (buildReport.summary.platform == BuildTarget.StandaloneWindows || buildReport.summary.platform == BuildTarget.StandaloneWindows64 ||
                    buildReport.summary.platform == BuildTarget.StandaloneLinux64 || buildReport.summary.platform == BuildTarget.StandaloneOSX)
                    return;
                string TempGameManger = BuildResolver.GetGameManagersAssetFromTemp();
                if (!File.Exists(TempGameManger))
                {
                    Log("NOT Found Temp Gamemanager Asset:" + TempGameManger, LogType.Warning);
                }
                else
                {
                    Log("Found Temp Gamemanager Asset:" + TempGameManger, LogType.Info);
                    ReplaceGamemanagerAsset(TempGameManger, monoSwapMaps);
                    hasPostAsset = true;
                }

                string CacheGameManagerPath = BuildResolver.GetGameManagerAssetFromCache(EditorUserBuildSettings.activeBuildTarget);
                if (!File.Exists(CacheGameManagerPath))
                {
                    Log("NOT Found Cache Gamemanager Asset:" + CacheGameManagerPath, LogType.Warning);
                }
                else
                {
                    Log("Found Cache Gamemanager Asset:" + CacheGameManagerPath, LogType.Info);
                    ReplaceGamemanagerAsset(CacheGameManagerPath, monoSwapMaps);
                    hasPostAsset = true;
                }
            }
        }
    }
    public void OnPostBuildPlayerScriptDLLs(BuildReport _Report)
    {
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        if (_Config == null)
        {
            EditorApplication.UnlockReloadAssemblies();
            Log("NOT Found Config!", LogType.Error);
            return;
        }
        if (!_Config.Enable_Obfuscator) return;
        if (!hasObfuscated)
        {
            if (BuildPipeline.isBuildingPlayer && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                string Asmpath;
                EditorApplication.LockReloadAssemblies();
                if (File.Exists(BuildResolver.GetAssemblyLocation("Assembly-CSharp.dll")))
                {
                    Asmpath = BuildResolver.GetAssemblyLocation("Assembly-CSharp.dll");
                    Log("Found Assembly:" + BuildResolver.GetAssemblyLocation("Assembly-CSharp.dll"), LogType.Info);
                    string[] ignoreMethods = ArrayCombine(ArrayCombine(ArrayCombine(ArrayCombine(_Config.Obfus.Keyfunc.Reflection.ignoreMethod, _Config.Obfus.Keyfunc.GUIComponent.ignoreMethod), _Config.Obfus.Keyfunc.AnimationComponent.ignoreMethod),_Config.Obfus.Keyfunc.custom_ignore_Method),_Config.Obfus.Keyfunc.UnityRuntime.ignoreMethod);
                    string[] ignoreTypes = ArrayCombine(_Config.Obfus.Keyfunc.custom_ignore_Class, _Config.Obfus.Keyfunc.Reflection.ignoreType);
                    string[] ignoreFields = ArrayCombine(_Config.Obfus.Keyfunc.custom_ignore_Field, _Config.Obfus.Keyfunc.UnityRuntime.ignoreField);
                    List<string> Mono = ComponentResolver.ScriptsResolver.ProjectScripts;
                    AssemblyLoader loader = new AssemblyLoader(File.ReadAllBytes(Asmpath));
                    List<Obfuscator> obfuscators = new List<Obfuscator>();
                    if (_Config.Obfus.Obfuscations.ControlFlow)
                    {
                        obfuscators.Add(new ControlFlow(loader.Module, _Config.Obfus.Keyfunc.ignore_ControlFlow_Method));
                    }
                    if (_Config.Obfus.Obfuscations.Obfusfunc)
                    {
                        if(_Config.Obfus.Keyfunc.ObfusType)
                            if(_Config.Obfus.Keyfunc.OnlyForProjectScripts)
                                obfuscators.Add(new ObfusFunc(loader.Module,ignoreMethods,ignoreFields,ignoreTypes,out monoSwapMaps, Mono, true));
                            else
                                obfuscators.Add(new ObfusFunc(loader.Module, ignoreMethods, ignoreFields, ignoreTypes, out monoSwapMaps, null, true));
                        else
                            obfuscators.Add(new ObfusFunc(loader.Module, ignoreMethods, ignoreFields, ignoreTypes, out monoSwapMaps, null, false));
                    }
                    if (_Config.Obfus.Obfuscations.NumObfus)
                    {
                        obfuscators.Add(new NumObfus(loader.Module));
                    }
                    if (_Config.Obfus.Obfuscations.LocalVariables2Field)
                    {
                        obfuscators.Add(new LocalVariables2Field(loader.Module));
                    }
                    if (_Config.Obfus.Obfuscations.StrCrypter)
                    {
                        obfuscators.Add(new StrCrypter(loader.Module));
                    }
                    if (_Config.Obfus.Obfuscations.AntiDe4dot)
                    {
                        obfuscators.Add(new Antide4dot(loader.Module));
                    }
                    if (_Config.Obfus.Obfuscations.FuckILdasm)
                    {
                        obfuscators.Add(new FuckILdasm(loader.Module));
                    }
                    if (_Config.Obfus.Obfuscations.MethodError && PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.Mono2x)
                    {
                        obfuscators.Add(new MethodError(loader.Module));
                    }
                    foreach (var obfuscator in obfuscators)
                    {
                        string outstr = obfuscator.ToString();
                        int i = outstr.IndexOf("Obfuscators.");
                        outstr = outstr.Substring(i + 12, outstr.Length - i - 12);
                        Log(outstr + " Executing...",LogType.Info);
                        obfuscator.Execute();
                    }
                    loader.Save(Asmpath);
                    if(_Config.Obfus.Obfuscations.PEPacker 
                        && PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.Mono2x 
                        && (_Report.summary.platform == BuildTarget.StandaloneWindows || _Report.summary.platform == BuildTarget.StandaloneWindows64))
                    {
                        PEPacker.pack(Asmpath);
                    }
                    hasObfuscated = true;
                    EditorApplication.UnlockReloadAssemblies();
                }
                else
                {
                    EditorApplication.UnlockReloadAssemblies();
                    Log("NOT Found Assembly!", LogType.Error);
                    return;
                }
                EditorApplication.UnlockReloadAssemblies();
            }
        }
    }
    public string GenerateAdditionalLinkXmlFile(BuildReport _Report, UnityLinkerBuildPipelineData _Data)
    {
        Log("GenerateAdditionalLinkXmlFile...", LogType.Info);
        /*
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        if (hasObfuscated && _Config.Obfus.Keyfunc.ObfusType)
        {
            if (!OverwriteAssetCheck(_Report))
                return null;
            if (_Report.summary.platform == BuildTarget.StandaloneWindows || _Report.summary.platform == BuildTarget.StandaloneWindows64 ||
                _Report.summary.platform == BuildTarget.StandaloneLinux64 || _Report.summary.platform == BuildTarget.StandaloneOSX)
                return null;
            string TempGameManger = BuildResolver.GetGameManagersAssetFromTemp();
            if (!File.Exists(TempGameManger))
            {
                Log("NOT Found Temp Gamemanager Asset:" + TempGameManger, LogType.Error);
            }
            else
            {
                Log("Found Temp Gamemanager Asset:" + TempGameManger, LogType.Info);
                ReplaceGamemanagerAsset(TempGameManger, monoSwapMaps);
            }

            string CacheGameManagerPath = BuildResolver.GetGameManagerAssetFromCache(EditorUserBuildSettings.activeBuildTarget);
            if (!File.Exists(CacheGameManagerPath))
            {
                Log("NOT Found Cache Gamemanager Asset:" + CacheGameManagerPath, LogType.Error);
            }
            else
            {
                Log("Found Cache Gamemanager Asset:" + CacheGameManagerPath, LogType.Info);
                ReplaceGamemanagerAsset(CacheGameManagerPath, monoSwapMaps);
            }
        }
        */
            return null;
    }
#if UNITY_2021_2_OR_NEWER
#else
    public void OnBeforeRun(BuildReport report, UnityLinkerBuildPipelineData data)
    {
        //Log("LinkXmlFile BeforeRun...");
    }

    public void OnAfterRun(BuildReport report, UnityLinkerBuildPipelineData data)
    {
        //Log("LinkXmlFile AfterRun...");
    }
#endif
    static string[] ArrayCombine(string[] a, string[] b)
    {
        string[] c = new string[a.Length + b.Length];

        Array.Copy(a, 0, c, 0, a.Length);
        Array.Copy(b, 0, c, a.Length, b.Length);

        return c;
    }
    public void OnPostprocessBuild(BuildReport _Report)
    {
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        if (_Config == null)
        {
            EditorApplication.UnlockReloadAssemblies();
            Log("NOT Found Config!", LogType.Error);
            return;
        }
        if (_Config.Enable_Obfuscator)
        {
            if (hasObfuscated && _Config.Obfus.Keyfunc.ObfusType)
            {
                //if (!OverwriteAssetCheck(_Report))
                    //return;
                if (!(_Report.summary.platform == BuildTarget.StandaloneWindows || _Report.summary.platform == BuildTarget.StandaloneWindows64 ||
                    _Report.summary.platform == BuildTarget.StandaloneLinux64 || _Report.summary.platform == BuildTarget.StandaloneOSX))
                    return;
                string path = BuildResolver.GetGameManagersAssetStandalone(_Report);
                if (File.Exists(path))
                {
                    Log("Found Gamemanager Asset:" + path, LogType.Info);
                    ReplaceGamemanagerAsset(path, monoSwapMaps);
                }
                else
                {
                    Log("NOT Found Manager Asset", LogType.Error);
                }
            }
            EtherLog _Log = ScriptableObject.CreateInstance<EtherLog>();
            List<SwapMap> smap = new List<SwapMap>();
            foreach (var map in monoSwapMaps)
            {
                smap.Add(new SwapMap
                {
                    OriginName = map.Key.Name,
                    ObfusName = map.Value.Name,
                });
            }
            _Log.Map = smap.ToArray();
            AssetDatabase.CreateAsset(_Log, "Assets/Plugins/Ether/log/Log.asset");
        }
        if(_Config.Enable_Il2CPP && PlayerSettings.GetScriptingBackend(BuildResolver.GetBuildTargetGroupByBuildTarget(EditorUserBuildSettings.activeBuildTarget)) == ScriptingImplementation.IL2CPP)
        {
            EtherIl2cppConfig config = new EtherIl2cppConfig();
            config.UnityVersion = _Config.il2cpp.UnityVersion;
            config.EncryptKey = _Config.il2cpp.Key;
            config.EnableStringEncrypt = _Config.il2cpp.StringEncrypt;
            config.EnableCheckSum = _Config.il2cpp.EnableCheckSum;
            config.EnableIl2cppAPIObfuscate = _Config.il2cpp.Il2cppAPIObfuscate;
            switch (_Report.summary.platform)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    {
                        new ExeEncrypter(_Report.summary.outputPath, config).Process();
                        Log("Ether Il2CPP Encryt: "+ _Report.summary.outputPath, LogType.Info);
                    }
                    break;
                case BuildTarget.Android:
                    {
                        new ApkEncrypter(_Report.summary.outputPath, config).Process();
                        Log("Ether Il2CPP Encryt: " + _Report.summary.outputPath, LogType.Info);
                    }
                    break;
                default: break;
            }
            try
            {
                Il2cppInstaller.UnInstall(System.Windows.Forms.Application.ExecutablePath);
                Log("Ether Il2CPP uninstall Successfully!", LogType.Info);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        EditorApplication.UnlockReloadAssemblies();
        return;
    }
    public string[] OnFilterAssemblies(BuildOptions _BuildOptions, string[] _Assemblies)
    {
        return _Assemblies;
    }
    public void ReplaceGamemanagerAsset(string path,Dictionary<TypeKey, TypeKey> Map)
    {
        AssetsFile asset = MonoUtils.LoadAsset(path);
        MonoUtils.SetMonoMapToAssetFile(asset, Map);
        MonoUtils.SaveAssetsToFile(asset, path);
    }
    public bool OverwriteAssetCheck(BuildReport report)
    {
        bool IsStandalone = report.summary.platform == BuildTarget.StandaloneWindows || report.summary.platform == BuildTarget.StandaloneWindows64 || report.summary.platform == BuildTarget.StandaloneLinux64 || report.summary.platform == BuildTarget.StandaloneOSX;
        bool IsCompressed = !typeof(EditorUserBuildSettings).GetMethod("GetCompressionType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { EditorUserBuildSettings.selectedBuildTargetGroup }).ToString().Equals("None");
        bool IsIl2CPP = PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup) == ScriptingImplementation.IL2CPP;
        if (IsCompressed)
            return false;
        if(!IsStandalone && !IsIl2CPP)
            return false;
        return true;
    }
    public enum LogType
    {
        Info,
        Warning,
        Error
    }
    public void Log(object msg, LogType type = LogType.Info)
    {
        switch(type)
        {
            case LogType.Info:
                Debug.Log("[Ether Uprotector]:" + msg);break;
            case LogType.Warning:
                Debug.LogWarning("[Ether Uprotector]:" + msg);break;
            case LogType.Error:
                Debug.LogError("[Ether Uprotector]:" + msg);break;
        }
    }
    
}
