using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ether_Obfuscator.Runtime
{
    public class AntiTamperChecker
    {
        public void TamperCheck()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            Stream baseStream = new StreamReader(location).BaseStream;
            BinaryReader binaryReader = new BinaryReader(baseStream);
            string temphash = BitConverter.ToString(SHA256.Create().ComputeHash(binaryReader.ReadBytes(File.ReadAllBytes(location).Length - 16)));
            baseStream.Seek(-16L, SeekOrigin.End);
            string checkhash = BitConverter.ToString(binaryReader.ReadBytes(16));
            if (temphash != checkhash)
            {
                throw new BadImageFormatException();
            }
        }
    }
}
