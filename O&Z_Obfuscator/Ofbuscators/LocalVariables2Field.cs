using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OZ_Obfus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ_Obfuscator.Ofbuscators
{
    public class LocalVariables2Field
    {
        private static Dictionary<Local, FieldDef> _convertedLocals = new Dictionary<Local, FieldDef>();
		ModuleDef module;
		public LocalVariables2Field(ModuleDef Md)
        {
            module = Md;
        }
            
		public void Execute()
        {
            foreach (var type in module.Types.Where(x => x != module.GlobalType))
            {
                foreach (var meth in type.Methods.Where(x => x.HasBody && x.Body.HasInstructions && !x.IsConstructor))
                {
                    _convertedLocals = new Dictionary<Local, FieldDef>();
                    Process(module, meth);
                }
            }
        }
		private static void Process(ModuleDef module, MethodDef meth)
		{
			meth.Body.SimplifyMacros(meth.Parameters);
			IList<Instruction> instructions = meth.Body.Instructions;
			foreach (Instruction t in instructions)
			{
				if (t.Operand is Local local)
				{
					FieldDef def;
					if (!_convertedLocals.ContainsKey(local))
					{
						def = new FieldDefUser(NameGenerator.GetObfusName(NameGenerator.Mode.FuncName,10), new FieldSig(local.Type), FieldAttributes.Public | FieldAttributes.Static);
                        module.GlobalType.Fields.Add(def);
						_convertedLocals.Add(local, def);
					}
					else
					{
						def = _convertedLocals[local];
					}
					Code? code = t.OpCode?.Code;
					if (1 == 0)
					{
					}
					OpCode opCode;
					switch (code)
					{
						case Code.Ldloc:
							opCode = OpCodes.Ldsfld;
							break;
						case Code.Ldloca:
							opCode = OpCodes.Ldsflda;
							break;
						case Code.Stloc:
							opCode = OpCodes.Stsfld;
							break;
						default:
							opCode = null;
							break;
					}
					if (1 == 0)
					{
					}
					OpCode eq = (t.OpCode = opCode);
					t.Operand = def;
				}
			}
			_convertedLocals.ToList().ForEach(delegate (KeyValuePair<Local, FieldDef> x)
			{
				meth.Body.Variables.Remove(x.Key);
			});
			_convertedLocals = new Dictionary<Local, FieldDef>();
		}
	}
}
