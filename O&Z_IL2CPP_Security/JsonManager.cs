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
        public int Obfusfunc { get; set; }
    }
    public class JsonManager
    {
        public JsonIndex index;
        public string path;
        public static string origin = "ewogICAgImtleSI6MTE0NTE0LAogICAgIlZlcnNpb24iOiIyNC40IiwKICAgICIvLyI6IuaUr+aMgTI4IDI0LjQiLAogICAgIk9iZnVzIjoKICAgIHsKICAgICAgICAiQ29udHJvbEZsb3ciOjEsCiAgICAgICAgIk51bU9iZnVzIjoxLAogICAgICAgICJMb2NhbFZhcmlhYmxlczJGaWVsZCI6MSwKICAgICAgICAiU3RyQ3J5cHRlciI6MSwKICAgICAgICAiT2JmdXNmdW5jIjoxLAogICAgICAgICIvLyI6IjA95YWz6ZetIDE95byA5ZCvIiwKICAgICAgICAiLy8iOiJDb250cm9sRmxvd++8muaOp+WItua1geeoi+a3t+a3hiIsCiAgICAgICAgIi8vIjoiTnVtT2JmdXPvvJrmlbDlrZfmt7fmt4YiLAogICAgICAgICIvLyI6IkxvY2FsVmFyaWFibGVzMkZpZWxk77ya5bGA6YOo5Y+Y6YeP6L2s5o2i5Li65a2X5q61IiwKICAgICAgICAiLy8iOiJTdHJDcnlwdGVy77ya5a2X56ym5Liy5Yqg5a+GIiwKICAgICAgICAiLy8iOiJPYmZ1c2Z1bmPvvJroh6rlrprkuYnmt7fmt4bnsbvlkozmlrnms5XlkI3np7As5Y+v5Lul5Zyoa2V5ZnVuY+S4reiHquWumuS5iea3t+a3huaooeW8jyjlpoLnlKjliLDkuoblj43lsITnrYnnsbvlnospIgoKICAgIH0KfQ==";
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
