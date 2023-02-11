using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Ether_Obfuscator.Obfuscators
{
    public class Block
    {
        //private Random random = new Random();
        private int randomIdentifier;
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();
        public int Number { get; set; }
        public int RandomIdentifier
        {
            get
            {
                if (randomIdentifier == 0)
                {
                    randomIdentifier = RandomGenerator.Generate(1, int.MaxValue);
                    if (RandomGenerator.Generate(0, 2) == 0)
                    {
                        randomIdentifier *= -1;
                    }
                    /*
                    randomIdentifier = random.Next(1, int.MaxValue);
                    if (random.Next(0, 2) == 0)
                    {
                        randomIdentifier *= -1;
                    }
                    */
                }
                return randomIdentifier;
            }
        }
    }
    public class ControlFlow : Obfuscator
    {
        public ModuleDef Module;
        //private Random random = new Random();
        List<string> IgnoreMethod = new List<string>();
        public ControlFlow(ModuleDef module, string[] ignoreMethod)
        {
            Module = module;
            if(ignoreMethod != null)
            foreach (var item in ignoreMethod)
                IgnoreMethod.Add(item);
        }
        public void Execute()
        {
            int amount = 0;

            for (int x = 0; x < Module.Types.Count; x++)
            {
                TypeDef tDef = Module.Types[x];

                for (int i = 0; i < tDef.Methods.Count; i++)
                {
                    MethodDef mDef = tDef.Methods[i];

                    if (!mDef.Name.StartsWith("get_") && !mDef.Name.StartsWith("set_"))
                    {
                        if (!mDef.HasBody || mDef.IsConstructor) continue;
                        if (IgnoreMethod.Count > 0 && IgnoreMethod.FirstOrDefault(x => mDef.FullName.Contains(x)) != null) continue;
                        mDef.Body.SimplifyBranches();
                        ObfusMethod(mDef);

                        amount++;
                    }
                }
            }
        }

        private void ObfusMethod(MethodDef Method)
        {
            Method.Body.SimplifyMacros(Method.Parameters);
            List<Block> BlockList = ParseMethod(Method);
            BlockList = Randomize(BlockList);
            Method.Body.Instructions.Clear();
            Local variable = new Local(Module.CorLibTypes.Int32);
            Method.Body.Variables.Add(variable);
            Instruction nop = Instruction.Create(OpCodes.Nop);
            Instruction br = Instruction.Create(OpCodes.Br, nop);
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, BlockList.Single((Block x) => x.Number == 0).RandomIdentifier));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, br));
            Method.Body.Instructions.Add(nop);
            foreach (Block block in BlockList)
            {
                if (block == BlockList.Single((Block x) => x.Number == BlockList.Count - 1))
                {
                    continue;
                }
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, variable));
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, block.RandomIdentifier));
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction temp = Instruction.Create(OpCodes.Nop);
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, temp));
                foreach (Instruction instruction in block.Instructions)
                {
                    Method.Body.Instructions.Add(instruction);
                }
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, BlockList.Single((Block x) => x.Number == block.Number + 1).RandomIdentifier));
                Method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
                Method.Body.Instructions.Add(temp);
            }
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, variable));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, BlockList.Single((Block x) => x.Number == BlockList.Count - 1).RandomIdentifier));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, br));
            Method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, BlockList.Single((Block x) => x.Number == BlockList.Count - 1).Instructions[0]));
            Method.Body.Instructions.Add(br);
            foreach (Instruction instruction in BlockList.Single((Block x) => x.Number == BlockList.Count - 1).Instructions)
            {
                Method.Body.Instructions.Add(instruction);
            }
            Method.Body.OptimizeMacros();
        }
        public static List<Block> ParseMethod(MethodDef Method)
        {
            bool methodHasReturnValue = Method.ReturnType.FullName != "System.Void";
            List<Block> list = new List<Block>();
            Block block = new Block();
            int num = 0;
            int stacknum = 0;
            block.Number = num;
            block.Instructions.Add(Instruction.Create(OpCodes.Nop));
            list.Add(block);
            block = new Block();
            Stack<ExceptionHandler> stack = new Stack<ExceptionHandler>();
            int pushes = 0;
            int pops = 0;
            int i = 0;
            foreach (Instruction instruction in Method.Body.Instructions)
            {
                foreach (ExceptionHandler exceptionHandler in Method.Body.ExceptionHandlers)
                {
                    if (exceptionHandler.HandlerStart == instruction || exceptionHandler.TryStart == instruction || exceptionHandler.FilterStart == instruction)
                    {
                        stack.Push(exceptionHandler);
                    }
                }
                foreach (ExceptionHandler exceptionHandler in Method.Body.ExceptionHandlers)
                {
                    if (exceptionHandler.HandlerEnd == instruction || exceptionHandler.TryEnd == instruction)
                    {
                        stack.Pop();
                    }
                }
                CalculateStackUsage(instruction, methodHasReturnValue, out pushes, out pops);
                block.Instructions.Add(instruction);
                if (instruction == Method.Body.Instructions.Last())
                {
                    num = (block.Number = num + 1);
                    list.Add(block);
                    block = new Block();
                }
                else
                {
                    stacknum += pushes - pops;
                    if (pushes == 0 && instruction.OpCode != OpCodes.Nop && (stacknum == 0 || instruction.OpCode == OpCodes.Ret) && stack.Count == 0)
                    {
                        num = (block.Number = num + 1);
                        list.Add(block);
                        block = new Block();
                    }
                }
                i++;
            }
            i = 0;
            foreach (Block _block in list)
            {
                foreach (Instruction instruction in _block.Instructions)
                {
                    i++;
                }
            }
            return list;
        }
        public static void CalculateStackUsage(Instruction instruction, bool methodHasReturnValue, out int pushes, out int pops)
        {
            OpCode opCode = instruction.OpCode;
            if (opCode.FlowControl == FlowControl.Call)
            {
                CalculateStackUsageCall(instruction, opCode.Code, out pushes, out pops);
            }
            else
            {
                CalculateStackUsageNonCall(instruction, opCode, methodHasReturnValue, out pushes, out pops);
            }
        }
        private static void CalculateStackUsageCall(Instruction instruction, Code code, out int pushes, out int pops)
        {
            pushes = 0;
            pops = 0;
            if (code == Code.Jmp)
            {
                return;
            }
            IMethod method = (IMethod)instruction.Operand;
            if (method != null)
            {
                bool flag = HasImplicitThis(method.MethodSig);
                if (!(method.MethodSig.RetType.FullName == "System.Void") || (code == Code.Newobj && method.MethodSig.HasThis))
                {
                    pushes++;
                }
                pops += method.GetParamCount();
                int parameterAfterSentinel = GetParameterAfterSentinel(method);
                if (parameterAfterSentinel > 0)
                {
                    pops += parameterAfterSentinel;
                }
                if (flag && code != Code.Newobj)
                {
                    pops++;
                }
                if (code == Code.Calli)
                {
                    pops++;
                }
            }
        }

        private static void CalculateStackUsageNonCall(Instruction instruction, OpCode opCode, bool hasReturnValue, out int pushes, out int pops)
        {
            switch (opCode.StackBehaviourPush)
            {
                case StackBehaviour.Push0:
                    pushes = 0;
                    break;
                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    pushes = 1;
                    break;
                case StackBehaviour.Push1_push1:
                    pushes = 2;
                    break;
                default:
                    pushes = 0;
                    break;
            }
            switch (opCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    pops = 0;
                    break;
                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    pops = 1;
                    break;
                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    pops = 2;
                    break;
                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    pops = 3;
                    break;
                case StackBehaviour.PopAll:
                    pops = -1;
                    break;
                case StackBehaviour.Varpop:
                    if (hasReturnValue)
                    {
                        pops = 1;
                    }
                    else
                    {
                        pops = 0;
                    }
                    break;
                default:
                    pops = 0;
                    break;
            }
        }
        public static bool HasImplicitThis(MethodSig method)
        {
            if (method.HasThis)
            {
                return !method.ExplicitThis;
            }
            return false;
        }
        private List<Block> Randomize(List<Block> input)
        {
            List<Block> list = new List<Block>();
            foreach (Block item in input)
            {
                list.Insert(RandomGenerator.Generate(0, list.Count), item);
                //list.Insert(random.Next(0, list.Count), item);
            }
            return list;
        }
        public static int GetParameterAfterSentinel(IMethod method)
        {
            if (!method.HasParams())
            {
                return -1;
            }
            IList<TypeSig> parameters = method.GetParams();
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].IsSentinel)
                {
                    return parameters.Count - 1 - i;
                }
            }
            return -1;
        }
    }
}
