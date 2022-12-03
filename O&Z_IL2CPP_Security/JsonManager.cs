using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using O_Z_IL2CPP_Security.LitJson;

namespace O_Z_IL2CPP_Security
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
        public int NumObfus { get; set; }
        public int LocalVariables2Field { get; set; }
        public int StrCrypter { get; set; }
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
