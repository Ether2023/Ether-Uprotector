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
        public string[] ignore_ControlFlow_Method { get; set; }
        public int NumObfus { get; set; }
        public int LocalVariables2Field { get; set; }
        public int StrCrypter { get; set; }
        public int Obfusfunc { get; set; }
    }
    public class JsonManager
    {
        public JsonIndex index;
        public string path;
        public static string origin = "ewogICAgImtleSI6MTE0NTE0LAogICAgIlZlcnNpb24iOiIyNC40IiwKICAgICIvLyI6IuaUr+aMgTI4IDI0LjQiLAogICAgIk9iZnVzIjoKICAgIHsKICAgICAgICAiQ29udHJvbEZsb3ciOjEsCiAgICAgICAgImlnbm9yZV9Db250cm9sRmxvd19NZXRob2QiOlsKICAgICAgICAiIiwiIgogICAgICAgIF0sCiAgICAgICAgIi8vIjoi6YOo5YiG5pa55rOV5Y+v6IO95re35reG5LmL5ZCO5pyJ5bCP5qaC546H5oql6ZSZ77yI5aSn5aSa5pWw5ZyoSUwyQ1BQ5p6E5bu65LmL5Lit5o+S5YWl5LqG5pys5re35reG5Yqf6IO977yJ77yM5Y+v5Lul5aSa5bCd6K+V5Yeg5qyh77yM5aaC5p6c5L2g5LiN5oOz5bCd6K+V77yM5Y+v5Lul55u05o6l5bCG5oql6ZSZ55qE5pa55rOV5re75Yqg5Zyo5q2k5aSE77yM5Zyo5omn6KGMQ29udHJvbEZsb3fml7blsIbkuI3lho3lr7nmraTmlrnms5Xov5vooYzmt7fmt4YiLAogICAgICAgICJOdW1PYmZ1cyI6MSwKICAgICAgICAiTG9jYWxWYXJpYWJsZXMyRmllbGQiOjEsCiAgICAgICAgIlN0ckNyeXB0ZXIiOjEsCiAgICAgICAgIk9iZnVzZnVuYyI6MSwKICAgICAgICAiLy8iOiIwPeWFs+mXrSAxPeW8gOWQryIsCiAgICAgICAgIi8vIjoiQ29udHJvbEZsb3fvvJrmjqfliLbmtYHnqIvmt7fmt4YiLAogICAgICAgICIvLyI6Ik51bU9iZnVz77ya5pWw5a2X5re35reGIiwKICAgICAgICAiLy8iOiJMb2NhbFZhcmlhYmxlczJGaWVsZO+8muWxgOmDqOWPmOmHj+i9rOaNouS4uuWtl+autSIsCiAgICAgICAgIi8vIjoiU3RyQ3J5cHRlcu+8muWtl+espuS4suWKoOWvhiIsCiAgICAgICAgIi8vIjoiT2JmdXNmdW5j77ya6Ieq5a6a5LmJ5re35reG57G75ZKM5pa55rOV5ZCN56ewLOWPr+S7peWcqGtleWZ1bmPkuK3oh6rlrprkuYnmt7fmt4bmqKHlvI8o5aaC55So5Yiw5LqG5Y+N5bCE562J57G75Z6LKSIKCiAgICB9Cn0=";
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
