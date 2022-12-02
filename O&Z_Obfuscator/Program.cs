using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.DotNet.Emit;
using System.Security.Cryptography;
using OZ_Obfus.obfuscators;
using System;
using System.Linq;
using OZ_Obfuscator.Ofbuscators;
using OZ_Obfus.Rumtime;
using System.Text;

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
            Antide4dot obfus3 = new Antide4dot(loader.Module);
            obfus3.Execute();
            loader.Save();
            //Console.WriteLine(dnlib.test.ModuleType.StringEncoder.DecryptString("123","oMgi2ofCqVjZ8w/8y1R87w==", "123456"));

            //int a, b;
            //GetXor(100, out a, out b);
            string str = StrCrypter.EncryptString2("HelloWorld", "123456");
            Console.WriteLine(str);
            str = StringEncoder.DecryptString2("awa", str, "123456");
            Console.WriteLine(str);
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
    public class RCX
    {
        private byte[] keybox;
        private const int keyLen = 256;
        private Encoding encoding;
        public enum OrderType
        {
            Asc, Desc
        }
        public RCX(byte[] pass)
        {
            encoding = Encoding.UTF8;
            keybox = GetKey(pass, keyLen);
        }


        public RCX(byte[] pass, Encoding encoding)
        {
            this.encoding = encoding;
            keybox = GetKey(pass, keyLen);
        }

        public RCX(string pass)
        {
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");
            var ps = Encoding.UTF8.GetBytes(pass);
            encoding = Encoding.UTF8;
            keybox = GetKey(ps, keyLen);
        }

        public RCX(string pass, Encoding encoding)
        {
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");
            var ps = encoding.GetBytes(pass);
            this.encoding = encoding;
            keybox = GetKey(ps, keyLen);
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            return encrypt(encoding.GetBytes(data), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data, Encoding encoding, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            return encrypt(encoding.GetBytes(data), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data, OrderType order = OrderType.Asc)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length == 0) throw new ArgumentNullException("data");
            return encrypt(data, order);
        }
        private unsafe byte[] encrypt(byte[] data, OrderType order)
        {

            byte[] mBox = new byte[keyLen];
            Array.Copy(keybox, mBox, keyLen);
            //Buffer.BlockCopy(keybox, 0, mBox, 0, keyLen);
            byte[] output = new byte[data.Length];


            if (order == OrderType.Asc)
            {
                fixed (byte* _mBox = &mBox[0])
                fixed (byte* _data = &data[0])
                fixed (byte* _output = &output[0])
                {
                    var length = data.Length;
                    int i = 0, j = 0;
                    for (Int64 offset = 0; offset < length; offset++)
                    {
                        i = (++i) & 0xFF;
                        j = (j + *(_mBox + i)) & 0xFF;

                        byte a = *(_data + offset);
                        byte c = (byte)(a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                        *(_output + offset) = c;

                        byte temp = *(_mBox + a);
                        *(_mBox + a) = *(_mBox + c);
                        *(_mBox + c) = temp;
                        j = (j + a + c);
                    }
                }
            }
            else
            {
                fixed (byte* _mBox = &mBox[0])
                fixed (byte* _data = &data[0])
                fixed (byte* _output = &output[0])
                {
                    var length = data.Length;
                    int i = 0, j = 0;
                    for (int offset = data.Length - 1; offset >= 0; offset--)
                    {
                        i = (++i) & 0xFF;
                        j = (j + *(_mBox + i)) & 0xFF;

                        byte a = *(_data + offset);
                        byte c = (byte)(a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                        *(_output + offset) = c;

                        byte temp = *(_mBox + a);
                        *(_mBox + a) = *(_mBox + c);
                        *(_mBox + c) = temp;
                        j = (j + a + c);
                    }
                }
            }

            //int i = 0, j = 0;
            //if (order == OrderType.Asc) {
            //    for (int offset = 0; offset < data.Length; offset++) {
            //        i = (++i) & 0xFF;
            //        j = (j + mBox[i]) & 0xFF;

            //        byte a = data[offset];
            //        byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //        output[offset] = c;

            //        byte temp2 = mBox[c];
            //        mBox[c] = mBox[a];
            //        mBox[a] = temp2;
            //        j = (j + a + c);
            //    }
            //} else {
            //    for (int offset = data.Length - 1; offset >= 0; offset--) {
            //        i = (++i) & 0xFF;
            //        j = (j + mBox[i]) & 0xFF;

            //        byte a = data[offset];
            //        byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //        output[offset] = c;

            //        byte temp2 = mBox[c];
            //        mBox[c] = mBox[a];
            //        mBox[a] = temp2;
            //        j = (j + a + c);
            //    }
            //}

            return output;
        }


        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, string pass, Encoding encoding, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

            return encrypt(encoding.GetBytes(data), encoding.GetBytes(pass), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, string pass, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

            return encrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(pass), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string pass, Encoding encoding, OrderType order = OrderType.Asc)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length == 0) throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

            return encrypt(data, encoding.GetBytes(pass), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string pass, OrderType order = OrderType.Asc)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length == 0) throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

            return encrypt(data, Encoding.UTF8.GetBytes(pass), order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, byte[] pass, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");
            if (pass.Length == 0) throw new ArgumentNullException("pass");

            return encrypt(Encoding.UTF8.GetBytes(data), pass, order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, byte[] pass, Encoding encoding, OrderType order = OrderType.Asc)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");
            if (pass.Length == 0) throw new ArgumentNullException("pass");

            return encrypt(encoding.GetBytes(data), pass, order);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] pass, OrderType order = OrderType.Asc)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length == 0) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");
            if (pass.Length == 0) throw new ArgumentNullException("pass");

            return encrypt(data, pass, order);
        }
        private unsafe static byte[] encrypt(byte[] data, byte[] pass, OrderType order)
        {
            byte[] mBox = GetKey(pass, keyLen);
            byte[] output = new byte[data.Length];
            //int i = 0, j = 0;

            if (order == OrderType.Asc)
            {
                fixed (byte* _mBox = &mBox[0])
                fixed (byte* _data = &data[0])
                fixed (byte* _output = &output[0])
                {
                    var length = data.Length;
                    int i = 0, j = 0;
                    for (Int64 offset = 0; offset < length; offset++)
                    {
                        i = (++i) & 0xFF;
                        j = (j + *(_mBox + i)) & 0xFF;

                        byte a = *(_data + offset);
                        byte c = (byte)(a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                        *(_output + offset) = c;

                        byte temp = *(_mBox + a);
                        *(_mBox + a) = *(_mBox + c);
                        *(_mBox + c) = temp;
                        j = (j + a + c);
                    }
                }
            }
            else
            {
                fixed (byte* _mBox = &mBox[0])
                fixed (byte* _data = &data[0])
                fixed (byte* _output = &output[0])
                {
                    var length = data.Length;
                    int i = 0, j = 0;
                    for (int offset = data.Length - 1; offset >= 0; offset--)
                    {
                        i = (++i) & 0xFF;
                        j = (j + *(_mBox + i)) & 0xFF;

                        byte a = *(_data + offset);
                        byte c = (byte)(a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                        *(_output + offset) = c;

                        byte temp = *(_mBox + a);
                        *(_mBox + a) = *(_mBox + c);
                        *(_mBox + c) = temp;
                        j = (j + a + c);
                    }
                }
            }

            //if (order == OrderType.Asc) {
            //    for (int offset = 0; offset < data.Length; offset++) {
            //        i = (++i) & 0xFF;
            //        j = (j + mBox[i]) & 0xFF;

            //        byte a = data[offset];
            //        byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //        output[offset] = c;

            //        byte temp2 = mBox[c];
            //        mBox[c] = mBox[a];
            //        mBox[a] = temp2;
            //        j = (j + a + c);
            //    }
            //} else {
            //    for (int offset = data.Length - 1; offset >= 0; offset--) {
            //        i = (++i) & 0xFF;
            //        j = (j + mBox[i]) & 0xFF;

            //        byte a = data[offset];
            //        byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //        output[offset] = c;

            //        byte temp2 = mBox[c];
            //        mBox[c] = mBox[a];
            //        mBox[a] = temp2;
            //        j = (j + a + c);
            //    }
            //}
            return output;
        }



        private static unsafe byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] mBox = new byte[kLen];
            fixed (byte* _mBox = &mBox[0])
            {
                for (Int64 i = 0; i < kLen; i++)
                {
                    *(_mBox + i) = (byte)i;
                }
                Int64 j = 0;
                int lengh = pass.Length;
                fixed (byte* _pass = &pass[0])
                {
                    for (Int64 i = 0; i < kLen; i++)
                    {
                        j = (j + *(_mBox + i) + *(_pass + (i % lengh))) % kLen;
                        byte temp = *(_mBox + i);
                        *(_mBox + i) = *(_mBox + j);
                        *(_mBox + j) = temp;
                    }
                }
            }

            //for (Int64 i = 0; i < kLen; i++) {
            //    mBox[i] = (byte)i;
            //}
            //Int64 j = 0;
            //for (Int64 i = 0; i < kLen; i++) {
            //    j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
            //    byte temp = mBox[i];
            //    mBox[i] = mBox[j];
            //    mBox[j] = temp;
            //}
            return mBox;
        }
    }

}
