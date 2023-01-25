using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Ether.Il2cpp
{
    public class ApkEncrypter : BaseEncrypter
    {
        public ApkEncrypter(string path, EtherIl2cppConfig config) : base(path, config) { }

        protected override void ProcessFile()
        {
            base.ProcessFile();
            //移动到临时文件夹
            string apkp = TempPath + "/origin.apk";
            File.Copy(FilePath, apkp);
            //解压缩
            string unzipp = TempPath + "/Unzip/";
            Directory.CreateDirectory(unzipp);
            Zip.UnZip(apkp, unzipp);

            
            if(!EtherIl2cppNative.EncryptApkUnpacked(unzipp, config))
            {
                throw new Exception(EtherIl2cppNative.GetLastError());
            }

            //压缩文件
            string newapkp = TempPath + "/ether.apk";
            Zip.CreateZip(unzipp, newapkp);

            //输出
            if (File.Exists(GetOutputPath()))
            {
                File.Delete(GetOutputPath());
            }
            File.Move(newapkp, GetOutputPath());
        }
    }
}
