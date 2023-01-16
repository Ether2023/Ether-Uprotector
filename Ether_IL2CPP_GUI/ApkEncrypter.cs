using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ether_IL2CPP_GUI
{
    class ApkEncrypter : BaseEncrypter
    {
        public ApkEncrypter(string path) : base(path) { }

        protected override void ProcessFile()
        {
            base.ProcessFile();
            //移动到临时文件夹
            string apkp = TempPath + "/origin.apk";
            File.Copy(FilePath, apkp);
            //解压缩
            string unzipp = TempPath + "/Unzip/";
            Directory.CreateDirectory(unzipp);
            try
            {
                Zip.UnZip(apkp, unzipp);
            }
            catch
            {
                ErrorMsg("apk文件解压错误");
                return;
            }

            string datap = "assets/bin/Data";
            string fp = unzipp + datap + "/Managed/Metadata/global-metadata.dat";

            //处理Metadata
            if(!EncryptMetadataFile(fp, fp + ".crypt"))
            {
                return;
            }
            File.Move(fp + ".crypt", fp, true);

            //压缩文件
            string newapkp = TempPath + "/oz.apk";
            Zip.CreateZip(unzipp, newapkp);


            //输出
            if (File.Exists(GetOutputPath()))
            {
                File.Delete(GetOutputPath());
            }
            File.Move(newapkp, GetOutputPath());

            Utilitys.ShowMsg("加密成功!\n提示:Apk加密完成后需要手动签名");
        }
    }
}
