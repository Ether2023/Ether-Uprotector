using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OZ_IL2CPP_GUI
{
    static class Il2cppSecurity
    {
        public static string FilePath
        {
            get;set;
        }

        public static string OutputPath => FilePath+".encrypted";

        public static void Process()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                Utilitys.ShowMsg("No file selected");
            }
            if (!File.Exists(FilePath))
            {
                Utilitys.ShowMsg("File not found");
            }
        }

        static void EncryptMetadata(string path)
        {

        }

        public static void ProcessExe(string path)
        {

        }

        public static void ProcessApk(string path)
        {

        }

        public static void ProcessIpa(string path)
        {

        }
    }
}
