using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Ether_IL2CPP.LitJson;
using Ether_Obfuscator.Obfuscators.Unity;
using Ether_Obfuscator.Obfuscators.Resolver;
using System.Reflection;

namespace Ether_Obfuscator.Obfuscators
{
    public class ObfusFunc : Obfuscator
    {
        bool ObfusType;
        ModuleDefMD module;
        List<string> ignoreMethod = new List<string>();
        List<string> ignoreField = new List<string>();
        List<string> ignoreClass = new List<string>();
        Dictionary<TypeKey, TypeKey> swapMaps= new Dictionary<TypeKey, TypeKey>();
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
        public ObfusFunc(ModuleDefMD module, string[] _ignoreMethod, string[] _ignoreField, string[] _ignoreClass,out Dictionary<TypeKey, TypeKey> Map ,List<String> MonoClass = null,bool ObufsType = true)
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
        public ObfusFunc(ModuleDefMD module, Dictionary<TypeKey, TypeKey> Map, List<String> MonoClass = null, bool ObufsType = true)
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
                && !(x.Name.StartsWith("<") || x.Name.StartsWith("do") && !x.IsSpecialName)))
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
                        TypeKey temptype = new TypeKey(type);
                        do
                        {
                            NameGenerator.SetObfusName(type, NameGenerator.Mode.RandomString, out TempName, 1, 5);
                        } while (jumpName.Contains(type.Name));
                        jumpName.Add(type.Name);
                        swapMaps.Add(temptype, new TypeKey(type));
                    }
                }
                else if (ObfusType && ignoreClass.FirstOrDefault(x => type.FullName.Contains(x)) == null && !type.IsGlobalModuleType && !type.Name.Contains("`") && !type.IsAbstract && Mono != null)
                {
                    if(Mono.Contains(type.Name) && MonoUtils.IsMonoBehaviour(type))
                    {
                        string TempName;
                        TypeKey temptype = new TypeKey(type);
                        do
                        {
                            NameGenerator.SetObfusName(type, NameGenerator.Mode.RandomString, out TempName, 1, 5);
                        } while (jumpName.Contains(type.Name));
                        jumpName.Add(type.Name);
                        swapMaps.Add(temptype, new TypeKey(type));
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
    public class TypeKey
    {
        public string Assembly { get; private set; }
        public string FullName { get; private set; }
        public string Namespace { get; private set; }
        public string Name { get; private set; }
        private TypeKey()
        {
        }
        public TypeKey(TypeDef type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("TypeDefinition");
            }
            Assembly = type.Module.Assembly.Name;
            FullName = type.FullName;
            Namespace = type.Namespace;
            Name = type.Name;
        }
        public TypeKey(Type _Type)
        {
            if (_Type == null)
            {
                throw new ArgumentNullException("_Type");
            }
            Assembly = _Type.Assembly.GetName().Name;
            FullName = _Type.FullName.Replace("+", "/");
            Namespace = _Type.Namespace;
            Name = _Type.Name;
        }
        public TypeKey(string _Assembly, string _Namespace, string _Name)
        {
            if (string.IsNullOrEmpty(_Assembly))
            {
                throw new ArgumentNullException("_Assembly");
            }
            if (string.IsNullOrEmpty(_Name))
            {
                throw new ArgumentNullException("_Name");
            }
            Assembly = _Assembly;
            FullName = (string.IsNullOrEmpty(_Namespace) ? _Name : (_Namespace + "." + _Name));
            Namespace = _Namespace;
            Name = _Name;
        }
        public override bool Equals(object _Object)
        {
            return Equals(_Object as TypeKey);
        }

        public bool Equals(TypeKey _TypeKey)
        {
            if (_TypeKey != null && Assembly.Equals(_TypeKey.Assembly))
            {
                return FullName.Equals(_TypeKey.FullName);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Assembly.GetHashCode() ^ FullName.GetHashCode();
        }
        public static bool operator ==(TypeKey _A, TypeKey _B)
        {
            if ((object)_A == _B)
            {
                return true;
            }
            if ((object)_A == null)
            {
                return false;
            }
            if ((object)_B == null)
            {
                return false;
            }
            return _A.Equals(_B);
        }

        public static bool operator !=(TypeKey _A, TypeKey _B)
        {
            return !(_A == _B);
        }
        public override string ToString()
        {
            StringBuilder var_Builder = new StringBuilder();
            var_Builder.Append("[");
            var_Builder.Append(Assembly);
            var_Builder.Append("]");
            var_Builder.Append(" ");
            var_Builder.Append(FullName);
            return var_Builder.ToString();
        }
    }
}
