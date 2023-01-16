using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ether_IL2CPP_GUI
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
            MessageBoxA(IntPtr.Zero, s, "提示", 0x40);
        }

        public static void ShowError(string s)
        {
            MessageBoxA(IntPtr.Zero, s, "错误", 0x10);
        }

        public static void MoveDirectory(string sourcePath, string destPath, bool ov=true)
        {
            string floderName = Path.GetFileName(sourcePath);
            DirectoryInfo di = Directory.CreateDirectory(Path.Combine(destPath, floderName));
            string[] files = Directory.GetFileSystemEntries(sourcePath);

            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    MoveDirectory(file, di.FullName, ov);
                }
                else
                {
                    File.Move(file, Path.Combine(di.FullName, Path.GetFileName(file)), ov);
                }
            }
        }

        public static void CopyDirectory(string sourcePath, string destPath)
        {
            string floderName = Path.GetFileName(sourcePath);
            DirectoryInfo di = Directory.CreateDirectory(Path.Combine(destPath, floderName));
            string[] files = Directory.GetFileSystemEntries(sourcePath);

            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    CopyDirectory(file, di.FullName);
                }
                else
                {
                    File.Copy(file, Path.Combine(di.FullName, Path.GetFileName(file)), true);
                }
            }
        }

        public static void OpenDirectory(string d)
        {
            system("start " + d);
        }

        public static string ExecCmd(string fp, string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = fp;
            p.StartInfo.Arguments = cmd;
            p.StartInfo.CreateNoWindow = false;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            

            string s = p.StandardOutput.ReadToEnd();

            p.WaitForExit();
            p.Close();
            return s;
        }

        [DllImport("msvcrt.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public extern static void system(string command); // longjmp


        [DllImport("user32.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public extern static void MessageBoxA(IntPtr hwnd, string text, string title, int mb_id); 
    }
}
