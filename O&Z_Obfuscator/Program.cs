using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.DotNet.Emit;
using System.Security.Cryptography;
using System;
using System.Linq;
using OZ_Obfuscator.Obfuscators;
using System.Text;

namespace OZ_Obfuscator
{
    public class OZ_Obfuscator
    {
        public ModuleDefMD ModuleDefMD;
        public ControlFlow controlFlow;
        public LocalVariables2Field localVariables2Field;
        public NumObfus numObfus;
        public StrCrypter strCrypter;
        public ObfusFunc obfusFunc;
        public Antide4dot antide4dot;
        public FuckILdasm fuckILdasm;
        public OZ_Obfuscator(ModuleDefMD module,object Config, object keyfunc = null)
        {
            ModuleDefMD = module;
            controlFlow = new ControlFlow(module, Config.GetType().GetField("ignore_ControlFlow_Method").GetValue(Config) as String[]);
            localVariables2Field = new LocalVariables2Field(module);
            numObfus = new NumObfus(module);
            strCrypter = new StrCrypter(module);
            obfusFunc = new ObfusFunc(module, (string[])keyfunc.GetType().GetField("ignoreMethod").GetValue(keyfunc), (string[])keyfunc.GetType().GetField("ignoreField").GetValue(keyfunc), (string[])keyfunc.GetType().GetField("custom_ignore_Method").GetValue(keyfunc), (string[])keyfunc.GetType().GetField("custom_ignore_Field").GetValue(keyfunc), (string[])keyfunc.GetType().GetField("custom_obfus_Class").GetValue(keyfunc));
            antide4dot = new Antide4dot(module);
            fuckILdasm = new FuckILdasm(module);
        }
    }
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
