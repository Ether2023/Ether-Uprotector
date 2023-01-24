using Ether_Obfuscator.Obfuscators.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimationResolver
{
    public static HashSet<string> modelFileExtension = new HashSet<string> { ".fbx", ".dae", ".3ds", ".dxf", ".obj", ".skp" };
    public static HashSet<string> animationFileExtension = new HashSet<string> { ".anim" };
    public HashSet<string> ReferencedAnimationMethodHashSet = new HashSet<string>();
    public bool ResolveAsset(UnityAssetReference AssetReference)
    {
        if (modelFileExtension.Contains(AssetReference.FileExtension))
        {
            //Debug.Log(_UnityAssetReference.FileName);
            ResolveModelFile(AssetReference);
        }
        if (animationFileExtension.Contains(AssetReference.FileExtension))
        {
            //Debug.Log(_UnityAssetReference.FileName);
            ResolveAnimationClip(AssetReference);
        }
        return true;
    }
    private void ResolveModelFile(UnityAssetReference AssetReference)
    {
        Object[] Assets = AssetDatabase.LoadAllAssetsAtPath(AssetReference.Path);
        if (Assets == null || Assets.Length == 0)
        {
            return;
        }
        List<string> AnimationMethodList = new List<string>();
        for (int a = 0; a < Assets.Length; a++)
        {
            if (Assets[a] is AnimationClip)
            {
                AnimationMethodList.AddRange(GetMethodsInAnimationClip(Assets[a] as AnimationClip));
            }
        }
        for (int i = 0; i < AnimationMethodList.Count; i++)
        {
            if (!this.ReferencedAnimationMethodHashSet.Contains(AnimationMethodList[i]))
            {
                this.ReferencedAnimationMethodHashSet.Add(AnimationMethodList[i]);
            }
        }
        //Debug.Log("Found Animation in model:" + AssetReference.Path);
        //Debug.Log(AnimationMethodList.Count);
        return;
    }

    private bool ResolveAnimationClip(UnityAssetReference AssetReference)
    {
        AnimationClip AnimationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetReference.Path);
        if (AnimationClip == null)
        {
            return true;
        }
        List<string> AnimationMethodList = this.GetMethodsInAnimationClip(AnimationClip);
        for (int i = 0; i < AnimationMethodList.Count; i++)
        {
            if (!this.ReferencedAnimationMethodHashSet.Contains(AnimationMethodList[i]))
            {
                this.ReferencedAnimationMethodHashSet.Add(AnimationMethodList[i]);
            }
        }
        //Debug.Log("Found Animation:" + AssetReference.Path);
        return true;
    }
    private List<string> GetMethodsInAnimationClip(AnimationClip Ani)
    {
        if (Ani.events == null)
        {
            return new List<string>();
        }
        HashSet<string> Result = new HashSet<string>();
        for (int e = 0; e < Ani.events.Length; e++)
        {
            if (Ani.events[e] != null && !Result.Contains(Ani.events[e].functionName))
            {
                Result.Add(Ani.events[e].functionName);
            }
        }
        return Result.ToList<string>();
    }
}
