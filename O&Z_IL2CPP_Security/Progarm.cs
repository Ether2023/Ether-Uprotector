//Unity IL2CPP Version 24.5
using O_Z_IL2CPP_Security;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using LitJson;
List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();
string OpenFilePath;
byte[]? metadata_origin = null;
if(!File.Exists("Config.json"))
{
    Console.WriteLine("Config.json not found!");
    Console.WriteLine("正在生成默认配置文件...");
    JsonIndex index = new JsonIndex()
    {
        key = 114514,
        Version = "24.4"
        
    };
    File.WriteAllText("Config.json", JsonMapper.ToJson(index));
    if (File.Exists("Config.json")) Console.WriteLine("已重新生成默认配置文件,..Done!");
}
JsonManager jsonManager = new JsonManager("Config.json");
if (args.Length == 0)
{
    Help();
    return;
}
else
{
    OpenFilePath = args[0];
}
Console.WriteLine("O&Z_IL2CPP_Security");
Console.WriteLine("Loading Meatadata:" + OpenFilePath);

switch(args[1])
{
    case "Crypt":_Crypt();break;
    case "Decrypt":return;
    case "Read":_Read();break;
    case "Test":_Test();break;
    case "CheckVersion": CheckVersion(); break;
    default:_default();break;
}
return;
void _Crypt()
{
    IL2CPP_Version ver;
    if (!File.Exists(OpenFilePath))
    {
        Console.WriteLine("File is not EXISTS!");
        return;
    }
    metadata_origin = File.ReadAllBytes(OpenFilePath);
    if (!CheckMetadataFile()) return;
    jsonManager = new JsonManager("Config.json");
    
    switch(jsonManager.index.Version)
    {
        case "24.4": ver = IL2CPP_Version.V24_4; break;
        case "28": ver = IL2CPP_Version.V28; break;
        default: Console.WriteLine("Error!"); return;
    }
    object Loader;
    if (ver == IL2CPP_Version.V24_4)
        Loader = new LoadMetadata_v24_5(new MemoryStream(metadata_origin));
    else if (ver == IL2CPP_Version.V28)
        Loader = new LoadMetadata_v28(new MemoryStream(metadata_origin));
    else
    {
        Console.WriteLine("Input version Error!");
        return;
    }
    Metadata metadata = new Metadata(Loader.GetType().GetField("metadatastream").GetValue(Loader) as Stream, Loader.GetType().GetField("Header").GetValue(Loader).GetType(), Loader.GetType().GetField("Header").GetValue(Loader), ver);
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, CryptB(allstring,(byte)Tools.CheckNull(jsonManager.index.key), jsonManager.index.key),ver);
    byte[] tmp = Tools.StreamToBytes(stream);
    File.WriteAllBytes(args[2], tmp); 
    
    
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
    jsonManager = new JsonManager("Config.json");
    Console.WriteLine("Your Password is: " + jsonManager.index.key);
    Console.WriteLine("Your MetadataVersion is: " + jsonManager.index.Version);
    Console.WriteLine((byte)Tools.CheckNull(114514));
}
void CheckVersion()
{
    metadata_origin = File.ReadAllBytes(OpenFilePath);
    if (!CheckMetadataFile()) return;
    MetadataCheck metadataCheck = new MetadataCheck(new MemoryStream(metadata_origin));
    Console.WriteLine("Your Metadata Version:" + metadataCheck.Version);
}
byte[] CryptB(byte[] b,byte skip,int key)
{
    byte[] result = new byte[b.Length];
    for (int i = 0; i < b.Length; i++)
    {
        if (b[i] != 0 && b[i] != skip)
            result[i] = (byte)(b[i] ^ key);
        else
            result[i] = b[i];
    }
    return result;
}
/*
byte[] CryptB(byte[] b)
{
    byte[] result = new byte[b.Length];
    for (int i = 0; i < b.Length; i++)
    {
        if (b[i] != 0 && b[i] != 0x52)
            result[i] = (byte)(b[i] ^ 114514);
        else
            result[i] = b[i];
    }
    return result;
}
*/
void Help()
{
    Console.WriteLine("O&Z_IL2CPP_Security使用方法:"+"\n");
    Console.WriteLine("O&Z_IL2CPP_Security.exe [文件路径] [参数] *[输出路径]");
    Console.WriteLine("参数: Crypt:加密 CheckVersion:检查版本");
}
