using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ether.Il2cpp
{
    public class ExeEncrypter : BaseEncrypter
    {
        public ExeEncrypter(string path, EtherIl2cppConfig config) : base(path, config) { }

        public override string GetOutputPath()
        {
            string p = Path.GetFileNameWithoutExtension(FilePath) + "_encrypted";

            //使用上面两级的文件夹
            p = Path.GetDirectoryName(Path.GetDirectoryName(FilePath)) + "/" + p;

            string dir = Path.GetDirectoryName(p);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return p;
        }

        protected override void ProcessFile()
        {
            base.ProcessFile();
            string dirp = Path.GetDirectoryName(FilePath)+"/";
            string datap = Path.GetFileNameWithoutExtension(FilePath) + "_Data";

            if (!Directory.Exists(dirp + datap))
            {
                throw new Exception("不是一个Unity应用程序");
            }

            Utilitys.CopyDirectory(dirp, TempPath);

            if (!EtherIl2cppNative.EncryptExe(TempPath, Path.GetFileName(FilePath), config))
            {
                throw new Exception(EtherIl2cppNative.GetLastError());
            }

            if (Directory.Exists(GetOutputPath()))
            {
                Directory.Delete(GetOutputPath());
            }
            Utilitys.MoveDirectory(TempPath, GetOutputPath());
        }
    }
}
