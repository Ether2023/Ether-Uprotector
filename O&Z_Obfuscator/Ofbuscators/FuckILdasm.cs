using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace OZ_Obfuscator.Ofbuscators
{
    public class FuckILdasm
    {
        ModuleDefMD module;
        public FuckILdasm(ModuleDefMD module)
        {
            this.module = module;
        }
        public void Execute()
        {
            AddAttr(module, typeof(SuppressIldasmAttribute).Namespace, nameof(SuppressIldasmAttribute));
        }
        //Thank to BitMono and sunnamed434
        public CustomAttribute AddAttr(ModuleDefMD moduleDefMD, string @namespace, string @name)
        {
            var attributeRef = moduleDefMD.CorLibTypes.GetTypeRef(@namespace, @name);
            var attributeCtor = new MemberRefUser(moduleDefMD, ".ctor", MethodSig.CreateInstance(moduleDefMD.CorLibTypes.Void), attributeRef);
            var customAttribute = new CustomAttribute(attributeCtor);
            moduleDefMD.CustomAttributes.Add(customAttribute);
            return customAttribute;
        }
    }
}
