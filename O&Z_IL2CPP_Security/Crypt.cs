using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeIL2CPP_Pro
{
    public static class Crypt
    {
        public static List<byte[]> Cryptstring(List<byte[]> bytes)
        {
            List<byte[]> result = new List<byte[]>();
            foreach (byte[] b in bytes)
            {
                result.Add(CryptByte(b));
            }
            return result;
        }
        public static byte[] CryptByte(byte[] b)
        {
            byte[] result = new byte[b.Length];
            for(int i = 0; i < b.Length; i++)
            {
                result[i] = (byte)(b[i]^114514);
            }
            return result;
        }
    }
}
