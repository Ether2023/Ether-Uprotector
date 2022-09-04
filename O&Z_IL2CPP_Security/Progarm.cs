//Unity IL2CPP Version 24.5
using OrangeIL2CPP_Pro;
using System.Text;
//stringLiterals
List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();

//imageName
List<byte[]> imageNameStrings = new List<byte[]>();
List<byte[]> imageNameStrings_Crypted = new List<byte[]>();
//Test();
Console.WriteLine("OrangeIL2CPP");
Console.WriteLine("Loading Meatadata:" + args[0]);
if (!File.Exists(args[0]))
{
    Console.WriteLine("File is not EXISTS!");
    return;
}
byte[] metadata_origin = File.ReadAllBytes(args[0]);
CheckMetadataFile();
switch(args[1])
{
    case "Crypt":_Crypt();break;
    case "Decrypt":return;
    case "Read":_Read();break;
    case "Test":_Test();break;
    default:_default();break;
}
return;
void _Crypt()
{
    Metadata metadata = new Metadata(new MemoryStream(metadata_origin));
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes);
    /*
    for (int i = 0; i < StringLiteraBytes_Crypted.Count; i++)
    {
        Console.WriteLine("[" + i + "]Origin:" + Encoding.UTF8.GetString(StringLiteraBytes[i]) + " Crypted:" + Encoding.UTF8.GetString(StringLiteraBytes_Crypted[i]));
    }
    */
    /*
    imageNameStrings = metadata.GetImageStringsFromImageDefinitions(metadata.imageDefinitions);
    imageNameStrings_Crypted = Crypt.Cryptstring(imageNameStrings);

    for (int i = 0; i < imageNameStrings_Crypted.Count; i++)
    {
        Console.WriteLine("[" + i + "]Origin:" + Encoding.UTF8.GetString(imageNameStrings[i]) + " Crypted:" + Encoding.UTF8.GetString(imageNameStrings_Crypted[i]));
    }
    */
    byte[] allstring = metadata.GetAllStringFromMeta();

    metadata.SetCryptedStringToMetadata(StringLiteraBytes_Crypted, CryptB(allstring), args[2]);
    
    return;
}
void _default()
{
    Console.WriteLine("parameter ERROR!");
    return;
}
void _Read()
{
    Metadata metadata = new Metadata(new MemoryStream(metadata_origin));
    MetadataHeader header = metadata.GetHeader();
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Console.WriteLine("[StringLitera]Count:"+ StringLiteraBytes.Count+"Baseoffset:"+header.stringLiteralDataOffset);
    for (int i = 0;i<StringLiteraBytes.Count; i++)
    {
        Console.WriteLine("[" +  i + "]" + Encoding.Default.GetString(StringLiteraBytes[i]));
    }
    Console.WriteLine(Encoding.UTF8.GetString(allstring));

    return;
}
void CheckMetadataFile()
{
    if (BitConverter.ToUInt32(metadata_origin, 0) != 4205910959)
    {
        Console.WriteLine("File is not a Meatadata file!");
        return;
    }
}
void _Test()
{
    /*
    Metadata metadata_o = new Metadata(new MemoryStream(metadata_origin));
    Metadata metadata = new Metadata(new MemoryStream(File.ReadAllBytes("global-metadata.dat.crypted")));
    byte[] str = metadata.GetStringFromIndex(184);
    Console.WriteLine(Encoding.UTF8.GetString(decrypt(str)));
    Console.WriteLine(Encoding.UTF8.GetString(metadata_o.GetStringFromIndex(184)));
    byte a = 0x52;
    byte b = (byte)(a ^ 114514);
    Console.WriteLine(b);
    for(int i = 0;i<255;i++)
    {
        Console.WriteLine((byte)(i^114514));
    }
    */
    byte[] str = Encoding.UTF8.GetBytes("global-metadata.dat");
    foreach (byte b in str)
    {
        Console.Write((byte)(b^114514)); Console.Write(",");
    }
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
/*
byte[] tmp = BitConverter.GetBytes(sanity + 1);
Console.WriteLine("Header-sanity-crypted:" + tmp[0].ToString("X") + " " + tmp[1].ToString("X") + " " + tmp[2].ToString("X") + " " + tmp[3].ToString("X"));
*/
/*
byte[] data_crypted = Crypt(meatadata_origin);
File.WriteAllBytes("global-metadata-crypted.dat", data_crypted);
Console.WriteLine("Cpryted Meatadata:global-metadata-crypted.dat");


Console.WriteLine("Done");

byte[] Crypt(byte[] data)
{
    byte[] tmp = BitConverter.GetBytes(sanity + 1);
    byte[] rtdata = data;
    rtdata[0] = tmp[0];
    rtdata[1] = tmp[1];
    rtdata[2] = tmp[2];
    rtdata[3] = tmp[3];
    Console.WriteLine("Header-sanity-crypted:" + rtdata[0].ToString("X") + " " + rtdata[1].ToString("X") + " " + rtdata[2].ToString("X") + " " + rtdata[3].ToString("X"));
    return rtdata;
}
*/