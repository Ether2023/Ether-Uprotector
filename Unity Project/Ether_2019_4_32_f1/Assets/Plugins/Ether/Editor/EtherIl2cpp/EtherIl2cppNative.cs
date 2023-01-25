using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ether.Il2cpp
{
    static class EtherIl2cppNative
    {
        const int API_VERSION = 105;

        const string LIB = "EtherIl2cpp";

        [DllImport(LIB)]
        private extern static int get_version();

        [DllImport(LIB)]
        private extern static int get_api_version();

        [DllImport(LIB, CharSet = CharSet.Ansi)]
        private extern static bool process_libil2cpp(string path, string config);

        [DllImport(LIB, CharSet = CharSet.Ansi)]
        private extern static bool restore_libil2cpp(string path, string config);

        [DllImport(LIB, CharSet = CharSet.Ansi)]
        private extern static bool encrypt_win(string game_dir, string game_exe_name, string config);

        [DllImport(LIB, CharSet = CharSet.Ansi)]
        private extern static bool encrypt_android(string input_apk_unpack, string config);

        public static bool CheckApiVersion()
        {
            return API_VERSION == get_api_version();
        }

        static void AssertApiVersion()
        {
            if(API_VERSION != get_api_version())
            {
                throw new Exception("EtherIl2cppNative native API not match!");
            }
        }

        public static bool InstallEtherIl2cpp(string libIl2cppPath, EtherIl2cppConfig config)
        {
            AssertApiVersion();
            ClearLastError();
            CheckPathLegal(libIl2cppPath);
            string cfg = JsonUtility.ToJson(new NativeEncryptConfig(config));
            File.WriteAllText(libIl2cppPath + "/EtherIl2cppConfig.json", cfg);
            return process_libil2cpp(libIl2cppPath, cfg);
        }

        public static bool UninstallEtherIl2cpp(string libIl2cppPath, EtherIl2cppConfig config)
        {
            AssertApiVersion();
            ClearLastError();
            CheckPathLegal(libIl2cppPath);
            if(File.Exists(libIl2cppPath + "/EtherIl2cppConfig.json"))
            {
                File.Delete(libIl2cppPath + "/EtherIl2cppConfig.json");
            }
            return restore_libil2cpp(libIl2cppPath, JsonUtility.ToJson(new NativeEncryptConfig(config)));
        }

        public static bool EncryptExe(string gameDir, string exePath, EtherIl2cppConfig config)
        {
            AssertApiVersion();
            ClearLastError();
            CheckPathLegal(exePath);
            CheckPathLegal(gameDir);
            return encrypt_win(gameDir, exePath, JsonUtility.ToJson(new NativeEncryptConfig(config)));
        }

        public static bool EncryptApkUnpacked(string apkUnpackedPath, EtherIl2cppConfig config)
        {
            AssertApiVersion();
            ClearLastError();
            CheckPathLegal(apkUnpackedPath);
            return encrypt_android(apkUnpackedPath, JsonUtility.ToJson(new NativeEncryptConfig(config)));
        }

        public static void CheckPathLegal(string str)
        {
            if(Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
            {
                throw new Exception("非法路径, 请删除路径中的中文!");
            }
        }

        public static void ClearLastError()
        {
            File.Delete(GetLogFilePath());
        }

        public static string GetLastError()
        {
            string s = File.ReadAllText(GetLogFilePath());
            string[] lines = s.Split('\n');
            string err = lines[lines.Length - 1];// last line
            if (string.IsNullOrEmpty(err))
            {
                err = lines[lines.Length - 2];
            }
            return err;
        }

        public static string GetLogFilePath()
        {
            string LogFilePath = "./output.log";
#if UNITY_EDITOR
            LogFilePath = Application.persistentDataPath + "/etheril2cpp.log";
#endif
#if RELEASE
            LogFilePath = "./etheril2cpp.log";
#endif
            return LogFilePath;
        }

        [Serializable]
        public class NativeEncryptConfig
        {
            // basic config
            public string logfile = "";
            public string unity_version = "2018.4.23";

            // custom options
            public string encrypt_key = "666";
            public bool enable_check_sum = true;
            // not supported below
            public bool enable_strings_encrypt = false;
            public bool enable_api_obfuscate = false;
            public bool enable_proxy_check = false;
            public bool enable_console_for_win = false;

            public NativeEncryptConfig() {  }

            public NativeEncryptConfig(EtherIl2cppConfig config)
            {
                logfile = GetLogFilePath();
                unity_version = config.UnityVersion;
                enable_check_sum = config.EnableCheckSum;
                byte[] keyBytes = Encoding.UTF8.GetBytes(config.EncryptKey);
                encrypt_key = Convert.ToBase64String(keyBytes);
                enable_strings_encrypt = config.EnableStringEncrypt;
                enable_api_obfuscate = config.EnableIl2cppAPIObfuscate;
            }
        }
    }

    [Serializable]
    public class EtherIl2cppConfig
    {
        public string UnityVersion { get; set; }

        public string EncryptKey { get; set; }
        public bool EnableCheckSum { get; set; }
        public bool EnableStringEncrypt { get; set; }
        public bool EnableIl2cppAPIObfuscate { get; set; }

        public static EtherIl2cppConfig Default = new EtherIl2cppConfig()
        {
            UnityVersion = Application.unityVersion,
            EncryptKey = "114514",
            EnableCheckSum = true,
            EnableStringEncrypt = false,
            EnableIl2cppAPIObfuscate = false,
        };
    }
}
