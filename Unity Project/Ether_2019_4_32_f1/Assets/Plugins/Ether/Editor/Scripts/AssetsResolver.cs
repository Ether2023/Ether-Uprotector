using Ether_Obfuscator.Obfuscators.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetsResolver
{
    public List<UnityAssetReference> AssetsRefList = new List<UnityAssetReference>();
    public AssetsResolver()
    {
        GetAllAssets();
    }
    void GetAllAssets()
    {
        string[] AssetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string path in AssetPaths)
        {
            if (path.StartsWith("Assets") || path.StartsWith("Packages"))
            {
                //Debug.Log(var_AssetPath);
                AssetsRefList.Add(new UnityAssetReference(path));
            }
        }
    }
    public List<UnityAssetReference> GetAssetsRefWithExtension(string extension)
    {
        List<UnityAssetReference> ret = new List<UnityAssetReference>();
        foreach(var asset in AssetsRefList)
        {
            if(asset.FileExtension == extension)
            { 
                ret.Add(asset); 
            }
        }
        return ret;
    }
    public static List<string> GetAllBuildScene()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToList<string>();
    }
    public static List<string> GetAllLoadScene()
    {
        List<string> Scenes = new List<string>();
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            Scenes.Add(SceneManager.GetSceneAt(i).path);
        }
        return Scenes;
    }
}
