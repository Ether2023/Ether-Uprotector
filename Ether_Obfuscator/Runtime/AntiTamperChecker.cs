using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Ether_Obfuscator.Runtime
{
    public class AntiTamperChecker
    {
        public static void TamperCheck()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            Stream baseStream = new StreamReader(location).BaseStream;
            BinaryReader binaryReader = new BinaryReader(baseStream);
            string temphash = BitConverter.ToString(SHA256.Create().ComputeHash(binaryReader.ReadBytes(File.ReadAllBytes(location).Length - 32)));
            baseStream.Seek(-32, SeekOrigin.End);
            string checkhash = BitConverter.ToString(binaryReader.ReadBytes(32));
            if (temphash != checkhash)
            {
                UnityEngine.Diagnostics.Utils.ForceCrash(ForcedCrashCategory.FatalError);
            }
        }
    }
}
