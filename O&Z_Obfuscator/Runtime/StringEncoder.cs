using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ_Obfuscator.Runtime
{
    public class StringEncoder
    {
        /*
        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }
        */

        public static String DecryptString1(String sign, String data, String key)
        {
            byte[] para1 = Convert.FromBase64String(data);
            byte[] para2 = Encoding.UTF8.GetBytes(key);
            if (para1.Length == 0)
            {
                return data;
            }

            Int32 length = para1.Length;
            Int32 n = (((length & 3) == 0) ? (length >> 2) : ((length >> 2) + 1));
            UInt32[] result;
            result = new UInt32[n];

            for (Int32 i = 0; i < length; i++)
            {
                result[i >> 2] |= (UInt32)para1[i] << ((i & 3) << 3);
            }

            byte[] FixedKey;
            if (para2.Length != 16)
            {
                Byte[] fixedkey = new Byte[16];
                if (para2.Length < 16)
                {
                    para2.CopyTo(fixedkey, 0);
                }
                else
                {
                    Array.Copy(para2, 0, fixedkey, 0, 16);
                }
                FixedKey = fixedkey;
            }
            else FixedKey = para2;

            Int32 length1 = FixedKey.Length;
            Int32 n1 = (((length1 & 3) == 0) ? (length1 >> 2) : ((length1 >> 2) + 1));
            UInt32[] result1;
            result1 = new UInt32[n1];

            for (Int32 i = 0; i < length1; i++)
            {
                result1[i >> 2] |= (UInt32)FixedKey[i] << ((i & 3) << 3);
            }


            uint[] de;

            Int32 n3 = result.Length - 1;
            if (n3 < 1)
            {
                de = result;
            }
            UInt32 zzz, yyy = result[0], summm, eee;
            Int32 p, q = 6 + 52 / (n3 + 1);
            unchecked
            {
                summm = (UInt32)(q * 0x9E3779B9);
                while (summm != 0)
                {
                    eee = summm >> 2 & 3;
                    for (p = n3; p > 0; p--)
                    {
                        zzz = result[p - 1];
                        //yyy = result[p] -= MX(summm, yyy, zzz, p, eee, result1);
                        yyy = result[p] -= (zzz >> 5 ^ yyy << 2) + (yyy >> 3 ^ zzz << 4) ^ (summm ^ yyy) + (result1[p & 3 ^ eee] ^ zzz);
                    }
                    zzz = result[n3];
                    //yyy = result[0] -= MX(summm, yyy, zzz, p, eee, result1);
                    yyy = result[0] -= (zzz >> 5 ^ yyy << 2) + (yyy >> 3 ^ zzz << 4) ^ (summm ^ yyy) + (result1[p & 3 ^ eee] ^ zzz);
                    summm -= 0x9E3779B9;
                }
            }
            de = result;

            Int32 n2 = de.Length << 2;
            Int32 m = (Int32)de[de.Length - 1];
            n2 -= 4;

            n2 = m;
            Byte[] result2 = new Byte[n2];
            if ((m < n2 - 3) || (m > n2))
            {
                result2 = null;
            }
            else
                for (Int32 i = 0; i < n2; i++)
                {
                    result2[i] = (Byte)(de[i >> 2] >> ((i & 3) << 3));
                }

            byte[] debyte = result2;
            string ret = Encoding.UTF8.GetString(debyte);
            return ret;
        }

        public unsafe static String DecryptString2(String sign, String data, String key)
        {
            byte[] key_ = Encoding.UTF8.GetBytes(key);
            byte[] data_ = Convert.FromBase64String(data);
            byte[] mBox = new byte[256];
            fixed (byte* _mBox = &mBox[0])
            {
                for (Int64 i = 0; i < 256; i++)
                {
                    *(_mBox + i) = (byte)i;
                }
                Int64 j = 0;
                int lengh = key_.Length;
                fixed (byte* _pass = &key_[0])
                {
                    for (Int64 i = 0; i < 256; i++)
                    {
                        j = (j + *(_mBox + i) + *(_pass + (i % lengh))) % 256;
                        byte temp = *(_mBox + i);
                        *(_mBox + i) = *(_mBox + j);
                        *(_mBox + j) = temp;
                    }
                }
            }
            byte[] output = new byte[data_.Length];
                fixed (byte* _mBox = &mBox[0])
                fixed (byte* _data = &data_[0])
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
            return Encoding.UTF8.GetString(output);
        }
        public unsafe static String DecryptString3(String sign, String data, String key)
        {
            byte[] key_ = Encoding.UTF8.GetBytes(key);
            byte[] data_ = Convert.FromBase64String(data);
            byte[] mBox = new byte[256];
            fixed (byte* _mBox = &mBox[0])
            {
                for (Int64 i = 0; i < 256; i++)
                {
                    *(_mBox + i) = (byte)i;
                }
                Int64 j = 0;
                int lengh = key_.Length;
                fixed (byte* _pass = &key_[0])
                {
                    for (Int64 i = 0; i < 256; i++)
                    {
                        j = (j + *(_mBox + i) + *(_pass + (i % lengh))) % 256;
                        byte temp = *(_mBox + i);
                        *(_mBox + i) = *(_mBox + j);
                        *(_mBox + j) = temp;
                    }
                }
            }
            byte[] output = new byte[data_.Length];
            fixed (byte* _mBox = &mBox[0])
            fixed (byte* _data = &data_[0])
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
            return Encoding.UTF8.GetString(output);
        }
    }
}

