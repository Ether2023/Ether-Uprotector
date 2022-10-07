//Unity IL2CPP Version 24.5
using O_Z_IL2CPP_Security;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using LitJson;
List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();
string OpenFilePath;
/*
if (!File.Exists("Config.json"))
{
    Json_Config json_Config = new Json_Config();
    json_Config.LastOpenFolder = "";
    json_Config.LastSaveFolder = "";
    json_Config.LastOpenFileName = "";
    json_Config.LastSaveFileName = "";
    File.WriteAllText("Config.json", JsonMapper.ToJson(json_Config));
}
return;
*/
if (args.Length == 0)
{
    
    return;
}
else
{
    OpenFilePath = args[0];
}
Console.WriteLine("O&Z_IL2CPP_Security");
Console.WriteLine("Loading Meatadata:" + OpenFilePath);
byte[]? metadata_origin = null;

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
    Console.WriteLine("Please input your il2cpp version(v24.5/v29):");
    string _Read = Console.ReadLine();
    switch(_Read)
    {
        case "v24.5": ver = IL2CPP_Version.V24_5; break;
        case "v29": ver = IL2CPP_Version.V29; break;
        default: Console.WriteLine("Error!"); return;
    }
    object Loader;
    if (ver == IL2CPP_Version.V24_5)
        Loader = new LoadMetadata_v24_5(new MemoryStream(metadata_origin));
    else if (ver == IL2CPP_Version.V29)
        Loader = new LoadMetadata_v29(new MemoryStream(metadata_origin));
    else
    {
        Console.WriteLine("Input version Error!");
        return;
    }
    Metadata metadata = new Metadata(Loader.GetType().GetField("metadatastream").GetValue(Loader) as Stream, Loader.GetType().GetField("Header").GetValue(Loader).GetType(), Loader.GetType().GetField("Header").GetValue(Loader), ver);
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, CryptB(allstring),ver);
    byte[] tmp = Tools.StreamToBytes(stream);
    File.WriteAllBytes(args[2], tmp); 
    
    
}
void _default()
{
    Console.WriteLine("parameter ERROR!");
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

}
void CheckVersion()
{
    metadata_origin = File.ReadAllBytes(OpenFilePath);
    if (!CheckMetadataFile()) return;
    MetadataCheck metadataCheck = new MetadataCheck(new MemoryStream(metadata_origin));
    Console.WriteLine("Your Metadata Version:" + metadataCheck.Version);
}
byte[] decrypt(byte[] b)
{
    byte[] result = new byte[b.Length];
    for (int i = 0; i < b.Length; i++)
    {
        if (b[i] != 0x52)
            result[i] = (byte)(b[i] ^ 114514);
        else
            result[i] = b[i];
    }
    return result;
}
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
