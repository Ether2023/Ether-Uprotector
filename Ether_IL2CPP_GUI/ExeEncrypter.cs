using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ether_IL2CPP_GUI
{
    class ExeEncrypter : BaseEncrypter
    {
        public ExeEncrypter(string path) : base(path) { }

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
                ErrorMsg("不是一个Unity应用程序");
                return;
            }

            Utilitys.CopyDirectory(dirp, TempPath);

            string fp = TempPath+datap + "/il2cpp_data/Metadata/global-metadata.dat";
            if(!EncryptMetadataFile(fp, fp + ".crypt"))
            {
                return;
            }
            File.Move(fp + ".crypt", fp, true);

            if (Directory.Exists(GetOutputPath()))
            {
                Directory.Delete(GetOutputPath());
            }
            Utilitys.MoveDirectory(TempPath, GetOutputPath());
        }
    }
}
