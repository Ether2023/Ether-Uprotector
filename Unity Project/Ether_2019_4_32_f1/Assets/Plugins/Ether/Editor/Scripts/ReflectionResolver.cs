using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ether_Obfuscator;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.IO;
public class ReflectionResolver
{
    public List<string> RelectionTypeList;
    public List<string> RelectionMethodList;
    public List<string> RelectionNamespaceList;
    public ReflectionResolver()
    {
        AssemblyLoader assemblyLoader = new AssemblyLoader(File.ReadAllBytes(Application.dataPath.Replace("Assets","") + "/Library/ScriptAssemblies/Assembly-CSharp.dll"));
        Ether_Obfuscator.Obfuscators.Resolver.ReflectionResolver resolver = new Ether_Obfuscator.Obfuscators.Resolver.ReflectionResolver(assemblyLoader.Module);
        RelectionMethodList = resolver.Reflections.Method;
        RelectionTypeList = resolver.Reflections.Type;
        RelectionNamespaceList = resolver.Reflections.Namespace;
    }
}
