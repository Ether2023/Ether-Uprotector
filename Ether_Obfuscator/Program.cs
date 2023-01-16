using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.DotNet.Emit;
using System.Security.Cryptography;
using System;
using System.Linq;
using Ether_Obfuscator.Obfuscators;
using System.Text;

namespace Ether_Obfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyLoader loader = new AssemblyLoader("C:\\Users\\22864\\Desktop\\2019Testbuild\\O&Z_2019_4_32_f1_Data\\Managed\\Assembly-CSharp - 副本.dll");
            Call2Calli c2cil = new Call2Calli(loader.Module);
            c2cil.Execute();
            loader.Save();
            //Console.ReadKey();
            /*
            AssemblyLoader loader;
            if (args.Length > 0)
            {
                loader = new AssemblyLoader(args[0]);

                for (int i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--ControlFlow":
                            {
                                ControlFlow controlFlow = new ControlFlow(loader.Module);
                                controlFlow.Execute();
                            }break;
                        case "--NumObfus":
                            {
                                NumObfus numObfus = new NumObfus(loader.Module);
                                numObfus.Execute();
                            }break;
                        case "--LocalVariables2Field":
                            {
                                LocalVariables2Field localVariables2Field = new LocalVariables2Field(loader.Module);
                                localVariables2Field.Execute();
                            }break;
                        case "--StrCrypter":
                            {
                                StrCrypter strCrypter = new StrCrypter(loader.Module);
                                strCrypter.Execute();
                            }break;
                        default:continue;
                    }
                }
                loader.Save();
            }
            else
            {
                Console.WriteLine("O&Z Mono Obfuscator");
                Console.WriteLine("Usage: OZ_Obfus.exe <file> [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("--ControlFlow");
                Console.WriteLine("--NumObfus");
                Console.WriteLine("--LocalVariables2Field");
                Console.WriteLine("--StrCrypter");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            */
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
        void CreateMethod(ModuleDefMD Module)
        {
            MethodDefUser method = new MethodDefUser("OrangeHacked", new MethodSig(CallingConvention.Default, 0, Module.CorLibTypes.Void), dnlib.DotNet.MethodAttributes.Public | dnlib.DotNet.MethodAttributes.Static);
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
