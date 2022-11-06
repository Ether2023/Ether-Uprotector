using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OZ_IL2CPP_GUI
{
    static class Il2cppSecurity
    {
        public static BaseEncrypter encrypter;

        public static void Process()
        {
            encrypter.Process();
        }

        public static void ProcessExe(string path)
        {
            encrypter = new ExeEncrypter(path);
        }

        public static void ProcessApk(string path)
        {
            encrypter = new ApkEncrypter(path);
        }

        public static void ProcessIpa(string path)
        {
            
        }
    }
}
