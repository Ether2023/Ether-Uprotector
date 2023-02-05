using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Ether_Obfuscator.Obfuscators
{
    public class Antide4dot : Obfuscator
    {
        ModuleDefMD ModuleDef;
        public Antide4dot(ModuleDefMD module)
        {
            ModuleDef = module;
        }
        public void Execute()
        {
            var typedef = new TypeDefUser("", "EtherProtector", ModuleDef.CorLibTypes.GetTypeRef("System", "Attribute"));
            ModuleDef.Types.Add(typedef);
            typedef.Interfaces.Add(new InterfaceImplUser(typedef));
            typedef.Interfaces.Add(new InterfaceImplUser(ModuleDef.GlobalType));
        }
    }
}
