using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Ether.Il2cpp
{
    static class Utilitys
    {
        public static void ShowMsg(string s)
        {
            MessageBoxA(IntPtr.Zero, s, "Info", 0x40);//MB_ICONINFO
        }

        public static void ShowError(string s)
        {
            MessageBoxA(IntPtr.Zero, s, "Error", 0x10);//MB_ICONERROR
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
                    File.Move(file, Path.Combine(di.FullName, Path.GetFileName(file)));
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

        [DllImport("msvcrt.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public extern static void system(string command); // longjmp


        [DllImport("user32.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public extern static void MessageBoxA(IntPtr hwnd, string text, string title, int mb_id); 
    }
}
