using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        string Path;
        public AntiTamper(ModuleDefMD moduleDef,string Path)
        { 
            ModuleDefMD = moduleDef;
            this.Path = Path;
        }
        public void Execute()
        {

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
