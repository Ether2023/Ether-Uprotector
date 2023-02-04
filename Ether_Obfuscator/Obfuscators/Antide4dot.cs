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
            var antitype = Utils.GetRuntimeTypeSelf("Ether_Obfuscator.Runtime.AntiDe4dotAttr");
            antitype.DeclaringType = null;
            antitype.Namespace = "";
            ModuleDef.Types.Add(antitype);
            var antitype1 = ModuleDef.FindNormal("AntiDe4dotAttr");
            var jmpmethod = antitype.FindMethod("NULL");
            jmpmethod.DeclaringType = null;
            var jmpdest = jmpmethod.Body.Instructions[0];

            jmpmethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, ModuleDef.Import(typeof(System.Reflection.Assembly).GetMethod("GetExecutingAssembly", new Type[] { }))));
            jmpmethod.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Call, ModuleDef.Import(typeof(System.Reflection.Assembly).GetMethod("GetCallingAssembly", new Type[] { }))));
            jmpmethod.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Call, ModuleDef.Import(typeof(System.Reflection.Assembly).GetMethod("op_Inequality", new Type[] { typeof(System.Reflection.Assembly), typeof(System.Reflection.Assembly) }))));
            jmpmethod.Body.Instructions.Insert(3, new Instruction(OpCodes.Brfalse_S, jmpdest));
            jmpmethod.Body.Instructions.Insert(4, Instruction.Create(OpCodes.Call, ModuleDef.Import(typeof(System.Diagnostics.Process).GetMethod("GetCurrentProcess", new Type[] { }))));
            jmpmethod.Body.Instructions.Insert(5, Instruction.Create(OpCodes.Callvirt, ModuleDef.Import(typeof(System.Diagnostics.Process).GetMethod("Kill", new Type[] { }))));
            jmpmethod.Body.Instructions.Insert(6, Instruction.Create(OpCodes.Ret));
            antitype1.Methods.Add(jmpmethod);
        }
    }
}
