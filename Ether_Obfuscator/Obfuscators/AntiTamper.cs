using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Ether_Obfuscator.Obfuscators
{
    public class AntiTamper : Obfuscator
    {
        ModuleDefMD ModuleDefMD;
        public AntiTamper(ModuleDefMD moduleDef)
        { 
            ModuleDefMD = moduleDef;
        }
        public void Execute()
        {
            var cstype = Utils.GetRuntimeTypeSelf("Ether_Obfuscator.Runtime.AntiTamperChecker");
            var antimethod = cstype.FindMethod("TamperCheck");
            antimethod.DeclaringType = null;
            TypeDefUser AntiTampertypeDef = new TypeDefUser("","AntiTamper", ModuleDefMD.CorLibTypes.GetTypeRef("System","Object"));
            AntiTampertypeDef.Attributes = dnlib.DotNet.TypeAttributes.AutoClass | dnlib.DotNet.TypeAttributes.BeforeFieldInit;
            NameGenerator.SetObfusName(AntiTampertypeDef, NameGenerator.Mode.RandomString, 1, 5);
            ModuleDefMD.Types.Add(AntiTampertypeDef);
            ModuleDefMD.Types.Single(x => x.Name == AntiTampertypeDef.Name).Methods.Add(antimethod);
            NameGenerator.SetObfusName(antimethod, NameGenerator.Mode.FuncName, 7);
            var cctor = ModuleDefMD.GlobalType.FindOrCreateStaticConstructor();
            cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Nop));
            cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, ModuleDefMD.Types.Single(x => x.Name == AntiTampertypeDef.Name).FindMethod(antimethod.Name)));
        }
        public static void CreateHashAndInjectAssembly(string Path)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] AssemblyData = hash.ComputeHash(File.ReadAllBytes(Path));
                using (FileStream Writer = new FileStream(Path, FileMode.Append))
                    Writer.Write(AssemblyData, 0, AssemblyData.Length);
            }
        }
    }
}
