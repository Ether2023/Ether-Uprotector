using Ether_Obfuscator.Obfuscators.Resolver;
using Ether_Obfuscator.Obfuscators.Unity;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ComponentResolver
{
    public static List<object> ComponentPool = new List<object>();
    public static AssetsResolver AssetsResolver = new AssetsResolver();
    public static GUIComponetResolver GUIComponetResolver = new GUIComponetResolver();
    public static ScriptsResolver ScriptsResolver = new ScriptsResolver();
    public static AnimationResolver AnimationResolver = new AnimationResolver();
    public static List<object> GetComponents()
    {
        List<object> Components = new List<object>();
        List<object> var_UserPluginComponents = new List<object> { };
        var_UserPluginComponents = GetPluginComponents();
        for (int c = 0; c < var_UserPluginComponents.Count; c++)
        {
            Components.Add(var_UserPluginComponents[c]);
        }
        return Components;
    }
    public static List<object> GetPluginComponents()
    {
        List<object> var_UserPluginComponentList = new List<object>();
        Type var_Type = typeof(object);
        IEnumerable<Type> var_AllTypes = from t in GetLoadableTypes() where var_Type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract select t;
        foreach (Type var_InheritedType in var_AllTypes)
        {
            if (var_InheritedType != null)
            {
                object var_Instance = Activator.CreateInstance(var_InheritedType);
                var_UserPluginComponentList.Add(var_Instance);
            }
        }
        return var_UserPluginComponentList;
    }
    public static IEnumerable<Type> GetLoadableTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => GetLoadableTypes(a));
    }
    public static IEnumerable<Type> GetLoadableTypes(Assembly _Assembly)
    {
        if (_Assembly == null)
        {
            throw new ArgumentNullException("assembly");
        }
        IEnumerable<Type> enumerable;
        try
        {
            enumerable = _Assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            enumerable = e.Types.Where((Type t) => t != null);
        }
        return enumerable;
    }
    public static void ProcessAllAssetFiles()
    {
        foreach (var asset in AssetsResolver.AssetsRefList)
        {
            AnimationResolver.ResolveAsset(asset);
            ScriptsResolver.ResolveProjectScripts(asset);
        }
    }
    public static void ProcessComponentsInAllScenes()
    {
        List<string> BuildScene = AssetsResolver.GetAllBuildScene();
        List<string> LoadScene = AssetsResolver.GetAllLoadScene();
        for (int i = 0; i < LoadScene.Count; i++)
        {
            string ScenePath = LoadScene[i];
            try
            {
                Scene s = SceneManager.GetSceneByPath(ScenePath);
                List<Component> SceneComponentList = GetAllComponentFromScene(s);
                ProcessComponents(SceneComponentList);
            }
            finally
            {
                BuildScene.Remove(ScenePath);
            }
        }
        for (int j = 0; j < BuildScene.Count; j++)
        {
            string ScenePath = BuildScene[j];
            Scene s = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            List<Component> SceneComponentList = GetAllComponentFromScene(s);
            ProcessComponents(SceneComponentList);

        }
        LoadScenes(LoadScene);
        return;
    }
    public static void ProcessComponentsInAllPrefabs()
    {
        List<Component> PrefabsComponent = GetAllComponeFromPrefabs(AssetsResolver);
        ProcessComponents(PrefabsComponent);
    }
    public static List<Component> GetAllComponeFromPrefabs(AssetsResolver assetsResolver)
    {
        List<UnityAssetReference> AssetReferenceList = assetsResolver.GetAssetsRefWithExtension(".prefab");
        List<Component> ret = new List<Component>();
        for (int i = 0; i < AssetReferenceList.Count; i++)
        {
            string path = AssetReferenceList[i].Path;
            GameObject GameObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Component[] Components = GameObj.GetComponentsInChildren<Component>(true);
            ret.AddRange(Components);
        }
        return ret;
    }
    public static List<Component> GetAllComponentFromScene(Scene s)
    {
        List<Component> ret = new List<Component>();
        GameObject[] RootGameobjs = s.GetRootGameObjects();
        foreach (GameObject Gameobj in RootGameobjs)
        {
            if (!(Gameobj == null))
            {
                Component[] Components = Gameobj.GetComponentsInChildren<Component>(true);
                ret.AddRange(Components);
            }
        }
        return ret;
    }
    public static void ProcessComponents(List<Component> componentList)
    {
        foreach (Component component in componentList)
        {
            GUIComponetResolver.ResolveGUIComponent(component);
            //ScriptsResolver.ResolveComponent(component);
        }
    }
    public static void LoadScenes(List<string> s)
    {
        if (s.Count == 0 || string.IsNullOrEmpty(s[0]))
        {
            return;
        }
        List<string> LoadedScenes = AssetsResolver.GetAllLoadScene();
        if (LoadedScenes.SequenceEqual(s))
        {
            return;
        }
        EditorSceneManager.OpenScene(s[0], OpenSceneMode.Single);
        for (int i = 1; i < s.Count; i++)
        {
            EditorSceneManager.OpenScene(s[i], OpenSceneMode.Additive);
        }
    }
}
