using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
namespace Ether_Obfuscator.Obfuscators.Resolver
{
    public class ReflectionResolver
    {
        public ReflectionSkip Reflections = new ReflectionSkip();
        HashSet<string> Reflection = new HashSet<string>();
        public ReflectionResolver(ModuleDefMD moduleDefMD) 
        {
            foreach(var type in moduleDefMD.Types)
            {
                foreach(var method in type.Methods.Where(x => x.HasBody))
                {
                    for (int index = 0; index < method.Body.Instructions.Count; index++)
                    {
                        Instruction Ins = method.Body.Instructions[index];
                        if (Ins.OpCode == OpCodes.Ldstr)
                        {
                            string str = (string)Ins.Operand;
                            if (!Reflection.Contains(str))
                            {
                                Reflection.Add(str);
                            }
                        }
                    }
                }
            }
            foreach (var type in moduleDefMD.Types)
            {
                if (Reflection.Contains(GetTypeNameWithoutGenericSuffix(type.Name)))
                {
                    Reflections.Type.Add(type.Name);
                    Reflections.Namespace.Add(type.Namespace);
                }
                foreach (var method in type.Methods)
                {
                    if(Reflection.Contains(method.Name))
                        Reflections.Method.Add(method.Name);
                }
            }
        }
        public static string GetTypeNameWithoutGenericSuffix(string Name)
        {
            if (Name.Contains("`"))
            {
                return Name.Remove(Name.LastIndexOf('`'));
            }
            return Name;
        }
    }
    public class ReflectionSkip
    {
        public List<string> Namespace = new List<string>();
        public List<string> Type = new List<string>();
        public List<string> Method = new List<string>();
    }
}
