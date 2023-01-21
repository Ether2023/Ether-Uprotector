using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Ether_Obfuscator.Obfuscators
{
    //Thank to BitMono and sunnamed434
    public class Antide4dot : Obfuscator
    {
        ModuleDefMD ModuleDef;
        public Antide4dot(ModuleDefMD module)
        {
            ModuleDef = module;
        }
        public void Execute()
        {
            AddAttrWtihout(ModuleDef, "SmartAssembly.Attributes", "PoweredBy", string.Empty);
            AddAttrWtihout(ModuleDef, "Xenocode.Client.Attributes.AssemblyAttributes", "PoweredBy", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "ObfuscatedByGoliath", string.Empty);
            AddAttrWtihout(ModuleDef, "SecureTeam.Attributes", "ObfuscatedByAgileDotNet", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "TrinityObfuscator", string.Empty);
            AddAttrWtihout(ModuleDef, "SecureTeam.Attributes", "ObfuscatedByCliSecure", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "ZYXDNGuarder", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "BabelObfuscator", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "BabelObfuscator", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "Dotfuscator", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "Centos", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "ConfusedBy", string.Empty);
            AddAttrWtihout(ModuleDef, "NineRays.Obfuscator", "Evaluation", string.Empty);
            AddAttrWtihout(ModuleDef, "CryptoObfuscator", "ProtectedWithCryptoObfuscator", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "();\u0009", string.Empty);
            AddAttrWtihout(ModuleDef, string.Empty, "EMyPID_8234_", string.Empty);
        }
        public CustomAttribute AddAttrWtihout(ModuleDefMD moduleDefMD, string _Namespace, string _Name, string text)
        {
            var attributeRef = moduleDefMD.CorLibTypes.GetTypeRef(_Namespace, _Name);
            var attributeCtor = new MemberRefUser(moduleDefMD, ".ctor", MethodSig.CreateInstance(moduleDefMD.CorLibTypes.Void, moduleDefMD.CorLibTypes.String), attributeRef);
            var customAttribute = new CustomAttribute(attributeCtor);
            customAttribute.ConstructorArguments.Add(new CAArgument(moduleDefMD.CorLibTypes.String, text));
            moduleDefMD.CustomAttributes.Add(customAttribute);
            return customAttribute;
        }
    }
}
