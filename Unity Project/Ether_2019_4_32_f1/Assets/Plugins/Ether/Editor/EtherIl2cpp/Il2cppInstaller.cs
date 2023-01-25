using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ether.Il2cpp
{
    public static class Il2cppInstaller
    {
        public static void Install(string unityExePath, EtherIl2cppConfig config)
        {
            if (!unityExePath.EndsWith("Unity.exe"))
            {
                throw new Exception("Invaild Unity.exe");
            }

            string editorPath = Path.GetDirectoryName(unityExePath);

            //Check
            //if (!CheckWritePermission(editorPath))
            //{
            //    return;
            //}

            //CheckBackup
            string ozsignFile = editorPath + "/Data/il2cpp/libil2cpp/" + "EtherIl2cppConfig.json";
            config.UnityVersion = Application.unityVersion;
            
            if (File.Exists(ozsignFile))
            {
                throw new Exception("已经安装");
            }
            if(!EtherIl2cppNative.InstallEtherIl2cpp(editorPath + "/Data/il2cpp/libil2cpp/", config))
            {
                throw new Exception(EtherIl2cppNative.GetLastError());
            }

            // Utilitys.ShowMsg("安装成功!");
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
                    throw new Exception("内部错误");
                }
                File.WriteAllText(dir + "/z9.txt", "TestPermission");
                File.Delete(dir + "/z9.txt");
            }
            catch
            {
                throw new Exception("无读写权限\n请以管理员身份运行");
            }
            return true;
        }

        public static void UnInstall(string unityExePath)
        {
            if (!unityExePath.EndsWith("Unity.exe"))
            {
                throw new Exception("Invaild Unity.exe");
            }

            string editorPath = Path.GetDirectoryName(unityExePath);


            //Check
            //if (!CheckWritePermission(editorPath))
            //{
            //    return;
            //}

            string ozsignFile = editorPath + "/Data/il2cpp/libil2cpp/" + "EtherIl2cppConfig.json";
            if (!File.Exists(ozsignFile))
            {
                throw new Exception("没有安装");
            }

            if (!EtherIl2cppNative.UninstallEtherIl2cpp(editorPath+ "/Data/il2cpp/libil2cpp/", EtherIl2cppConfig.Default))
            {
                throw new Exception(EtherIl2cppNative.GetLastError());
            }

            File.Delete(ozsignFile);

            // Utilitys.ShowMsg("卸载成功!");
        }
    }
}