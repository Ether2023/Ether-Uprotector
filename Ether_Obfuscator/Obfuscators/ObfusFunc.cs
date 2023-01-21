using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Ether_IL2CPP.LitJson;
using Ether_Obfuscator.Obfuscators.UnityMonoBehavior;
namespace Ether_Obfuscator.Obfuscators
{
    public class ObfusFunc : Obfuscator
    {
        ModuleDefMD module;
        List<string> ignoreMethod = new List<string>();
        List<string> ignoreField = new List<string>();
        List<string> obfusClass = new List<string>();
        List<MonoSwapMap> swapMaps= new List<MonoSwapMap>();
        List<string> jumpName = new List<string>();
        List<string> Mono;
        ReflectionResolver ReflectionResolver;
        public ObfusFunc(ModuleDefMD module, List<String> MonoClass = null)
        {
            ReflectionResolver = new ReflectionResolver(module);
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
                ignoreMethod.Add(item);
            foreach (var item in ig.ignoreField)
                ignoreField.Add(item);
            foreach(var item in ig.custom_ignore_Method)
                ignoreMethod.Add(item);
            foreach (var item in ig.custom_ignore_Field)
                ignoreField.Add(item);
            foreach (var item in ig.custom_obfus_Class)
                obfusClass.Add(item);
            Mono = MonoClass;
        }
        public ObfusFunc(ModuleDefMD module,string Keyfunc, List<String> MonoClass = null)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            ignore ig = JsonMapper.ToObject<ignore>(Keyfunc);
            foreach (var item in ig.ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in ig.ignoreField)
                ignoreField.Add(item);
            foreach (var item in ig.custom_ignore_Method)
                ignoreMethod.Add(item);
            foreach (var item in ig.custom_ignore_Field)
                ignoreField.Add(item);
            foreach (var item in ig.custom_obfus_Class)
                obfusClass.Add(item);
            Mono = MonoClass;
        }
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _custom_ignore_Method, string[] _custom_ignore_Field, string[] _obfusClass, List<String> MonoClass = null)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            foreach (var item in _ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in _ignoreField)
                ignoreField.Add(item);
            foreach (var item in _custom_ignore_Method)
                ignoreMethod.Add(item);
            foreach (var item in _custom_ignore_Field)
                ignoreField.Add(item);
            foreach (var item in _obfusClass)
                obfusClass.Add(item);
            Mono = MonoClass;
        }
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _custom_ignore_Method, string[] _custom_ignore_Field, string[] _obfusClass,out List<MonoSwapMap> maps ,List<String> MonoClass = null)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            maps = swapMaps;
            foreach (var item in _ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in _ignoreField)
                ignoreField.Add(item);
            foreach (var item in _custom_ignore_Method)
                ignoreMethod.Add(item);
            foreach (var item in _custom_ignore_Field)
                ignoreField.Add(item);
            foreach (var item in _obfusClass)
                obfusClass.Add(item);
            Mono = MonoClass;
        }
        public ObfusFunc(ModuleDefMD module,out List<MonoSwapMap> maps ,List<String> MonoClass = null)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            maps = swapMaps;
            if (!File.Exists("keyfunc.json"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("keyfunc.json not found!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Download new keyfunc.json From Github...");
                Console.WriteLine("If you do not want to use the default keyfunc.json, please reconfigure keyfunc.json");
                Console.ForegroundColor = ConsoleColor.White;
                File.WriteAllText("keyfunc.json", Utils.DownloadText("https://raw.githubusercontent.com/Z1029-oRangeSumMer/O-Z-Unity-Protector/main/Configs/keyfunc.json"));
                if (!File.Exists("keyfunc.json"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Download Failed!");
                    Console.WriteLine("ObfusFunc function will not work normally!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            ignore ig = JsonMapper.ToObject<ignore>(File.ReadAllText("keyfunc.json"));
            foreach (var item in ig.ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in ig.ignoreField)
                ignoreField.Add(item);
            foreach (var item in ig.custom_ignore_Method)
                ignoreMethod.Add(item);
            foreach (var item in ig.custom_ignore_Field)
                ignoreField.Add(item);
            foreach (var item in ig.custom_obfus_Class)
                obfusClass.Add(item);
            Mono = MonoClass;
        }
        public void Execute()
        {
            foreach (var type in module.Types.Where(x => !(x.Name.StartsWith("<"))))
            {
                foreach (var field in type.Fields.Where(x => !x.IsRuntimeSpecialName && !x.IsSpecialName
                && !(x.Name.StartsWith("<"))))
                {
                    if (ignoreField.FirstOrDefault(x => field.FullName.Contains(x)) == null)
                        NameGenerator.SetObfusName(field, NameGenerator.Mode.FuncName, 4);
                }
                foreach (var method in type.Methods.Where(x => !x.IsConstructor && !x.IsVirtual
                && !x.IsRuntime && !x.IsRuntimeSpecialName && !x.IsAbstract
                && !((x.GenericParameters != null) && x.GenericParameters.Count > 0) && !(x.Overrides.Count > 0)
                && !(x.Name.StartsWith("<") || x.Name.StartsWith("do"))))
                {
                    if (ignoreMethod.FirstOrDefault(x => method.FullName.Contains(x)) == null)
                        NameGenerator.SetObfusName(method, NameGenerator.Mode.FuncName, 5);
                    if (method.HasParams())
                    {
                        foreach (var p in method.Parameters)
                        {
                            p.Name = NameGenerator.GetName(NameGenerator.Mode.FuncName, 3);
                        }
                    }
                }
                foreach (var p in type.Properties.Where(x => !x.IsRuntimeSpecialName && !x.IsSpecialName))
                    NameGenerator.SetObfusName(p, NameGenerator.Mode.RandomString, 4);
                if (Mono != null && obfusClass.FirstOrDefault(x => type.FullName.Contains(x)) == null && !type.IsGlobalModuleType 
                    && Mono.Contains(type.Name) && !type.Name.Contains("`") && !type.IsAbstract)
                {
                    if (MonoUtils.MonoTypeCheck(type))
                    {
                        string TempName;
                        do
                        {
                            NameGenerator.SetObfusName(type, NameGenerator.Mode.RandomString, out TempName, 1, 5);
                        } while (jumpName.Contains(type.Name));
                        jumpName.Add(type.Name);
                        swapMaps.Add(new MonoSwapMap
                        {
                            OriginName = TempName,
                            ObfusName = type.Name,
                            Set = false
                        });
                    }
                }
                /*
                if (obfusClass.FirstOrDefault(x => type.FullName.Contains(x)) != null)
                {
                    NameGenerator.SetObfusName(type, NameGenerator.Mode.FuncName, 4);
                }
                */
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
