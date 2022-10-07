using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OZ_IL2CPP_GUI
{
    static class Utilitys
    {
        public static string UserSelectUnityExe()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择Unity.exe";
            ofd.Filter = "Unity.exe|*.exe";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static string UserSelectExeFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择exe文件";
            ofd.Filter = "Exe文件(*.exe)|*.exe";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static string UserSelectApkFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择apk文件";
            ofd.Filter = "Android安装包(*.apk)|*.apk";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static string UserSelectIpaFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择ipa文件";
            ofd.Filter = "IOS安装包(*.ipa)*.ipa";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static void ShowMsg(string s)
        {
            MessageBox.Show(s);
        }
    }
}
