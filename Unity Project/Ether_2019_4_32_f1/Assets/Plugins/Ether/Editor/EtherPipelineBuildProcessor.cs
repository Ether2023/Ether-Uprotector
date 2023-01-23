using Codice.Client.BaseCommands;
using Ether_Obfuscator;
using Ether_Obfuscator.Obfuscators;
using Ether_Obfuscator.Obfuscators.UnityMonoBehavior;
using Ether_Obfuscator.Unity;
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
using UnityEditor.UnityLinker;
using UnityEngine;

public class EtherPipelineBuildProcessor : IPreprocessBuildWithReport, IFilterBuildAssemblies, IPostBuildPlayerScriptDLLs, IUnityLinkerProcessor, IPostprocessBuildWithReport
{
    static bool hasObfuscated = false;
    Dictionary<string,string> monoSwapMaps = new Dictionary<string, string>();
    public int callbackOrder
    {
        get { return 0; }
    }
    public void OnPreprocessBuild(BuildReport _Report)
    {
        hasObfuscated = false;
        ComponentResolver.ProcessComponentsInAllScenes();
        ComponentResolver.ProcessComponentsInAllPrefabs();
        ComponentResolver.ProcessAllAssetFiles();
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
        if (!_Config.Enable) return;
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
                    if (_Config.Obfus.Obfuscations.MethodError)
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
                    if(_Config.Obfus.Obfuscations.PEPacker)
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
        AssetsFile assets;
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        if (hasObfuscated && _Config.Obfus.Obfuscations.Obfusfunc)
        {
            string TempGameManger = BuildResolver.GetGameManagersAssetFromTemp();
            Debug.Log(TempGameManger);
            if (File.Exists(TempGameManger))
            {
                Log("Found Temp Gamemanager Asset:" + TempGameManger, LogType.Info);
                ReplaceGamemanagerAsset(TempGameManger, monoSwapMaps);
            }
            string CacheGameManagerPath = BuildResolver.GetGameManagerAssetFromCache(EditorUserBuildSettings.activeBuildTarget);
            Debug.Log(CacheGameManagerPath);
            if (File.Exists(CacheGameManagerPath))
            {
                Log("Found Cache Gamemanager Asset:" + CacheGameManagerPath, LogType.Info);
                ReplaceGamemanagerAsset(CacheGameManagerPath, monoSwapMaps);
            }
            if (!File.Exists(CacheGameManagerPath) && !File.Exists(TempGameManger))
            {
                Log("NOT Found Manager Asset", LogType.Error);
            }
        }
            return null;
    }
#if UNITY_2021_2_OR_NEWER
#else
    public void OnBeforeRun(BuildReport report, UnityLinkerBuildPipelineData data)
    {
    }

    public void OnAfterRun(BuildReport report, UnityLinkerBuildPipelineData data)
    {
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
        AssetsFile assets;
        EtherConfig _Config = AssetDatabase.LoadAssetAtPath<EtherConfig>("Assets/Plugins/Ether/Config.asset");
        if (_Config == null)
        {
            EditorApplication.UnlockReloadAssemblies();
            Log("NOT Found Config!", LogType.Error);
            return;
        }
        if (hasObfuscated && _Config.Obfus.Obfuscations.Obfusfunc)
        {
            string path = BuildResolver.GetGameManagersAssetStandalone(_Report);
            if(File.Exists(path))
            {
                Log("Found Cache Gamemanager Asset:" + path, LogType.Info);
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
                OriginName = map.Key,
                ObfusName = map.Value,
            });
        }
        _Log.Map = smap.ToArray();
        AssetDatabase.CreateAsset(_Log, "Assets/Plugins/Ether/log/Log.asset");
        EditorApplication.UnlockReloadAssemblies();
        return;
    }
    public string[] OnFilterAssemblies(BuildOptions _BuildOptions, string[] _Assemblies)
    {
        return _Assemblies;
    }
    public void ReplaceGamemanagerAsset(string path,Dictionary<string,string> Map)
    {
        AssetsFile asset = MonoUtils.LoadAsset(path);
        MonoUtils.SetMonoMapToAssetFile(asset, Map);
        MonoUtils.SaveAssetsToFile(asset, path);
    }
    public enum LogType
    {
        Info,
        Warning,
        Error
    }
    public void Log(object msg, LogType type)
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
