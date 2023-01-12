using O_Z_IL2CPP_Security;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using O_Z_IL2CPP_Security.LitJson;
using System.Diagnostics;
using OZ_Obfuscator.Obfuscators;
using OZ_Obfuscator;
using System;
using System.Collections.Generic;
using System.IO;
using OZ_Obfuscator.Ofbuscators.UnityMonoBehavior;
using OZ_Obfuscator.Unity;
#if NET6_0_OR_GREATER
List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();
string OpenFilePath;
byte[]? metadata_origin = null;

Console.WriteLine("O&Z_IL2CPP_Security");
if (!File.Exists("Config.json"))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Config.json not found!");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Download Config.json From Github..."); 
    File.WriteAllText("Config.json", Tools.DownloadText("https://raw.githubusercontent.com/Z1029-oRangeSumMer/O-Z-Unity-Protector/main/Configs/Config.json"));
    if (File.Exists("Config.json")) Console.WriteLine("Download succeeded!");
    Console.ForegroundColor = ConsoleColor.White;
}
if (args.Length == 0)
{
    Help();
    return;
}
JsonManager jsonManager = new JsonManager("Config.json");

if (args[0] == "Generate")
{
    _Generate();
    Console.WriteLine("Done!");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}

OpenFilePath = args[0];

Console.WriteLine("Loading File:" + OpenFilePath);

switch(args[1])
{
    case "Crypt":_Crypt();break;
    case "Decrypt":return;
    case "Read":_Read();break;
    case "Test":_Test();break;
    case "CheckVersion": CheckVersion(); break;
    case "MonoObfus": MonoObfus(); break;
    default:_default();break;
}
return;
void _Crypt()
{
    Console.WriteLine("Encrypting...");
    IL2CPP_Version ver;
    if (!File.Exists(OpenFilePath))
    {
        Console.WriteLine("File is not EXISTS!");
        return;
    }
    metadata_origin = File.ReadAllBytes(OpenFilePath);
    if (!CheckMetadataFile())
    {
        Console.WriteLine("This is NOT a Metadata Fil or it had been crypted!");
        return; 
    }
    jsonManager = new JsonManager("Config.json");
    
    switch(jsonManager.index.Version)
    {
        case "24.4":
            { 
                ver = IL2CPP_Version.V24_4;
                Console.WriteLine("Metadata Verion:24.4");
            }
            break;
        case "28":
            { 
                ver = IL2CPP_Version.V28;
                Console.WriteLine("Metadata Verion:28");
            }
            break;
        case "24.1":
            {
                ver = IL2CPP_Version.V24_1;
                Console.WriteLine("Metadata Verion:24.1");
            }
            break;
        default: Console.WriteLine("Version error! Please ensure that you have configured the correct and supported metadata version!(24.4 / 28)"); return;
    }
    object Loader;
    switch (ver)
    {
        case IL2CPP_Version.V24_4:
            {
                Loader = new LoadMetadata_v24_4(new MemoryStream(metadata_origin));
            }
            break;
        case IL2CPP_Version.V28:
            {
                Loader = new LoadMetadata_v28(new MemoryStream(metadata_origin));
            }
            break;
        case IL2CPP_Version.V24_1:
            {
                Loader = new LoadMetadata_v24_1(new MemoryStream(metadata_origin));
            }
            break;
        default: Console.WriteLine("Version error! Please ensure that you have configured the correct and supported metadata version!"); return;
    }
    Console.WriteLine("Creating O&Z Metadata...");
    Metadata metadata = new Metadata(Loader.GetType().GetField("metadatastream").GetValue(Loader) as Stream, Loader.GetType().GetField("Header").GetValue(Loader).GetType(), Loader.GetType().GetField("Header").GetValue(Loader), ver);
    Console.WriteLine("Encrypting StringLiteral...");
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    Console.WriteLine("Encrypting String...");
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes,jsonManager.index.key);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Console.WriteLine("Building new Metadata...");
    Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, Crypt.CryptWithSkipNULL(allstring,(byte)O_Z_IL2CPP_Security.Tools.CheckNull(jsonManager.index.key), jsonManager.index.key),ver);
    byte[] tmp = O_Z_IL2CPP_Security.Tools.StreamToBytes(stream);
    Console.WriteLine("Writing to File...");
    File.WriteAllBytes(args[2], tmp);
    Console.WriteLine("Done!");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

