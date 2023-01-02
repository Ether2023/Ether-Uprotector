using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OZ_Obfuscator;
using System.Collections.Generic;
using System.Linq;

namespace OZ_Obfuscator.Obfuscators
{
    public class NumObfus : Obfuscator
    {
        public ModuleDef Module;
        public Dictionary<int, FieldDef> Numbers;
        public NumObfus(ModuleDefMD moduleDef)
        {
            Module = moduleDef;
            Numbers = new Dictionary<int, FieldDef>();
        }
        public void Execute()
        {
            Numbers = new Dictionary<int, FieldDef>();
            foreach (var type in Module.Types.Where(x => x != Module.GlobalType))
                foreach (var method in type.Methods.Where(x => !x.IsConstructor && x.HasBody && x.Body.HasInstructions))
                    ObfusMethod(method);
        }
        public FieldDef AddNumberField(int num)
        {
            var cstype = Tools.GetRuntimeTypeSelf("OZ_Obfus.Rumtime.Num2Modle");
            FieldDef field = cstype.FindField("NUM");
            NameGenerator.SetObfusName(field, NameGenerator.Mode.Base64, 2);
            field.DeclaringType = null;
            Module.GlobalType.Fields.Add(field);
            int tmp1, tmp2;
            RandomGenerator.GetXor(num, out tmp1, out tmp2);
            var method = Module.GlobalType.FindOrCreateStaticConstructor();

            method.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4, tmp1));
            method.Body.Instructions.Insert(1, new Instruction(OpCodes.Ldc_I4, tmp2));
            method.Body.Instructions.Insert(2, new Instruction(OpCodes.Xor));
            method.Body.Instructions.Insert(3, new Instruction(OpCodes.Stsfld, field));//field = tmp1 ^ tmp2

            return field;
        }
        public void ObfusMethod(MethodDef method)
        {
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var instr = method.Body.Instructions[i];
                if (instr.IsLdcI4())
                {
                    if (Module.GlobalType.Fields.Count < 65000)
                    {
                        var Value = instr.GetLdcI4Value();
                        if (Value == 0 || Value == 1)
                            continue;
                        FieldDef fld;
                        if (!Numbers.TryGetValue(Value, out fld))
                        {
                            fld = AddNumberField(Value);
                            Numbers.Add(Value, fld);
                        }
                        instr.OpCode = OpCodes.Ldsfld;
                        instr.Operand = fld;
                    }
                }
            }
        }
        
    }
}
