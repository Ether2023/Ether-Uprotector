using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.PE;
using System.Text;
using Xxtea;

namespace dnlib.test.obfuscators
{
    public class StrCrypter
    {
        public MethodDef DecryptStr;
        public ModuleDefMD moduleDef;
        public StrCrypter(ModuleDefMD ModuleDef)
        {
            moduleDef = ModuleDef;
        }
        public void Execute()
        {
            var cstype = Tools.GetRuntimeType("dnlib.test.ModuleType.StringEncoder");
            DecryptStr = cstype.FindMethod("DecryptString");
            DecryptStr.CustomAttributes.Clear();
            NameGenerator.GetObfusName(DecryptStr, NameGenerator.Mode.Base64, 2);
            DecryptStr.DeclaringType = null;
            moduleDef.GlobalType.Methods.Add(DecryptStr);
            foreach (TypeDef type in moduleDef.Types)
                foreach (MethodDef method in type.Methods)
                    if (method.HasBody && method.Body.HasInstructions)
                    {
                        ReplaceString(method);
                    }
        }
        public void ReplaceString(MethodDef method)
        {
            method.Body.SimplifyBranches();
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                string key = method.Rid.ToString();
                if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr&& method.Body.Instructions[i].Operand.ToString() != "")
                {
                    string str = method.Body.Instructions[i].Operand.ToString();
                    Console.WriteLine(str);
                    string newstr = XXTEA.EncryptToBase64String(str, key);
                    method.Body.Instructions[i].Operand = "OrangeObfuscator by oRangeSumMer"; 
                    method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldstr, newstr)); //1
                    method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, key)); //2
                    method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, moduleDef.GlobalType.FindMethod(DecryptStr.Name))); //3
                    i += 3;
                }
            }
            method.Body.OptimizeBranches();
        }
    }
}
