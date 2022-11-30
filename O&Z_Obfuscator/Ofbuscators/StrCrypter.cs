using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.PE;
using System.Text;
using System;
namespace OZ_Obfus.obfuscators
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
            //var cstype = Tools.GetRuntimeType("dnlib.test.ModuleType.StringEncoder");
            //OZ_Obfus.Rumtime.StringEncoder stringEncoder = new OZ_Obfus.Rumtime.StringEncoder();
            TypeDef cstype =  Tools.GetRuntimeTypeSelf("OZ_Obfus.Rumtime.StringEncoder");
            DecryptStr = cstype.FindMethod("DecryptString");
            NameGenerator.GetObfusName(DecryptStr, NameGenerator.Mode.Invalid, 5);
            DecryptStr.DeclaringType = null;
            //DecryptStr.CustomAttributes.Clear();
            moduleDef.GlobalType.Methods.Add(DecryptStr);
            //moduleDef.GlobalType.Methods.Add(methoddefUser);
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
                    string newstr = EncryptToBase64String(str, key);
                    method.Body.Instructions[i].Operand = "OrangeObfuscator by oRangeSumMer"; 
                    method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldstr, newstr)); //1
                    method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, key)); //2
                    method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, moduleDef.GlobalType.FindMethod(DecryptStr.Name))); //3
                    i += 3;
                }
            }
            method.Body.OptimizeBranches();
        }
        private static uint MX(uint sum, uint y, uint z, int p, uint e, uint[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }
        public static string EncryptToBase64String(string data, string key)
        {
            return Convert.ToBase64String(Encrypt(data, key));
        }
        private static byte[] Encrypt(string data, string key)
        {
            return Encrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(key));
        }
        private static byte[] Encrypt(byte[] data, byte[] key)
        {
            if (data.Length == 0)
            {
                return data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(FixKey(key), false)), false);
        }
        private static uint[] Encrypt(uint[] v, uint[] k)
        {
            int n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            uint z = v[n], y, sum = 0, e;
            int p, q = 6 + 52 / (n + 1);
            unchecked
            {
                while (0 < q--)
                {
                    sum += 0x9E3779B9;
                    e = sum >> 2 & 3;
                    for (p = 0; p < n; p++)
                    {
                        y = v[p + 1];
                        z = v[p] += MX(sum, y, z, p, e, k);
                    }
                    y = v[0];
                    z = v[n] += MX(sum, y, z, p, e, k);
                }
            }
            return v;
        }
        private static byte[] ToByteArray(uint[] data, bool includeLength)
        {
            int n = data.Length << 2;
            if (includeLength)
            {
                int m = (int)data[data.Length - 1];
                n -= 4;
                if (m < n - 3 || m > n)
                {
                    return null;
                }
                n = m;
            }
            byte[] result = new byte[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = (byte)(data[i >> 2] >> ((i & 3) << 3));
            }
            return result;
        }
        private static uint[] ToUInt32Array(byte[] data, bool includeLength)
        {
            int length = data.Length;
            int n = (length & 3) == 0 ? length >> 2 : (length >> 2) + 1;
            uint[] result;
            if (includeLength)
            {
                result = new uint[n + 1];
                result[n] = (uint)length;
            }
            else
            {
                result = new uint[n];
            }
            for (int i = 0; i < length; i++)
            {
                result[i >> 2] |= (uint)data[i] << ((i & 3) << 3);
            }
            return result;
        }
        private static byte[] FixKey(byte[] key)
        {
            if (key.Length == 16) return key;
            byte[] fixedkey = new byte[16];
            if (key.Length < 16)
            {
                key.CopyTo(fixedkey, 0);
            }
            else
            {
                Array.Copy(key, 0, fixedkey, 0, 16);
            }
            return fixedkey;
        }
    }
}
