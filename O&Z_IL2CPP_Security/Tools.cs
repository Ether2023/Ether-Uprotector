using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Z_IL2CPP_Security
{
    internal class Tools
    {
        public static byte[] addBytes(byte[] data1, byte[] data2)
        {
            byte[] data3 = new byte[data1.Length + data2.Length];
            data1.CopyTo(data3, 0);
            data2.CopyTo(data3, data1.Length);
            return data3;
        }
        public static byte[] StreamToBytes(Stream stream)

        {

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }
    }
}
