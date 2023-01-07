using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Linq;

namespace OZ_Obfuscator.Obfuscators
{
    public class MethodError : Obfuscator
    {
        //Thank to BitMono and sunnamed434
        public ModuleDef Module;
        public MethodError(ModuleDef module) {
            Module = module;
        }
        public void Execute()
        {
            foreach (TypeDef type in Module.Types.Where(x => x.HasMethods))
            {
                    foreach (MethodDef method in type.Methods.Where(x=>x.HasBody))
                    {
                            Random random = new Random();
                            int randomValueForInsturction = 0;
                            if (method.Body.Instructions.Count >= 3)
                            {
                                randomValueForInsturction = random.Next(0, method.Body.Instructions.Count - 3);
                            }

                            // It could not work sometimes (may will harm to your app) (be careful with that, do some checks before)
                            // e.g try to ignore async methods if you have
                            int randomValue = random.Next(0, 3);
                            Instruction randomlySelectedInstruction = new Instruction();
                            randomlySelectedInstruction.OpCode = randomValue switch
                            {
                                0 => OpCodes.Readonly,
                                1 => OpCodes.Unaligned,
                                2 => OpCodes.Volatile,
                                3 => OpCodes.Constrained,
                                _ => throw new ArgumentOutOfRangeException(),
                            };

                            // Probably bit methods in some decompilers (e.g bit methods on old dnspy versions and a bit ilspy)
                            // this bit wont work on dnspyEx and dotpeek also
                            method.Body.Instructions.Insert(randomValueForInsturction, Instruction.Create(OpCodes.Br_S, method.Body.Instructions[randomValueForInsturction]));
                            method.Body.Instructions.Insert(randomValueForInsturction + 1, randomlySelectedInstruction);
                        
                    }
                
            }
        }
    }
}
