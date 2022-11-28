using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dnlib.test
{
    public static class Extensions
    {
        public static string ToBase64(this string str)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(str));
        }
    }
    public class Tools
    {
        public static Importer Importer;
        public static ModuleDefMD SelfModule = ModuleDefMD.Load(typeof(Tools).Assembly.Modules.First());
        public static TypeDef GetRuntimeType(string fullName)
        {
            var type = SelfModule.Find(fullName, true);
            return Clone(type);
        }
        public static TypeDef Clone(TypeDef origin)
        {
            var ret = CopyTypeDef(origin);

            foreach (TypeDef nestedType in origin.NestedTypes)
                ret.NestedTypes.Add(Clone(nestedType));

            foreach (MethodDef method in origin.Methods)
                ret.Methods.Add(CopyMethodDef(method));

            foreach (FieldDef field in origin.Fields)
                ret.Fields.Add(CopyFieldDef(field));
            return ret;
        }
        static TypeDef CopyTypeDef(TypeDef origin)
        {
            var ret = new TypeDefUser(origin.Namespace, origin.Name);
            ret.Attributes = origin.Attributes;

            if (origin.ClassLayout != null)
                ret.ClassLayout = new ClassLayoutUser(origin.ClassLayout.PackingSize, origin.ClassSize);

            foreach (GenericParam genericParam in origin.GenericParameters)
                ret.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));

            ret.BaseType = (ITypeDefOrRef)Importer.Import(ret.BaseType);

            foreach (InterfaceImpl iface in origin.Interfaces)
                ret.Interfaces.Add(new InterfaceImplUser((ITypeDefOrRef)Importer.Import(iface.Interface)));
            return ret;
        }
        static MethodDef CopyMethodDef(MethodDef origin)
        {
            var newMethodDef = new MethodDefUser(origin.Name, null, origin.ImplAttributes, origin.Attributes);

            foreach (GenericParam genericParam in origin.GenericParameters)
                newMethodDef.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));


            newMethodDef.Signature = Importer.Import(origin.Signature);
            newMethodDef.Parameters.UpdateParameterTypes();

            if (origin.ImplMap != null)
                newMethodDef.ImplMap = new ImplMapUser(new ModuleRefUser(origin.Module, origin.ImplMap.Module.Name), origin.ImplMap.Name, origin.ImplMap.Attributes);

            foreach (CustomAttribute ca in origin.CustomAttributes)
                newMethodDef.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)Importer.Import(ca.Constructor)));

            if (origin.HasBody)
            {
                newMethodDef.Body = new CilBody(origin.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>());
                newMethodDef.Body.MaxStack = origin.Body.MaxStack;

                var bodyMap = new Dictionary<object, object>();

                foreach (Local local in origin.Body.Variables)
                {
                    var newLocal = new Local(Importer.Import(local.Type));
                    newMethodDef.Body.Variables.Add(newLocal);
                    newLocal.Name = local.Name;
                    newLocal.Attributes = local.Attributes;

                    bodyMap[local] = newLocal;
                }

                foreach (Instruction instr in origin.Body.Instructions)
                {
                    var newInstr = new Instruction(instr.OpCode, instr.Operand);
                    newInstr.SequencePoint = instr.SequencePoint;

                    if (newInstr.Operand is IType)
                        newInstr.Operand = Importer.Import((IType)newInstr.Operand);

                    else if (newInstr.Operand is IMethod)
                        newInstr.Operand = Importer.Import((IMethod)newInstr.Operand);

                    else if (newInstr.Operand is IField)
                        newInstr.Operand = Importer.Import((IField)newInstr.Operand);

                    newMethodDef.Body.Instructions.Add(newInstr);
                    bodyMap[instr] = newInstr;
                }

                foreach (Instruction instr in newMethodDef.Body.Instructions)
                {
                    if (instr.Operand != null && bodyMap.ContainsKey(instr.Operand))
                        instr.Operand = bodyMap[instr.Operand];

                    else if (instr.Operand is Instruction[])
                        instr.Operand = ((Instruction[])instr.Operand).Select(target => (Instruction)bodyMap[target]).ToArray();
                }

                foreach (ExceptionHandler eh in origin.Body.ExceptionHandlers)
                    newMethodDef.Body.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
                    {
                        CatchType = eh.CatchType == null ? null : (ITypeDefOrRef)Importer.Import(eh.CatchType),
                        TryStart = (Instruction)bodyMap[eh.TryStart],
                        TryEnd = (Instruction)bodyMap[eh.TryEnd],
                        HandlerStart = (Instruction)bodyMap[eh.HandlerStart],
                        HandlerEnd = (Instruction)bodyMap[eh.HandlerEnd],
                        FilterStart = eh.FilterStart == null ? null : (Instruction)bodyMap[eh.FilterStart]
                    });

                newMethodDef.Body.SimplifyMacros(newMethodDef.Parameters);
            }

            return newMethodDef;
        }

        static FieldDef CopyFieldDef(FieldDef fieldDef)
        {
            var newFieldDef = new FieldDefUser(fieldDef.Name, null, fieldDef.Attributes);

            newFieldDef.Signature = Importer.Import(fieldDef.Signature);

            return newFieldDef;
        }
        
    }
    public class NameGenerator
    {
        public static void GetObfusName(IMemberDef member, Mode mode, int depth = 1, int sublength = 10)
        {
            member.Name = GetEndName(mode, depth, sublength);
        }

        public static string GetEndName(Mode mode, int depth = 1, int sublength = 10)
        {
            string endname = string.Empty;
            for (int i = 0; i < depth; i++)
            {
                endname += GetName(mode, sublength);
            }
            return endname;
        }

        public static string GetName(Mode mode, int length)
        {
            switch (mode)
            {
                case Mode.Base64:
                    return GenertateRandomFuncName().ToBase64();
                case Mode.Chinese:
                    return GetZH_CNString(length);
                case Mode.Invalid:
                    return GetERRORString(length);
                case Mode.FuncName:
                    return GenertateRandomFuncName();
                default:
                    throw new InvalidOperationException();
            }
        }

        public enum Mode
        {
            Base64,
            Chinese,
            Invalid,
            FuncName
        }
        public static string GetZH_CNString(int len)
        {
            string shit = "";
            for (int i = 0; i < len; i++)
{
                shit += ChineseCharacters[RandomGenerator.Generate(ChineseCharacters.Length)];
            }
            return shit;
        }
        public static char[] ChineseCharacters => new char[]
        {
            '囖','囋','㐂','墼','姵','嬧','㐆','尐','巎','彟','廅'
        };
        public static string GenertateRandomFuncName()
{
            return FuncNames[RandomGenerator.Generate(FuncNames.Length)];
        }

        public static string[] FuncNames =
        {
            "Awake","Start","Updata","FixedUpdata",
            "OnApplicationFocus","OnApplicationPause","OnApplicationQuit",
            "OnBecameInvisible","OnBecameVisible",
            "OnCollisionEnter","OnCollisionExit","OnCollisionStay",
            "OnConnectedToServer","OnDisconnectedFromServer","OnFailedToConnect","OnFailedToConnectToMasterServer",
            "OnMasterServerEvent","OnNetworkInstantiate",
            "OnPlayerConnected","OnPlayerDisconnected","OnControllerColliderHit",
            "OnParticleCollision","OnDisable","OnEnable","OnDrawGizmos","OnDrawGizmosSelected",
            "OnGUI","OnJointBreak","OnLevelWasLoaded","OnMouseDown","OnMouseDrag","OnMouseEnter",
            "OnMouseExit","OnMouseOver","OnMouseUp","OnPostRender","OnPreCull","OnPreRender",
            "OnRenderImage","OnRenderObject","OnSerializeNetworkView","OnServerInitialized",
            "OnTriggerEnter","OnTriggerExit","OnTriggerStay"
        };
        
        public const string ERROR = ";̥͓̠̙̠̺̫̱̹̮͈͈͓͍̟̻͆ͧ͒ͩͨ̉ͯ̂̈̉̽̉͑̔̊́͟͜;̢̧͔͓͉̝̆̒ͣͣ̄ͣ̊̈́̎̓͛̇͆ͯͪ̿͟;ͫͭ͒̉̐͑̀҉̭̭͕̟͇̰̺͖͎̗̰̩͉;̸̛̘̬̫̫͔̜͙̣̯̠̯̻͍̰͍̥͓ͦ̎ͯͯ͂ͤ̉̃̊͐̐̽͜;̵̧̂͐̉̆́̚̚҉̜̦̳͇͍;̙͈̞̪̖͚̬͍͙̹ͭ̿͒ͧͧͨ̀;͐̇̋̍̿̎̀͌ͣ͌҉ ̞̙̘̱͟͠_̍̽̋̑͒ͧ̌͐̿͞͡҉̯̣̥̹̗̫̥̩͈̘͟ͅͅ_̈͛̈͊̈́ͥͬ̌ͪ̃̽͑̓͋͛̆̈͋̽҉̸̛̠̝̜͈̮͢_͒͂̋̈́͋̉͒ͦ̊ͯ̐̾̂̐҉҉̧̯̜̣̮̦̱̦̖̗͡_̧̨̹̳̘̯̱͖͙̘̍ͥ͊͌̌ͧͥ̍ͨ̐͡_̵̺̮̞̖̰͔̮̺̳͖̳̳̥͖͖͊͐͛ͥͪ͛͑͠ͅ_ͧ̓ ̴̡̪͎̣̘̳̤̬͔̟̺̳̻̥͇ͧͫ̽͐̄ͤ̎̔_̸̠̪̺͕̩̮̹̦͇̫͙͖̦̻̏̈́̅ͦ͐_̴̸̢͚̤͙͓̱̬̫̝̞̣̥̽͛͊ͥͬ̍͆ͨ͑͋̍͊ͭ͗́ͅ^̵͖̖̹̦͎̦̜͋̉͋͐̈́ͪ̋̊̄́͘͟ ̨͚͙͖̫͚̙̊̏̍̐ͥ̅̏̎͆͗ͧ́̚͞!̧͕͈͕͙̱̟̆ͭ͋ͫ̕͢͞;̛̣̭̖̹̜̘̮̜̭͓̰̫͙͋̏ͯͤ̂ͬ͗ͥ̌ͥ̓ͮͪ͗́͞ͅ;̪̳̼̱̽ͨ͋͛̔ͪͬ̃͌̂̌͐̀ͧͬ̾ͨ̚̚;̛͍̘̗̣͉͓̘͖͙̪͙̦͇̩͈ͩ͋̄̓ͣ́̃ͦͫ͒̑͋̃ͣͥ̋̀;̢͚̰͈͍ͮͤͣ͂̆͋ͨ̀̐̕͞͞ͅ;̨̢̬̹̯̯̤͕͍̺̩̫͈͉̙̪̪̜̻͚̂͋̏̓͛ͣ͟;̥̖̭͕͔̝͇̞̠̰͐̿̆ͣ̈͟͡;̵̸̻̫͔̼͚̤͇̝̞̬̞͚͇̓̐͆̾ͭ̈́ͫ̈́́͜͞;̌ͨ͌̐̉̂̃̅̃̋ͤͤͣͯ҉̧̹̗̺̹͈̙͇̦̣ͅ;̸̫̙͈̫̮̻͎̱͓̗̍&a_̈͛̈͊̈́ͥͬ̌ͪ̃̽͑̓͋͛̆̈͋̽҉̸̛̠̝̜͈̮͢_͒&‮‮‮‮‮‮‮‮‮‮‮";


        public static string GetERRORString(int len)
        {
            string sta = "";
            for (int i = 0; i < len; i++)
{
                sta += ERROR[RandomGenerator.Generate(ERROR.Length)];
            }
            return sta;
        }
    }
    public class RandomGenerator
    {
        public static int Generate(int Range)
        {
            return RandomNumberGenerator.GetInt32(Range);
        }
        public static void GetXor(int input, out int output1, out int output2)
        {
            int a = RandomNumberGenerator.GetInt32(int.MaxValue);
            int b = RandomNumberGenerator.GetInt32(int.MaxValue);
            int c = input ^ a ^ b;
            int d = b ^ a;
            output1 = c;
            output2 = d;
        }
    }
}
