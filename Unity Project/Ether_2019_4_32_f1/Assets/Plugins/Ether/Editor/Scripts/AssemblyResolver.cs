using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.Linq;

public class AssemblyResolver
{
    public List<Assembly> Assemblies;
    public Assembly Assembly_CSharp;
    public Assembly Assembly_CSharp_firstpass;
    public List<Assembly> BuildAssemblies = new List<Assembly>();
    public AssemblyResolver()
    {
        Assemblies = new List<Assembly>(CompilationPipeline.GetAssemblies(AssembliesType.Player));
        foreach (var asm in Assemblies)
        {
            if(asm.name == "Assembly-CSharp") Assembly_CSharp = asm;
            if(asm.name == "Assembly-CSharp-firstpass") Assembly_CSharp_firstpass = asm;
            if(IsBuild(asm)) BuildAssemblies.Add(asm);
        }
    }
    public bool IsBuild(Assembly asm)
    {
        if (asm.name != "Assembly-CSharp" && asm.name != "Assembly-CSharp-firstpass" && asm.sourceFiles != null)
            if (asm.sourceFiles.Length != 0 && !IsTestAssembly(asm))
                return true;
            else
                return false;
        else
            return false;
    }
    public static bool IsTestAssembly(Assembly asm)
    {
        return asm.assemblyReferences.Where((Assembly a) => a.name == "UnityEngine.TestRunner").Count<Assembly>() > 0;
    }
    public static bool IsAssetsAssembly(Assembly asm)
    {
        if(asm.sourceFiles[0].StartsWith("Assets")) return true;
        return false;
    }
    public static bool IsPackageAssembly(Assembly asm)
    {
        if (asm.sourceFiles[0].StartsWith("Packages")) return true;
        return false;
    }
}
