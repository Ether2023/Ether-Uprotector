using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.DotNet.Emit;
using System.Security.Cryptography;
using OZ_Obfus.obfuscators;
using System;
using System.Linq;
using OZ_Obfuscator.Ofbuscators;

namespace OZ_Obfus
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyLoader loader = new AssemblyLoader("C:\\Users\\22864\\Desktop\\2019Testbuild\\O&Z_2019_4_32_f1_Data\\Managed\\Assembly-CSharp.bak.dll");
            //AssemblyLoader loader = new AssemblyLoader("Y:\\Code\\cs\\TestLibrary1\\bin\\Debug\\net6.0\\TestLibrary1.dll");
            //printfinstr(loader.Module);
            //CreateMethod(loader.Module);
            //NumObfus obfus = new NumObfus(loader.Module);
            //obfus.Execute();
            StrCrypter obfus = new StrCrypter(loader.Module);
            obfus.Execute();
            ControlFlow obfus2 = new ControlFlow(loader.Module);
            obfus2.Execute();
            loader.Save();
            //Console.WriteLine(dnlib.test.ModuleType.StringEncoder.DecryptString("123","oMgi2ofCqVjZ8w/8y1R87w==", "123456"));

            //int a, b;
            //GetXor(100, out a, out b);
            //Console.WriteLine(100 + "=" + a + "^" + b + "=" + (a ^ b));
        }
        void printfinstr(ModuleDefMD Module)
        {
            foreach (var type in Module.Types.Where(x => x != Module.GlobalType))
                foreach (var method in type.Methods.Where(x => !x.IsConstructor && x.HasBody && x.Body.HasInstructions))
                {
                    Console.WriteLine(method.FullName);
                    foreach (var instr in method.Body.Instructions)
                    {
                        Console.WriteLine(instr.ToString());

                    }
                    Console.WriteLine("\n");
                }
        }
        /*
        void GetXor(int input, out int output1, out int output2)
        {
            int a = RandomNumberGenerator.GetInt32(int.MaxValue);
            int b = RandomNumberGenerator.GetInt32(int.MaxValue);
            int c = input ^ a ^ b;
            int d = b ^ a;
            output1 = c;
            output2 = d;
        }
        */
        void CreateMethod(ModuleDefMD Module)
        {
            MethodDefUser method = new MethodDefUser("OrangeHacked", new MethodSig(CallingConvention.Default, 0, Module.CorLibTypes.Void), MethodAttributes.Public | MethodAttributes.Static);
            method.Body = new CilBody();
            Local lcl = new Local(Module.CorLibTypes.Int32);
            Local lcl2 = new Local(Module.CorLibTypes.String);
            method.Body.Variables.Add(lcl);
            method.Body.Variables.Add(lcl2);
            method.Body.Instructions.Add(new Instruction(OpCodes.Ldc_I4, 100));
            method.Body.Instructions.Add(new Instruction(OpCodes.Stloc, lcl));
            method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "OrangeHacked! by dnlib"));
            method.Body.Instructions.Add(new Instruction(OpCodes.Stloc, lcl2));
            method.Body.Instructions.Add(new Instruction(OpCodes.Ldloc, lcl2));
            method.Body.Instructions.Add(new Instruction(OpCodes.Call, Module.Import(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))));
            method.Body.Instructions.Add(new Instruction(OpCodes.Nop));
            method.Body.Instructions.Add(new Instruction(OpCodes.Ret));
            Module.Types[1].Methods.Add(method);
        }
    }
}
