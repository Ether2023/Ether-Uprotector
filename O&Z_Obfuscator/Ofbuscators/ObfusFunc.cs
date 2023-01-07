using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using O_Z_IL2CPP_Security.LitJson;

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
                Console.WriteLine("Download new keyfunc.json From Github...");
                Console.WriteLine("If you do not want to use the default keyfunc.json, please reconfigure keyfunc.json");
                Console.ForegroundColor = ConsoleColor.White;
                File.WriteAllText("keyfunc.json", Utils.DownloadText("https://raw.githubusercontent.com/Z1029-oRangeSumMer/O-Z-Unity-Protector/main/Configs/keyfunc.json"));
                if(!File.Exists("keyfunc.json"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Download Failed!");
                    Console.WriteLine("ObfusFunc function will not work normally!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
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
        }
}
