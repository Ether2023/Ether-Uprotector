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
        public int AntiDe4dot { get; set; }
        public int FuckILdasm { get; set; }
        public int PEPacker { get; set; }
    }
    public class JsonManager
    {
        public JsonIndex index;
        public string path;
        public static string origin = "ew0KICAgICJrZXkiOjExNDUxNCwNCiAgICAiVmVyc2lvbiI6IjI0LjQiLA0KICAgICIvLyI6IuaUr+aMgTI4IDI0LjQiLA0KICAgICJPYmZ1cyI6DQogICAgew0KICAgICAgICAiQ29udHJvbEZsb3ciOjEsDQogICAgICAgICJpZ25vcmVfQ29udHJvbEZsb3dfTWV0aG9kIjpbDQogICAgICAgICIiLCIiDQogICAgICAgIF0sDQogICAgICAgICIvLyI6IumDqOWIhuaWueazleWPr+iDvea3t+a3huS5i+WQjuacieWwj+amgueOh+aKpemUme+8iOWkp+WkmuaVsOWcqElMMkNQUOaehOW7uuS5i+S4reaPkuWFpeS6huacrOa3t+a3huWKn+iDve+8ie+8jOWPr+S7peWkmuWwneivleWHoOasoe+8jOWmguaenOS9oOS4jeaDs+Wwneivle+8jOWPr+S7peebtOaOpeWwhuaKpemUmeeahOaWueazlea3u+WKoOWcqOatpOWkhO+8jOWcqOaJp+ihjENvbnRyb2xGbG935pe25bCG5LiN5YaN5a+55q2k5pa55rOV6L+b6KGM5re35reGIiwNCiAgICAgICAgIk51bU9iZnVzIjoxLA0KICAgICAgICAiTG9jYWxWYXJpYWJsZXMyRmllbGQiOjEsDQogICAgICAgICJTdHJDcnlwdGVyIjoxLA0KICAgICAgICAiT2JmdXNmdW5jIjoxLA0KICAgICAgICAiQW50aURlNGRvdCI6MSwNCiAgICAgICAgIkZ1Y2tJTGRhc20iOjEsDQogICAgICAgICJQRVBhY2tlciI6MSwNCiAgICAgICAgIi8vIjoiMD3lhbPpl60gMT3lvIDlkK8iLA0KICAgICAgICAiLy8iOiJDb250cm9sRmxvd++8muaOp+WItua1geeoi+a3t+a3hiIsDQogICAgICAgICIvLyI6Ik51bU9iZnVz77ya5pWw5a2X5re35reGIiwNCiAgICAgICAgIi8vIjoiTG9jYWxWYXJpYWJsZXMyRmllbGTvvJrlsYDpg6jlj5jph4/ovazmjaLkuLrlrZfmrrUiLA0KICAgICAgICAiLy8iOiJTdHJDcnlwdGVy77ya5a2X56ym5Liy5Yqg5a+GIiwNCiAgICAgICAgIi8vIjoiT2JmdXNmdW5j77ya6Ieq5a6a5LmJ5re35reG57G75ZKM5pa55rOV5ZCN56ewLOWPr+S7peWcqGtleWZ1bmPkuK3oh6rlrprkuYnmt7fmt4bmqKHlvI8o5aaC55So5Yiw5LqG5Y+N5bCE562J57G75Z6LKSIsDQogICAgICAgICIvLyI6IkFudGlEZTRkb3Q66YCa6L+H5re75Yqg54m55q6K5bGe5oCn5L2/5b6XZGU0ZG905peg5rOV5q2j56Gu6K+G5Yir6KKr5re35reG55qE5ZCN56ew77yM5LuO6ICM5L2/5YW25peg5rOV6L+Y5Y6f5q2j56Gu5Y+N5re35reG5ZCN56ewIiwNCiAgICAgICAgIi8vIjoiRnVja0lMZGFzbTrpgJrov4dNU+aPkOS+m+eahFN1cHByZXNzSWxkYXNtQXR0cmlidXRl5L2/5Y+N57yW6K+R5Zmo5peg5rOV5q2j5bi45bel5L2cIiwNCiAgICAgICAgIi8vIjoiUEVQYWNrZXI65bCB6KOFTkVU56iL5bqP6ZuG77yM5Y676ZmkTkVU56ym5Y+377yM5L2/5b6X5Y+N57yW6K+R5Zmo5peg5rOV5q2j56Gu6K+G5YirTkVU56iL5bqP6ZuGIg0KICAgIH0NCn0=";
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
