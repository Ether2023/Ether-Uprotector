using Ether_Obfuscator.Obfuscators.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MonoScript = UnityEditor.MonoScript;
using MonoType = Ether_Obfuscator.Obfuscators.Unity.MonoType;

public class ScriptsResolver
{
    public List<MonoType> ReferencedMonoScriptTypeList = new List<MonoType>();
    public List<string> ProjectScripts = new List<string>();
    public void ResolveProjectScripts(UnityAssetReference AssetReference)
    {
        if(AssetReference.FileExtension == ".cs" && AssetReference.Path.StartsWith("Assets"))
        {
            ProjectScripts.Add(AssetReference.FileName);
        }
        return;
    }
    public bool ResolveComponent(Component _Component)
    {
        if (_Component is MonoBehaviour)
        {
            MonoScript MonoScripts = UnityEditor.MonoScript.FromMonoBehaviour(_Component as MonoBehaviour);
            Type type = MonoScripts.GetClass();
            MonoType monoClass = new MonoType(type.Assembly.GetName().Name, type.Namespace, type.Name);
            if (!ReferencedMonoScriptTypeList.Contains(monoClass))
            {
                ReferencedMonoScriptTypeList.Add(monoClass);
            }
        }
        List<MonoScript> FoundMonoScripts = SearchVariablesWithSerializedObject(_Component);
        if (FoundMonoScripts.Count == 0)
        {
            return true;
        }
        for (int i = 0; i < FoundMonoScripts.Count; i++)
        {
            Type type = FoundMonoScripts[i].GetClass();
            MonoType Class = new MonoType(type.Assembly.GetName().Name, type.Namespace, type.Name);
            if (!ReferencedMonoScriptTypeList.Contains(Class))
            {
                ReferencedMonoScriptTypeList.Add(Class);
            }
        }
        return true;
    }
    private static List<MonoScript> SearchVariablesWithSerializedObject(UnityEngine.Object _Object)
    {
        HashSet<MonoScript> Result = new HashSet<MonoScript>();
        SerializedObject SerializedObject = new SerializedObject(_Object);
        SerializedProperty Iterator = SerializedObject.GetIterator().Copy();
        Iterator.NextVisible(true);
        SerializedProperty Iterator_Children = SerializedObject.GetIterator().Copy();
        bool SearchChildren = true;
        HashSet<string> PropertyPathAlreadySearchedHashSet = new HashSet<string>();
        if (Iterator_Children.NextVisible(SearchChildren))
        {
            while (!PropertyPathAlreadySearchedHashSet.Contains(Iterator_Children.propertyPath))
            {
                PropertyPathAlreadySearchedHashSet.Add(Iterator_Children.propertyPath);
                if (SerializedProperty.EqualContents(Iterator, Iterator_Children))
                {
                    Iterator.NextVisible(false);
                    SearchChildren = true;
                }
                else
                {
                    SearchChildren = false;
                }
                SerializedPropertyType propertyType = Iterator_Children.propertyType;
                if (propertyType == SerializedPropertyType.ObjectReference && Iterator_Children.objectReferenceValue is MonoScript)
                {
                    MonoScript MonoScript = (MonoScript)Iterator_Children.objectReferenceValue;
                    if (!Result.Contains(MonoScript))
                    {
                        Result.Add(MonoScript);
                    }
                }
                if (!Iterator_Children.NextVisible(SearchChildren))
                {
                    break;
                }
            }
        }
        return Result.ToList<MonoScript>();
    }
}
