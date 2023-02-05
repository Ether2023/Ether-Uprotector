using Ether_IL2CPP;
using Ether_IL2CPP.LitJson;
using Ether_Obfuscator.Obfuscators;
using Ether_Obfuscator;
using Spectre.Console;
using Ether_UnityAsset.AssetFile;
using Ether_UnityAsset.AssetFile.Object;
using Ether_Obfuscator.Obfuscators.Unity;
using System.Collections.Generic;

List<byte[]> StringLiteraBytes = new List<byte[]>();
List<byte[]> StringLiteraBytes_Crypted = new List<byte[]>();
string OpenFilePath;
byte[]? metadata_origin = null;

AnsiConsole.Write(new Panel("Ether Uprotector"));
if (!File.Exists("Config.json"))
{
    AnsiConsole.Foreground = ConsoleColor.Red;
    AnsiConsole.WriteLine("Config.json NOT FOUND!");
    AnsiConsole.Foreground = ConsoleColor.Yellow;
    AnsiConsole.WriteLine("Regenerate Config.json...");
    AnsiConsole.Foreground = ConsoleColor.White;
    await Task.Delay(3000);
    _GenerateConfig();
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

switch (args[1])
{
    case "Crypt": _Crypt(); break;
    case "Decrypt": return;
    case "Read": _Read(); break;
    case "Test": _Test(); break;
    case "CheckVersion": CheckVersion(); break;
    case "MonoObfus": MonoObfus(); break;
    default: _default(); break;
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

    switch (jsonManager.index.Version)
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
    Console.WriteLine("Creating Ether Metadata...");
    Metadata metadata = new Metadata(Loader.GetType().GetField("metadatastream").GetValue(Loader) as Stream, Loader.GetType().GetField("Header").GetValue(Loader).GetType(), Loader.GetType().GetField("Header").GetValue(Loader), ver);
    Console.WriteLine("Encrypting StringLiteral...");
    StringLiteraBytes = metadata.GetBytesFromStringLiteral(metadata.stringLiterals);
    Console.WriteLine("Encrypting String...");
    StringLiteraBytes_Crypted = Crypt.Cryptstring(StringLiteraBytes, jsonManager.index.key);
    byte[] allstring = metadata.GetAllStringFromMeta();
    Console.WriteLine("Building new Metadata...");
    Stream stream = metadata.SetCryptedStreamToMetadata(StringLiteraBytes_Crypted, Crypt.CryptWithSkipNULL(allstring, (byte)Ether_IL2CPP.Utils.CheckNull(jsonManager.index.key), jsonManager.index.key), ver);
    byte[] tmp = Ether_IL2CPP.Utils.StreamToBytes(stream);
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
        cpp = new CPP(src, IL2CPP_Version.V24_4, jsonManager.index.key, (byte)Ether_IL2CPP.Utils.CheckNull(jsonManager.index.key));
        File.WriteAllText("Generation/" + jsonManager.index.Version + "/libil2cpp/vm/MetadataCache.cpp", cpp.retsrc);
        File.WriteAllLines("Generation/" + jsonManager.index.Version + "/libil2cpp/il2cpp-metadata.h", File.ReadAllLines("src-res/" + jsonManager.index.Version + "/il2cpp-metadata.h"));
    }
    else if (jsonManager.index.Version == "28")
    {
        src = File.ReadAllText("src-res/" + jsonManager.index.Version + "/GlobalMetadata.cpp");
        cpp = new CPP(src, IL2CPP_Version.V24_4, jsonManager.index.key, (byte)Ether_IL2CPP.Utils.CheckNull(jsonManager.index.key));
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
    Antide4dot antide4Dot = new Antide4dot(loader.Module);
    ControlFlow controlFlow = new ControlFlow(loader.Module,new string[] {"123456"});
    controlFlow.Execute();
    antide4Dot.Execute();
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
    Console.WriteLine("Usage:" + "\n");
    Console.WriteLine("Encrypt:\n    Ether-Uprotector.exe [Input] Crypt [Output]\n");
    Console.WriteLine("Show Metadata Version:\n    Ether-Uprotector.exe [Input] CheckVersion\n");
    Console.WriteLine("Generate KEY Component:\n    Ether-Uprotector.exe Generate\n");
    Console.WriteLine("Obfuscate NET Assembly:\n    Ether-Uprotector.exe [Input] MonoObfus\n");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
void MonoObfus()
{
    List<Obfuscator> obfuscators = new List<Obfuscator>();
    AssemblyLoader loader = new AssemblyLoader(OpenFilePath);
    if (jsonManager.index.Obfus.ControlFlow == 1)
    {
        obfuscators.Add(new ControlFlow(loader.Module, jsonManager.index.Obfus.ignore_ControlFlow_Method));
    }
    if (jsonManager.index.Obfus.Obfusfunc == 1)
    {
        obfuscators.Add(new ObfusFunc(loader.Module,File.ReadAllText("keyfunc.json"),null,true));
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
    if (jsonManager.index.Obfus.AntiDe4dot == 1)
    {
        obfuscators.Add(new Antide4dot(loader.Module));
    }
    if (jsonManager.index.Obfus.FuckILdasm == 1)
    {
        obfuscators.Add(new FuckILdasm(loader.Module));
    }
    if (jsonManager.index.Obfus.MethodError == 1)
    {
        obfuscators.Add(new MethodError(loader.Module));
    }
    foreach (var obfuscator in obfuscators)
    {
        string outstr = obfuscator.ToString();
        int i = outstr.IndexOf("Obfuscators.");
        outstr = outstr.Substring(i + 12, outstr.Length - i - 12);
        Console.WriteLine(outstr + " Executing...");
        obfuscator.Execute();
    }
    loader.Save();
    if (jsonManager.index.Obfus.PEPacker == 1)
    {
        Console.WriteLine("PEPacking...");
        PEPacker.pack(loader.OutputPath);
    }
}

void _GenerateConfig()
{
    Console.Clear();
    AnsiConsole.Write(new Panel("Generate Your Config"));
    AnsiConsole.Write(new Rule("[yellow]KEY[/]").RuleStyle("grey").LeftJustified());
    var key = AnsiConsole.Ask<int>("What's your [green]KEY[/]?");
    AnsiConsole.Write(new Rule("[yellow]MetadataVersion[/]").RuleStyle("grey").LeftJustified());
    var Version = AnsiConsole.Prompt(
                new TextPrompt<string>("What's your [green]MetadataVersion[/]?")
                    .InvalidChoiceMessage("[red]Version error! Please ensure that you have configured the correct and supported metadata version![/]")
                    .DefaultValue("Version?")
                    .AddChoice("24.4")
                    .AddChoice("28"));
    AnsiConsole.Write(new Rule("[yellow]Obfuscator Config[/]").RuleStyle("grey").LeftJustified());
    var Obfuscations = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("Configure Your Obfuscator:")
                    .MoreChoicesText("[grey](Move up and down to reveal more Obfuscations)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to toggle a Obfuscations, [green]<enter>[/] to accept)[/]")
                    .AddChoices(new[]
                    {
                        "ControlFlow","NumObfus","LocalVariables2Field","StrCrypter","Obfusfunc","AntiDe4dot","FuckILdasm","PEPacker","MethodError"
                    }));
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[yellow]Your Config[/]").RuleStyle("grey").LeftJustified());
    AnsiConsole.Write(new Table().AddColumns("[grey]Config[/]", "[grey]Key[/]")
        .RoundedBorder().BorderColor(Spectre.Console.Color.Grey)
        .AddRow("[grey]KEY[/]", key.ToString())
        .AddRow("[grey]MetadataVersion[/]", Version)
        .AddRow("[grey]Obfuscations[/]",ListtoString(Obfuscations)));
    AnsiConsole.Foreground = ConsoleColor.Green;
    AnsiConsole.WriteLine("Generate Successful!");
    AnsiConsole.Foreground = Color.White;
    JsonIndex config = new JsonIndex();
    config.key = key;
    config.Version = Version;
    config.Obfus = new ObfusConfig();
    if (Obfuscations.Contains("ControlFlow"))
        config.Obfus.ControlFlow = 1;
    if (Obfuscations.Contains("NumObfus"))
        config.Obfus.NumObfus = 1;
    if (Obfuscations.Contains("LocalVariables2Field"))
        config.Obfus.LocalVariables2Field = 1;
    if (Obfuscations.Contains("StrCrypter"))
        config.Obfus.StrCrypter = 1;
    if (Obfuscations.Contains("Obfusfunc"))
        config.Obfus.Obfusfunc = 1;
    if (Obfuscations.Contains("AntiDe4dot"))
        config.Obfus.AntiDe4dot = 1;
    if (Obfuscations.Contains("FuckILdasm"))
        config.Obfus.FuckILdasm = 1;
    if (Obfuscations.Contains("PEPacker"))
        config.Obfus.PEPacker = 1;
    if (Obfuscations.Contains("MethodError"))
        config.Obfus.MethodError = 1;
    File.WriteAllText("Config.json", JsonMapper.ToJson(config));

}
string ListtoString(List<string> list)
{
    string ret = "";
    foreach(var str in list)
    {
        ret += (str + " ");
    }
    return ret;
}
void WriteLineColor(string str,Color color)
{
    AnsiConsole.Foreground = color;
    AnsiConsole.WriteLine(str);
    AnsiConsole.Foreground = Color.White;
}