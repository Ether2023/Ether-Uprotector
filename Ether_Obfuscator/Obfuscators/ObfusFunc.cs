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
        bool ObfusType;
        ModuleDefMD module;
        List<string> ignoreMethod = new List<string>();
        List<string> ignoreField = new List<string>();
        List<string> ignoreClass = new List<string>();
        Dictionary<string, string> swapMaps= new Dictionary<string, string>();
        List<string> jumpName = new List<string>();
        List<string> Mono;
        ReflectionResolver ReflectionResolver;
        public ObfusFunc(ModuleDefMD module,string Keyfunc, List<String> MonoClass = null, bool ObufsType = true)
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
                ignoreClass.Add(item);
            Mono = MonoClass;
            ObfusType = ObufsType;
        }
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _ignoreClass, List<String> MonoClass = null, bool ObufsType = true)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            foreach (var item in _ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in _ignoreField)
                ignoreField.Add(item);
            foreach (var item in _ignoreClass)
                ignoreClass.Add(item);
            Mono = MonoClass;
            ObfusType = ObufsType;
        }
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _ignoreClass,out Dictionary<string,string> Map ,List<String> MonoClass = null,bool ObufsType = true)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            Map = swapMaps;
            foreach (var item in _ignoreMethod)
                ignoreMethod.Add(item);
            foreach (var item in _ignoreField)
                ignoreField.Add(item);
            foreach (var item in _ignoreClass)
                ignoreClass.Add(item);
            Mono = MonoClass;
            ObfusType = ObufsType;
        }
        public ObfusFunc(ModuleDefMD module, Dictionary<string, string> Map, List<String> MonoClass = null, bool ObufsType = true)
        {
            ReflectionResolver = new ReflectionResolver(module);
            this.module = module;
            Map = swapMaps;
            if (!File.Exists("keyfunc.json"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("keyfunc.json not found!");
                throw (new Exception("keyfunc.json not found!"));
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
                ignoreClass.Add(item);
            Mono = MonoClass;
            ObfusType = ObufsType;
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

                if(ObfusType && ignoreClass.FirstOrDefault(x => type.FullName.Contains(x)) == null && !type.IsGlobalModuleType && !type.Name.Contains("`") && !type.IsAbstract && Mono == null)
                {
                    if (MonoUtils.IsMonoBehaviour(type))
                    {
                        string TempName;
                        do
                        {
                            NameGenerator.SetObfusName(type, NameGenerator.Mode.RandomString, out TempName, 1, 5);
                        } while (jumpName.Contains(type.Name));
                        jumpName.Add(type.Name);
                        swapMaps.Add(TempName, type.Name);
                    }
                }
                else if (ObfusType && ignoreClass.FirstOrDefault(x => type.FullName.Contains(x)) == null && !type.IsGlobalModuleType && !type.Name.Contains("`") && !type.IsAbstract && Mono != null)
                {
                    if(Mono.Contains(type.Name) && MonoUtils.IsMonoBehaviour(type))
                    {
                        string TempName;
                        do
                        {
                            NameGenerator.SetObfusName(type, NameGenerator.Mode.RandomString, out TempName, 1, 5);
                        } while (jumpName.Contains(type.Name));
                        jumpName.Add(type.Name);
                        swapMaps.Add(TempName, type.Name);
                    }
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
