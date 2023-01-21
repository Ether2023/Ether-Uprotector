using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
namespace Ether_Obfuscator.Obfuscators
{
    //Thank to BitMono and sunnamed434
    //Thank to MindLated
    [Obsolete]
    public class Call2Calli : Obfuscator
    {
        ModuleDefMD ModuleDef;
        Importer importer;
        public Call2Calli(ModuleDefMD module)
        {
            ModuleDef = module;
            importer = new Importer(module);
        }
        public void Execute()
        {
            ITypeDefOrRef hRuntimeMethod = importer.Import(typeof(RuntimeMethodHandle));
            IMethod _GetTypeFromHandle = importer.Import(typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle), new Type[] { typeof(RuntimeTypeHandle) }));
            IMethod _GetMethod = importer.Import(typeof(Type).GetProperty(nameof(Type.Module)).GetMethod);
            IMethod _ResolveMethod = importer.Import(typeof(Module).GetMethod(nameof(Module.ResolveMethod), new Type[] { typeof(int) }));
            IMethod _GetMethodHandle = importer.Import(typeof(MethodBase).GetProperty(nameof(MethodBase.MethodHandle)).GetMethod);
            IMethod _FuncPointer = importer.Import(typeof(RuntimeMethodHandle).GetMethod(nameof(RuntimeMethodHandle.GetFunctionPointer)));
            foreach (var type in ModuleDef.Types.Where(x => x != ModuleDef.GlobalType))
            {
                foreach (var methodDef in type.Methods.Where(x => (x.HasBody && x.Body.HasInstructions && !x.IsConstructor
                && !x.DeclaringType.IsGlobalModuleType && CheckCritical(x))))
                {
                    for (var i = 0; i < methodDef.Body.Instructions.Count; i++)
                    {
                        if (methodDef.Body.Instructions[i].OpCode == OpCodes.Call)
                        {
                            if (methodDef.Body.Instructions[i].Operand is MethodDef callingMethodDef && callingMethodDef.HasBody)
                            {
                                var runtimeMethodHandleLocal = methodDef.Body.Variables.Add(new Local(new ValueTypeSig(hRuntimeMethod)));
                                if (methodDef.Body.HasExceptionHandlers == false)
                                {
                                    methodDef.Body.Instructions[i].ReplaceWith(OpCodes.Ldtoken, ModuleDef.GlobalType);
                                    methodDef.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, _GetTypeFromHandle));
                                    methodDef.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Callvirt, _GetMethod));
                                    methodDef.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Ldc_I4, callingMethodDef.MDToken.ToInt32()));
                                    methodDef.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Call, _ResolveMethod));
                                    methodDef.Body.Instructions.Insert(i + 5, new Instruction(OpCodes.Callvirt, _GetMethodHandle));
                                    methodDef.Body.Instructions.Insert(i + 6, new Instruction(OpCodes.Stloc, runtimeMethodHandleLocal));
                                    methodDef.Body.Instructions.Insert(i + 7, new Instruction(OpCodes.Ldloca, runtimeMethodHandleLocal));
                                    methodDef.Body.Instructions.Insert(i + 8, new Instruction(OpCodes.Call, _FuncPointer));
                                    methodDef.Body.Instructions.Insert(i + 9, new Instruction(OpCodes.Calli, callingMethodDef.MethodSig));
                                    i += 9;
                                }
                            }
                        }
                    }
            
                }
            }
        }
        public bool CheckCritical(IDnlibDef dnlibDef)
        {
            if (dnlibDef is TypeDef typeDef)
            {
                return typeDef.IsRuntimeSpecialName == false;
            }
            if (dnlibDef is FieldDef fieldDef)
            {
                return fieldDef.IsRuntimeSpecialName == false
                    && fieldDef.IsLiteral == false
                    && fieldDef.DeclaringType.IsEnum == false;
            }
            if (dnlibDef is MethodDef methodDef)
            {
                return methodDef.IsRuntimeSpecialName && methodDef.DeclaringType.IsForwarder
                    ? false
                    : true;
            }
            if (dnlibDef is EventDef eventDef)
            {
                return eventDef.IsRuntimeSpecialName == false;
            }
            return true;
        }
    }
    public static class InstructionExtensions
    {
        public static Instruction ReplaceWith(this Instruction source, OpCode opCode, object operand)
        {
            source.OpCode = opCode;
            source.Operand = operand;
            return source;
        }
    }
}
