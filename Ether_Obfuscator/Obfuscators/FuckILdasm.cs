using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Ether_Obfuscator.Obfuscators
{
    public class FuckILdasm : Obfuscator
    {
        ModuleDefMD module;
        public FuckILdasm(ModuleDefMD module)
        {
            this.module = module;
        }
        public void Execute()
        {
            module.CustomAttributes.Add(new CustomAttribute(new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), module.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressIldasmAttribute"))));
        }
    }
}
