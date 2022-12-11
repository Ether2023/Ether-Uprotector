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
    public class Block
    {
        public Block()
        {
            Instructions = new List<Instruction>();
        }
        public List<Instruction> Instructions { get; set; }

        public int Number { get; set; }
        public int Next { get; set; }
    }
    public class ControlFlow
    {
        public ModuleDef Module;
        List<string> IgnoreMethod = new List<string>();
        public ControlFlow(ModuleDef module, string[] ignoreMethod)
        {
            Module = module;
            foreach (var item in ignoreMethod)
                IgnoreMethod.Add(item.ToLower());
        }
        public void Execute()
        {
            for (int i = 0; i < Module.Types.Count; i++)
            {
                var tDef = Module.Types[i];
                if (tDef != Module.GlobalType)
                    for (int j = 0; j < tDef.Methods.Count; j++)
                    {
                        var mDef = tDef.Methods[j];
                        if (!mDef.Name.StartsWith("get_") && !mDef.Name.StartsWith("set_"))
                        {
                            if (!mDef.HasBody || mDef.IsConstructor) continue;
                            if (IgnoreMethod.FirstOrDefault(x => mDef.FullName.ToLower().Contains(x)) != null) continue;
                            mDef.Body.SimplifyBranches();
                            ObfusMethod(mDef);
                        }
                    }
            }
        }
        public void ObfusMethod(MethodDef method)
        {
            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Parse(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.Int32);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calculation(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (Block block in blocks)
            {
                if (block != blocks.Single(x => x.Number == blocks.Count - 1))
                {
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                    foreach (Instruction instruction in Calculation(block.Number))
                        method.Body.Instructions.Add(instruction);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                    Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                    foreach (Instruction instruction in block.Instructions)
                        method.Body.Instructions.Add(instruction);
                    foreach (Instruction instruction in Calculation(block.Number + 1))
                        method.Body.Instructions.Add(instruction);

                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                    method.Body.Instructions.Add(instruction4);
                }
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calculation(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static List<Block> Parse(MethodDef method)
        {
            List<Block> blocks = new List<Block>();
            List<Instruction> body = new List<Instruction>(method.Body.Instructions);
            Block block = new Block();
            int Id = 0;
            int usage = 0;
            block.Number = Id;
            block.Instructions.Add(Instruction.Create(OpCodes.Nop));
            blocks.Add(block);
            block = new Block();
            Stack<ExceptionHandler> handlers = new Stack<ExceptionHandler>();
            foreach (Instruction instruction in method.Body.Instructions)
            {
                foreach (var eh in method.Body.ExceptionHandlers)
                {
                    if (eh.HandlerStart == instruction || eh.TryStart == instruction || eh.FilterStart == instruction)
                        handlers.Push(eh);
                }
                foreach (var eh in method.Body.ExceptionHandlers)
                {
                    if (eh.HandlerEnd == instruction || eh.TryEnd == instruction)
                        handlers.Pop();
                }
                int stacks, pops;
                instruction.CalculateStackUsage(out stacks, out pops);
                block.Instructions.Add(instruction);
                usage += stacks - pops;
                if (stacks == 0)
                {
                    if (instruction.OpCode != OpCodes.Nop)
                    {
                        if ((usage == 0 || instruction.OpCode == OpCodes.Ret) && handlers.Count == 0)
                        {

                            block.Number = ++Id;
                            blocks.Add(block);
                            block = new Block();
                        }
                    }
                }
            }

            return blocks;
        }
        public List<Block> Randomize(List<Block> input)
        {
            List<Block> ret = new List<Block>();
            foreach (var group in input)
                ret.Insert(RandomGenerator.Generate(0, ret.Count), group);
            return ret;
        }


        public List<Instruction> Calculation(int value)
        {
            List<Instruction> instructions = new List<Instruction>();
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value));
            return instructions;
        }

        public void obfusJMP(IList<Instruction> instrs, Instruction target)
        {
            instrs.Add(Instruction.Create(OpCodes.Br, target));
        }
    }
}
