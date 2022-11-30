using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

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

            UnityVersion uniVer = new UnityVersion(GetUnityVersion(unityExePath));
            string il2Ver = Il2cppLibUtilitys.GetVersion(uniVer);

            if (!Il2cppLibUtilitys.HasOZSupport(il2Ver))
            {
                Utilitys.ShowError("OZ Il2cpp暂不支持该版本Unity\nIl2cpp版本:" + il2Ver);
                return;
            }

            //Update json
            UpdateConfigIl2cppVersion(il2Ver);
            //Gen code
            BaseEncrypter.InvokeOZIL2CPPSecurity("Generate");

            string editorPath = Path.GetDirectoryName(unityExePath);

            //Check
            if (!CheckWritePermission(editorPath))
            {
                return;
            }

            //CheckBackup
            string zipPath = editorPath + "/libil2cpp.zip";
            if (File.Exists(zipPath))
            {
                Utilitys.ShowError("您已经安装OZIl2cpp\n请勿重复安装!");
                return;
            }
            //Backup
            CreateBackup(editorPath);

            //Move
            if(!InstallLibil2cpp(editorPath, il2Ver))
            {
                //Uninstall if failed
                UnInstall(unityExePath);
                return;
            }

            //Utilitys.CopyDirectory()
            Utilitys.ShowMsg("安装成功!");
        }

        public static void UpdateConfigIl2cppVersion(string v)
        {
            ConfigJson cfg = JsonConvert.DeserializeObject<ConfigJson>(File.ReadAllText("Config.json"));
            cfg.Version = v;
            File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
        }

        public static string GetUnityVersion(string fp)
        {
            FileVersionInfo f_vi = FileVersionInfo.GetVersionInfo(fp);
            return f_vi.FileVersion;
        }

        public static bool CheckWritePermission(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Utilitys.ShowError("内部错误\n文件夹不存在");
                }
                File.WriteAllText(dir + "/z9.txt", "TestPermission");
                File.Delete(dir + "/z9.txt");
            }
            catch
            {
                Utilitys.ShowError("无读写权限" + "\n" +
                    "请以管理员身份运行");
                //提升权限
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = Application.ExecutablePath;
                //设置启动动作,确保以管理员身份运行
                startInfo.Verb = "runas";
                Process.Start(startInfo);
                Application.Exit();
                return false;
            }
            return true;
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

        static bool InstallLibil2cpp(string edtp,string ver)
        {
            string libIl2cppPath = edtp + "\\Data\\il2cpp\\";
            try
            {
                Utilitys.CopyDirectory("./Generation/" + ver + "/libil2cpp", libIl2cppPath);
            }
            catch(Exception e)
            {
                Utilitys.ShowError("安装OZIl2cpp失败"+"\n错误原因:\n" + e.ToString());
                return false;
            }
            Directory.Delete("./Generation");
            return true;
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

public class ConfigJson
{
    // Token: 0x0400009D RID: 157
    public int key;

    // Token: 0x0400009E RID: 158
    public string Version;
}