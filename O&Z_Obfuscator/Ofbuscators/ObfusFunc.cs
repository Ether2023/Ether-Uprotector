using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using O_Z_IL2CPP_Security.LitJson;
using OZ_Obfus;

namespace OZ_Obfuscator.Obfuscators
{
    public class ObfusFunc : Obfuscator
    {
        ModuleDefMD module;
        List<string> ignoreMethod = new List<string>();
        List<string> ignoreField = new List<string>();
        List<string> obfusClass = new List<string>();
        public ObfusFunc(ModuleDefMD module)
        {
            this.module = module;
            if(!File.Exists("keyfunc.json"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("keyfunc.json not found!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("正在重新生成默认keyfunc.json...");
                Console.WriteLine("如果您不想使用默认keyfunc.json,请重新配置keyfunc.json");
                Console.ForegroundColor = ConsoleColor.White;
                File.WriteAllBytes("keyfunc.json", Convert.FromBase64String(ignore.origin));
            }
            ignore ig = JsonMapper.ToObject<ignore>(File.ReadAllText("keyfunc.json"));
            foreach (var item in ig.ignoreMethod)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in ig.ignoreField)
                ignoreField.Add(item.ToLower());
            foreach(var item in ig.custom_ignore_Method)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in ig.custom_ignore_Field)
                ignoreField.Add(item.ToLower());
            foreach (var item in ig.custom_obfus_Class)
                obfusClass.Add(item.ToLower());
        }
        public ObfusFunc(ModuleDefMD module,string Keyfunc)
        {
            this.module = module;
            ignore ig = JsonMapper.ToObject<ignore>(Keyfunc);
            foreach (var item in ig.ignoreMethod)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in ig.ignoreField)
                ignoreField.Add(item.ToLower());
            foreach (var item in ig.custom_ignore_Method)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in ig.custom_ignore_Field)
                ignoreField.Add(item.ToLower());
            foreach (var item in ig.custom_obfus_Class)
                obfusClass.Add(item.ToLower());
        }
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _custom_ignore_Method, string[] _custom_ignore_Field, string[] _obfusClass)
        {
            this.module = module;
            foreach (var item in _ignoreMethod)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in _ignoreField)
                ignoreField.Add(item.ToLower());
            foreach (var item in _custom_ignore_Method)
                ignoreMethod.Add(item.ToLower());
            foreach (var item in _custom_ignore_Field)
                ignoreField.Add(item.ToLower());
            foreach (var item in _obfusClass)
                obfusClass.Add(item.ToLower());
        }
        public void Execute()
        {
            foreach (var type in module.Types.Where(x => !(x.Name.StartsWith("<"))))
            {
                foreach (var field in type.Fields.Where(x => !x.IsRuntimeSpecialName && !x.IsSpecialName
                && !(x.Name.StartsWith("<"))))
                {
                    if (ignoreField.FirstOrDefault(x => field.FullName.ToLower().Contains(x)) == null)
                        NameGenerator.SetObfusName(field, NameGenerator.Mode.FuncName, 3);
                }
                foreach (var method in type.Methods.Where(x => !x.IsConstructor && !x.IsVirtual
                && !x.IsRuntime && !x.IsRuntimeSpecialName && !x.IsAbstract
                && !((x.GenericParameters != null) && x.GenericParameters.Count > 0) && !(x.Overrides.Count > 0)
                && !(x.Name.StartsWith("<") || x.Name.ToLower().StartsWith("do"))))
                {
                    if (ignoreMethod.FirstOrDefault(x => method.FullName.ToLower().Contains(x)) == null)
                        NameGenerator.SetObfusName(method, NameGenerator.Mode.FuncName, 5);
                    if (method.HasParams())
                    {
                        foreach (var p in method.Parameters)
                        {
                            p.Name = NameGenerator.GetName(NameGenerator.Mode.FuncName, 4);
                        }
                    }
                }
                foreach (var p in type.Properties.Where(x => !x.IsRuntimeSpecialName && !x.IsSpecialName))
                    NameGenerator.SetObfusName(p, NameGenerator.Mode.Base64, 5);
                //NameGenerator.SetObfusName(type, NameGenerator.Mode.FuncName, 6);

                if (obfusClass.FirstOrDefault(x => type.FullName.ToLower().Contains(x)) != null)
                {
                    NameGenerator.SetObfusName(type, NameGenerator.Mode.FuncName, 6);
                }
            }
        }
    }
    public class ignore
    {
        public string[] ignoreMethod;
        public string[] ignoreField;
        public string[] custom_ignore_Method;
        public string[] custom_ignore_Field;
        public string[] custom_obfus_Class;
        public static string origin = "ewogICAgImlnbm9yZU1ldGhvZCI6IFsKICAgICAgICAiQXdha2UiLAogICAgICAgICJPbkVuYWJsZSIsCiAgICAgICAgIlN0YXJ0IiwKICAgICAgICAiRml4ZWRVcGRhdGUiLAogICAgICAgICJVcGRhdGUiLAogICAgICAgICJPbkRpc2FibGUiLAogICAgICAgICJMYXRlVXBkYXRlIiwKICAgICAgICAiUmVzZXQiLAogICAgICAgICJPblZhbGlkYXRlIiwKICAgICAgICAiRml4ZWRVcGRhdGUiLAogICAgICAgICJPblRyaWdnZXJFbnRlciIsCiAgICAgICAgIk9uVHJpZ2dlckVudGVyMkQiLAogICAgICAgICJPblRyaWdnZXJFeGl0IiwKICAgICAgICAiT25UcmlnZ2VyRXhpdDJEIiwKICAgICAgICAiT25UcmlnZ2VyU3RheTJEIiwKICAgICAgICAiT25Db2xsaXNpb25FbnRlciIsCiAgICAgICAgIk9uQ29sbGlzaW9uRW50ZXIyRCIsCiAgICAgICAgIk9uQ29sbGlzaW9uRXhpdCIsCiAgICAgICAgIk9uQ29sbGlzaW9uRXhpdDJEIiwKICAgICAgICAiT25Db2xsaXNpb25TdGF5IiwKICAgICAgICAiT25Db2xsaXNpb25TdGF5MkQiLAogICAgICAgICJPbk1vdXNlRG93biIsCiAgICAgICAgIk9uTW91c2VEcmFnIiwKICAgICAgICAiT25Nb3VzZUVudGVyIiwKICAgICAgICAiT25Nb3VzZUV4aXQiLAogICAgICAgICJPbk1vdXNlT3ZlciIsCiAgICAgICAgIk9uTW91c2VVcCIsCiAgICAgICAgIk9uTW91c2VVcEFzQnV0dG9uIiwKICAgICAgICAiT25QcmVDdWxsIiwKICAgICAgICAiT25CZWNhbWVWaXNpYmxlIiwKICAgICAgICAiT25CZWNhbWVJbnZpc2libGUiLAogICAgICAgICJPbldpbGxSZW5kZXJPYmplY3QiLAogICAgICAgICJPblByZVJlbmRlciIsCiAgICAgICAgIk9uUmVuZGVyT2JqZWN0IiwKICAgICAgICAiT25Qb3N0UmVuZGVyIiwKICAgICAgICAiT25SZW5kZXJJbWFnZSIsCiAgICAgICAgIk9uR1VJIiwKICAgICAgICAiT25EcmF3R2l6bW9zIiwKICAgICAgICAiT25EcmF3R2l6bW9zU2VsZWN0ZWQiLAogICAgICAgICJPbkFwcGxpY2F0aW9uRm9jdXMiLAogICAgICAgICJPbkFwcGxpY2F0aW9uUGF1c2UiLAogICAgICAgICJPbkFwcGxpY2F0aW9uUXVpdCIsCiAgICAgICAgIk9uRGlzYWJsZSIsCiAgICAgICAgIk9uRGVzdG9yeSIsCiAgICAgICAgIk9uTGV2ZWxXYXNMb2FkZWQiLAogICAgICAgICJPbkFuaW1hdG9ySUsiLAogICAgICAgICJPbkFuaW1hdG9yTW92ZSIsCiAgICAgICAgIk9uQXBwbGljYXRpb25Gb2N1cyIsCiAgICAgICAgIk9uQXBwbGljYXRpb25QYXVzZSIsCiAgICAgICAgIk9uQXBwbGljYXRpb25RdWl0IiwKICAgICAgICAiT25BdWRpb0ZpbHRlclJlYWQiLAogICAgICAgICJPbkJlY2FtZUludmlzaWJsZSIsCiAgICAgICAgIk9uQmVjYW1lVmlzaWJsZSIsCiAgICAgICAgIk9uQ29ubmVjdGVkVG9TZXJ2ZXIiLAogICAgICAgICJPbkNvbnRyb2xsZXJDb2xsaWRlckhpdCIsCiAgICAgICAgIk9uRW5hYmxlIiwKICAgICAgICAiT25GYWlsZWRUb0Nvbm5lY3QiLAogICAgICAgICJPbkRpc2Nvbm5lY3RlZEZyb21TZXJ2ZXIiLAogICAgICAgICJPbkRyYXdHaXptb3MiLAogICAgICAgICJPbkRyYXdHaXptb3NTZWxlY3RlZCIsCiAgICAgICAgIk9uRW5hYmxlIiwKICAgICAgICAiT25GYWlsZWRUb0Nvbm5lY3QiLAogICAgICAgICJPbkZhaWxlZFRvQ29ubmVjdFRvTWFzdGVyU2VydmVyIiwKICAgICAgICAiT25Kb2ludEJyZWFrIiwKICAgICAgICAiT25Kb2ludEJyZWFrMkQiLAogICAgICAgICJPbk1hc3RlclNlcnZlckV2ZW50IiwKICAgICAgICAiT25OZXR3b3JrSW5zdGFudGlhdGUiLAogICAgICAgICJPblBhcnRpY2xlQ29sbGlzaW9uIiwKICAgICAgICAiT25QYXJ0aWNsZVN5c3RlbVN0b3BwZWQiLAogICAgICAgICJPblBhcnRpY2xlVHJpZ2dlciIsCiAgICAgICAgIk9uUGFydGljbGVVcGRhdGVKb2JTY2hlZHVsZWQiLAogICAgICAgICJPblBsYXllckNvbm5lY3RlZCIsCiAgICAgICAgIk9uUGxheWVyRGlzY29ubmVjdGVkIiwKICAgICAgICAiT25Qb3N0UmVuZGVyIiwKICAgICAgICAiT25QcmVDdWxsIiwKICAgICAgICAiT25QcmVSZW5kZXIiLAogICAgICAgICJPblJlbmRlckltYWdlIiwKICAgICAgICAiT25SZW5kZXJPYmplY3QiLAogICAgICAgICJPblNlcmlhbGl6ZU5ldHdvcmtWaWV3IiwKICAgICAgICAiT25TZXJ2ZXJJbml0aWFsaXplZCIsCiAgICAgICAgIk9uVHJhbnNmb3JtQ2hpbGRyZW5DaGFuZ2VkIiwKICAgICAgICAiT25UcmFuc2Zvcm1QYXJlbnRDaGFuZ2VkIiwKICAgICAgICAiT25WYWxpZGF0ZSIsCiAgICAgICAgIk9uV2lsbFJlbmRlck9iamVjdCIsCiAgICAgICAgIlJlc2V0IiwKICAgICAgICAiX18iLAogICAgICAgICJpbmNvbnRyb2wiLAogICAgICAgICJzdG9wIiwKICAgICAgICAib3B0aW9uIiwKICAgICAgICAicGF1c2VtYW5hZ2VyIiwKICAgICAgICAiZmFsbGluZ3JvY2siLAogICAgICAgICJwb3N0Zml4IiwKICAgICAgICAicHJlZml4IiwKICAgICAgICAidHJhbnNwaWxlciIKICAgIF0sCiAgICAiaWdub3JlRmllbGQiOlsKICAgICAgICAiaW5jb250cm9sIiwKICAgICAgICAiZW51bSIsCiAgICAgICAgIm9wdGlvbmFsIiwKICAgICAgICAiX18iCiAgICBdLAogICAgIi8vIjoi5Lul5LiK5Li66buY6K6k5b+955Wl5YiX6KGo77yM6buY6K6k5YyF5ZCr5YWo6YOo55qEVW5pdHnlhbPplK7mlrnms5XvvIzlu7rorq7kuI3opoHkv67mlLnvvIzku6XkuIvkuLroh6rlrprkuYnlv73nlaXliJfooajvvIzlj6/ku6XmoLnmja7pnIDopoHoh6rooYzmt7vliqAo5aaC5p6c6ZyA6KaB55So5Yiw5Y+N5bCE5oiW6ICF5Yqo5oCB6LCD55So55qE5pa55rOV77yM5bu66K6u5re75Yqg5Yiw6L+Z6YeMKSkiLAoKICAgICIvLyI6IuWFs+S6juWmguS9leaYr+S9v+eUqOiHquWumuS5ieW/veeVpeWIl+ihqO+8jOi/memHjOacieWHoOeCueW7uuiuriIsCiAgICAiLy8iOiIxLuWcqFVuaXR55Lit77yMR2FtZU9iamVjdOaIluiAhXByZWZhYnPliJ3lp4vnu5HlrprkuobohJrmnKzvvIzliJnor6XohJrmnKznmoTnsbvlkI3kuI3lj6/mt7fmt4bvvIzmlrnms5XlkI3lkozlrZfmrrXlkI3lj6/ku6Xmt7fmt4YiLAogICAgIi8vIjoiMi7lnKhVbml0eeS4re+8jEdhbWVPYmplY3TmiJbogIVwcmVmYWJz5Yid5aeL5rKh5pyJ57uR5a6a6ISa5pys77yM5L2G5piv5Zyo5Luj56CB5Lit5Yqo5oCB5re75Yqg5LqG6ISa5pys77yM5YiZ6K+l6ISa5pys55qE57G75ZCN44CB5pa55rOV5ZCN5ZKM5a2X5q615ZCN6YO95Y+v5Lul5re35reGIiwKICAgICIvLyI6IjMu5aaC5p6c6K+l6ISa5pys5Lit5raJ5Y+K5Yiw5LqGVUnnmoTkuovku7blk43lupQo5aaCQnV0dG9uLk9uQ2xpY2spLOWImeivpeiEmuacrOeahOexu+WQjeWSjOivpeaWueazleWQjemDveS4jeWPr+a3t+a3hizlrZfmrrXlkI3lj6/ku6Xmt7fmt4YiLAogICAgIi8vIjoiNC5Vbml0eeeahOeUn+WRveWRqOacn+aWueazleWSjOWbnuiwg+aWueazleS4jeiDvea3t+a3hizkuIrmlrnnmoTlv73nlaXliJfooajljIXlkKvkuoblpKflpJrmlbDluLjnlKjnmoTnlJ/lkb3lkajmnJ/lkozlm57osIPmlrnms5XvvIzlpoLmnpzmnInpgZfmvI/vvIzlj6/ku6Xoh6rooYzmt7vliqAiLAogICAgIi8vIjoiNS5Vbml0eeS4reeahEludm9rZeetieeJueauiuaWueazleaJgOiwg+eUqOeahOWHveaVsOaWueazleS4jeWPr+a3t+a3hizlkIznkIbljY/nqIvnsbvnmoTmlrnms5XkuZ/kuI3lj6/mt7fmt4Ys6K+36Ieq6KGM5re75Yqg5Yiw6Ieq5a6a5LmJ5b+955Wl5YiX6KGoIiwKICAgICIvLyI6IjYu6YOo5YiG5raJ5Y+K5Y+N5bCE57G755qE5Luj56CB5LiN6IO95re35reGLOWmglN5c3RlbS5SZWZsZWN0aW9uKEdldEZpZWxkLEdldE1ldGhvZCxJbnZva2XnrYkpLOivt+iHquihjOa3u+WKoOWIsOiHquWumuS5ieW/veeVpeWIl+ihqCIsCiAgICAiLy8iOiI3Lk5hdGl2ZeWxgumHjOebtOaOpeiwg+eUqEMj5oiW6YCa6L+HVW5pdHnlhoXnva5BUEnlj5HpgIHkuovku7bliLBDI+eahOexu+WSjOaWueazleS4jeWPr+a3t+a3hijlpKflpJrmlbDlnKjnp7vliqjlubPlj7DkuK0pIiwKICAgICIvLyI6Ijgu5LiA5Lqb54m55q6K5o+S5Lu25a+55bqU55qE6ISa5pys5LiN5Y+v5re35reG77yM5L6L5aaCeEx1YeWSjOS4juS5i+e7keWumueahEMj6ISa5pysIiwKCiAgICAKICAgICIvLyI6IuWvueS6juaWueazleWQjeeahOa3t+a3humHh+eUqOeahOaYr+eZveWQjeWNleaooeW8j++8jOWNs+m7mOiupOa3t+a3hu+8jOWmguaenOS4jemcgOimgea3t+a3hu+8jOWPr+S7pea3u+WKoOWIsOi/memHjCIsCiAgICAiY3VzdG9tX2lnbm9yZV9NZXRob2QiOlsKICAgICAgICAiSW5pdGlhbGl6ZSIsIlJlZ2lzdGVyQ29tcGFzc0VsZW1lbnQiLCJVbnJlZ2lzdGVyQ29tcGFzc0VsZW1lbnQiLCJEZXRlY3RUYXJnZXQiLAogICAgICAgICJMb3N0VGFyZ2V0IiwiTG9hZFRhcmdldFNjZW5lIiwiU2V0R2FtZU9iamVjdEFjdGl2ZSIsIiIKICAgIF0sCiAgICAiLy8iOiLlr7nkuo7lrZfmrrXlkI3nmoTmt7fmt4bph4fnlKjnmoTmmK/nmb3lkI3ljZXmqKHlvI/vvIzljbPpu5jorqTmt7fmt4bvvIzlpoLmnpzkuI3pnIDopoHmt7fmt4bvvIzlj6/ku6Xmt7vliqDliLDov5nph4wiLAogICAgImN1c3RvbV9pZ25vcmVfRmllbGQiOlsKICAgIF0sCiAgICAiLy8iOiLlr7nkuo7nsbvlkI3nmoTmt7fmt4bph4fnlKjnmoTmmK/pu5HlkI3ljZXmqKHlvI/vvIzljbPpu5jorqTkuI3mt7fmt4bvvIzlpoLmnpzpnIDopoHmt7fmt4bvvIzlj6/ku6Xmt7vliqDliLDov5nph4wiLAogICAgImN1c3RvbV9vYmZ1c19DbGFzcyI6WwogICAgXQp9";
    }
}
