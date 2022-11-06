using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OZ_IL2CPP_GUI
{
    static class Il2cppInstaller
    {
        public static void Install(string unityExePath)
        {
            if (!unityExePath.EndsWith("Unity.exe"))
            {
                Utilitys.ShowError("没有选择正确的Unity.exe");
                return;
            }

            string editorPath = Path.GetDirectoryName(unityExePath);
            //CheckBackup
            string zipPath = editorPath + "/libil2cpp.zip";
            if (File.Exists(zipPath))
            {
                Utilitys.ShowError("您已经安装OZIl2cpp\n请勿重复安装!");
                return;
            }
            //Backup
            CreateBackup(editorPath);

            //Utilitys.CopyDirectory()
            Utilitys.ShowMsg("安装成功!");
        }

        public static void UnInstall(string unityExePath)
        {
            string editorPath = Path.GetDirectoryName(unityExePath);

            if (!unityExePath.EndsWith("Unity.exe"))
            {
                Utilitys.ShowError("没有选择正确的Unity.exe");
                return;
            }

            if (!UnpackBackup(editorPath))
            {
                return;
            }

            Utilitys.ShowMsg("卸载成功!");
        }

        static void CreateBackup(string editorPath)
        {
            //Backup
            string libIl2cppPath = editorPath + "\\Data\\il2cpp\\libil2cpp\\";
            Zip.CreateZip(libIl2cppPath, editorPath + "/libil2cpp.zip");
        }

        static bool UnpackOZPack(string editorPath, string ozZip)
        {
            //CheckBackup
            string zipPath = ozZip;
            if (!File.Exists(zipPath))
            {
                Utilitys.ShowError("您没有安装OZIl2cpp或删除了备份文件!");
                return false;
            }

            //Decompress
            string libIl2cppPath = editorPath + "\\Data\\il2cpp\\libil2cpp\\";
            try
            {
                if (Directory.Exists(libIl2cppPath))
                    Directory.Delete(libIl2cppPath, true);
            }
            catch(Exception e)
            {
                Utilitys.ShowError($"删除OZIl2cpp失败\n路径:{libIl2cppPath}\nEx:{e}");
                return false;
            }
            try
            {
                Zip.UnZip(zipPath, libIl2cppPath + "/");
            }
            catch
            {
                return false;
            }
            //删除备份
            File.Delete(zipPath);
            return true;
        }

        static bool UnpackBackup(string editorPath)
        {
            //CheckBackup
            string zipPath = editorPath + "/libil2cpp.zip";
            if (!File.Exists(zipPath))
            {
                Utilitys.ShowError("您没有安装OZIl2cpp或删除了备份文件!");
                return false;
            }

            //Decompress
            string libIl2cppPath = editorPath + "\\Data\\il2cpp\\libil2cpp\\";
            try
            {
                if (Directory.Exists(libIl2cppPath))
                    Directory.Delete(libIl2cppPath, true);
            }
            catch (Exception e)
            {
                Utilitys.ShowError($"删除OZIl2cpp失败\n路径:{libIl2cppPath}\nEx:{e}");
                return false;
            }
            try
            {
                Zip.UnZip(zipPath, libIl2cppPath + "/");
            }
            catch
            {
                return false;
            }
            //删除备份
            File.Delete(zipPath);
            return true;
        }
    }
}
