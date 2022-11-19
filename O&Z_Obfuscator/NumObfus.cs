using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Collections.Generic;
using System.Linq;

namespace dnlib.test
{
    public class NumObfus
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
            var cstype = Tools.GetRuntimeType("dnlib.test.ModuleType.Num2Modle");
            FieldDef field = cstype.FindField("NUM");
            NameGenerator.SetNewName(field, NameGenerator.RenameMode.Base64, 2);
            field.DeclaringType = null;
            Module.GlobalType.Fields.Add(field);

            var method = Module.GlobalType.FindOrCreateStaticConstructor();
            method.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4, num));
            method.Body.Instructions.Insert(1, new Instruction(OpCodes.Stsfld, field));
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
                        var val = instr.GetLdcI4Value();
                        FieldDef fld;
                        if (!Numbers.TryGetValue(val, out fld))
                        {
                            fld = AddNumberField(val);
                            Numbers.Add(val, fld);
                        }
                        instr.OpCode = OpCodes.Ldsfld;
                        instr.Operand = fld;
                    }
                }
            }
        }
    }
}