void _default()
{
    Console.WriteLine("parameter ERROR!");
    Help();
    return;
}
void _Read()
{
    return;
}
void _Generate()
{
    jsonManager = new JsonManager("Config.json");
    string src;
    CPP cpp;
    Console.WriteLine("Creating KEY Component...");
    Console.WriteLine("Metadata Version:" + jsonManager.index.Version);
    Console.WriteLine("KEY:" + jsonManager.index.key);
    if (!Directory.Exists("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/"))
        Directory.CreateDirectory("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/");
    if (jsonManager.index.Version == "24.4")
    {
        src = File.ReadAllText("src-res/" + jsonManager.index.Version + "/MetadataCache.cpp");
        cpp = new CPP(src, IL2CPP_Version.V24_4, jsonManager.index.key, (byte)O_Z_IL2CPP_Security.Tools.CheckNull(jsonManager.index.key));
        File.WriteAllText("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/MetadataCache.cpp", cpp.retsrc);
        File.WriteAllLines("Generation/" + jsonManager.index.Version + "/libil2cpp/il2cpp-metadata.h", File.ReadAllLines("src-res/" + jsonManager.index.Version + "/il2cpp-metadata.h"));
    }
    else if (jsonManager.index.Version == "28")
    {
        src = File.ReadAllText("src-res/" + jsonManager.index.Version + "/GlobalMetadata.cpp");
        cpp = new CPP(src, IL2CPP_Version.V24_4, jsonManager.index.key, (byte)O_Z_IL2CPP_Security.Tools.CheckNull(jsonManager.index.key));
        File.WriteAllText("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/GlobalMetadata.cpp", cpp.retsrc);
        File.WriteAllLines("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/GlobalMetadataFileInternals.h", File.ReadAllLines("src-res/" + jsonManager.index.Version + "/GlobalMetadataFileInternals.h"));
    }
    else
    {
        Console.WriteLine("Version error! Please ensure that you have configured the correct and supported metadata version!");
        return;
    }
    File.WriteAllLines("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/xxtea.cpp", File.ReadAllLines("src-res/xxtea.cpp"));
    File.WriteAllLines("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/xxtea.h", File.ReadAllLines("src-res/xxtea.h"));
    return;
}
bool CheckMetadataFile()
{
    if (BitConverter.ToUInt32(metadata_origin, 0) != 4205910959)
    {
        return false;
    }
    else
        return true;
}
void _Test()
{
    AssemblyLoader loader = new AssemblyLoader(OpenFilePath);
    List<MonoSwapMap> maps= new List<MonoSwapMap>();
    ObfusFunc obfusFunc = new ObfusFunc(loader.Module,out maps);
    obfusFunc.Execute();
    AssetsFile assets = MonoUtils.LoadAsset("C:/Users/22864/Desktop/2019Testbuild/O&Z_2019_4_32_f1_Data/globalgamemanagers.assets.bak");
    MonoUtils.SetMonoMapToAssetFile(assets, maps);
    MonoUtils.SaveAssetsToFile(assets, "C:/Users/22864/Desktop/2019Testbuild/O&Z_2019_4_32_f1_Data/globalgamemanagers.assets.obfus");
    Console.WriteLine("Fuck executing...");
    loader.Save();
}
void CheckVersion()
{
    metadata_origin = File.ReadAllBytes(OpenFilePath);
    if (!CheckMetadataFile()) return;
    MetadataCheck metadataCheck = new MetadataCheck(new MemoryStream(metadata_origin));
    Console.WriteLine("Your Metadata Version:" + metadataCheck.Version);
}
void Help()
{
    Console.WriteLine("Usage:"+"\n");
    Console.WriteLine("Encrypt:\n    O&Z_IL2CPP_Security.exe [Input] Crypt [Output]\n");
    Console.WriteLine("Show Metadata Version:\n    O&Z_IL2CPP_Security.exe [Input] CheckVersion\n");
    Console.WriteLine("Generate KEY Component:\n    O&Z_IL2CPP_Security.exe Generate\n");
    Console.WriteLine("Obfuscate NET Assembly:\n    O&Z_IL2CPP_Security.exe [Input] MonoObfus\n");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
void MonoObfus()
{
    List<Obfuscator> obfuscators= new List<Obfuscator>();
    AssemblyLoader loader = new AssemblyLoader(OpenFilePath);
    if (jsonManager.index.Obfus.ControlFlow == 1)
    {
        obfuscators.Add(new ControlFlow(loader.Module, jsonManager.index.Obfus.ignore_ControlFlow_Method));
    }
    if (jsonManager.index.Obfus.Obfusfunc == 1)
    {
        obfuscators.Add(new ObfusFunc(loader.Module));
    }
    if (jsonManager.index.Obfus.NumObfus == 1)
    {
        obfuscators.Add(new NumObfus(loader.Module));
    }
    if (jsonManager.index.Obfus.LocalVariables2Field == 1)
    {
        obfuscators.Add(new LocalVariables2Field(loader.Module));
    }
    if (jsonManager.index.Obfus.StrCrypter == 1)
    {
        obfuscators.Add(new StrCrypter(loader.Module));
    }
    if(jsonManager.index.Obfus.AntiDe4dot == 1)
    {
        obfuscators.Add(new Antide4dot(loader.Module));
    }
    if(jsonManager.index.Obfus.FuckILdasm == 1)
    {
        obfuscators.Add(new FuckILdasm(loader.Module));
    }
    if(jsonManager.index.Obfus.MethodError==1)
    {
        obfuscators.Add(new MethodError(loader.Module));
    }
    foreach(var obfuscator in obfuscators)
    {
        string outstr = obfuscator.ToString();
        int i = outstr.IndexOf("Obfuscators.");
        outstr = outstr.Substring(i+12, outstr.Length-i-12);
        Console.WriteLine(outstr + " Executing...");
        obfuscator.Execute();
    }
    loader.Save();
    if(jsonManager.index.Obfus.PEPacker== 1)
    {
        Console.WriteLine("PEPacking...");
        PEPacker.pack(loader.OutputPath);
    }
}
#elif NET481_OR_GREATER
namespace O_Z_IL2CPP_Security
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
    public class O_Z_UnityProtector
    {
        byte[] metadata_origin;
        List<byte[]> StringLiteraBytes = new List<byte[]>();
        List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();
        public O_Z_UnityProtector()
        {

        }
        void _Crypt(string OpenFilePath,string OutPath ,IL2CPP_Version version,int key)
        {
            IL2CPP_Version ver;
            if (!File.Exists(OpenFilePath))
            {
                throw new Exception("File is not EXISTS!");
            }
            metadata_origin = File.ReadAllBytes(OpenFilePath);
            if (!CheckMetadataFile(metadata_origin))
            {
                throw new Exception("This is not a Metadata File!");
            }

            switch (version)
            {
                case IL2CPP_Version.V24_4:
                    {
                        ver = IL2CPP_Version.V24_4;
                    }
                    break;
                case IL2CPP_Version.V28:
                    {
                        ver = IL2CPP_Version.V28;
                    }
                    break;
                case IL2CPP_Version.V24_1:
                    {
                        ver = IL2CPP_Version.V24_1;
                    }
                    break;
                default: throw new Exception("Metadata Version is not supported or Error Version!");
            }
            object Loader;
            switch (ver)
            {
                case IL2CPP_Version.V24_4:
                    {
                        Loader = new LoadMetadata_v24_4(new MemoryStream(metadata_origin));
                    }
                    break;
                case IL2CPP_Version.V28:
                    {
                        Loader = new LoadMetadata_v28(new MemoryStream(metadata_origin));
                    }
                    break;
                case IL2CPP_Version.V24_1:
                    {
                        Loader = new LoadMetadata_v24_1(new MemoryStream(metadata_origin));
                    }
                    break;
                default: Console.WriteLine("版本错误!请确保你配置了正确且受支持的Metadata版本!"); return;
            }
            Metadata metadata = new Metadata(Loader.GetType().GetField("metadatastream").GetValue(Loader) as Stream, Loader.GetType().GetField("Header").GetValue(Loader).GetType(), Loader.GetType().GetField("Header").GetValue(Loader), ver);
            StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
            StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes, key);
            byte[] allstring = metadata.GetAllStringFromMeta();
            Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, Crypt.CryptWithSkipNULL(allstring, (byte)O_Z_IL2CPP_Security.Tools.CheckNull(key), key), ver);
            byte[] tmp = O_Z_IL2CPP_Security.Tools.StreamToBytes(stream);
            File.WriteAllBytes(OutPath, tmp);
        }
        bool CheckMetadataFile(byte[] data)
        {
            if (BitConverter.ToUInt32(data, 0) != 4205910959)
            {
                return false;
            }
            else
                return true;
        }
        public double CheckVersion(string OpenFilePath)
        {
            byte[] data = File.ReadAllBytes(OpenFilePath);
            if (!CheckMetadataFile(data)) throw new Exception("This is not a Metadata File!");
            MetadataCheck metadataCheck = new MetadataCheck(new MemoryStream(data));
            return metadataCheck.Version;
        }
    }
}
#endif