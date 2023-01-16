using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace Ether_IL2CPP
{
    public class JsonIndex
    {
        public int key;
        public string Version;
        public ObfusConfig Obfus;
    }
    public class ObfusConfig
    {
        public int ControlFlow { get; set; }
        public string[] ignore_ControlFlow_Method { get; set; }
        public int NumObfus { get; set; }
        public int LocalVariables2Field { get; set; }
        public int StrCrypter { get; set; }
        public int Obfusfunc { get; set; }
        public int AntiDe4dot { get; set; }
        public int FuckILdasm { get; set; }
        public int PEPacker { get; set; }

        public int MethodError { get; set; }
    }
    public class JsonManager
    {
        public JsonIndex index;
        public string path;
        public JsonManager(string _path)
        {
            path = _path;
            Read();
        }
        public void Read()
        {
            index = JsonMapper.ToObject<JsonIndex>(File.ReadAllText(path));
        }
        public void Set()
        {
            File.WriteAllText(path,JsonMapper.ToJson(index));
        }
    }
}
