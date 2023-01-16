# Ether Uprotector

![qq](https://img.shields.io/badge/oRangeSumMer(QQ)-2296401259-green) ![qq](https://img.shields.io/badge/Z1029(QQ)-3408708525-green) ![qqgroup](https://img.shields.io/badge/QQGroup-957552913-orange) [![Bilibili](https://img.shields.io/badge/bilibili-%E6%A9%99%E4%B9%8B%E5%A4%8F-blue)](https://space.bilibili.com/79045701) ![email](https://img.shields.io/badge/Email-2286401259%40qq.com-yellowgreen)

![star](https://img.shields.io/github/stars/Z1029-oRangeSumMer/O-Z-Unity-Protector?style=social) ![download](https://img.shields.io/github/downloads/Z1029-oRangeSumMer/O-Z-Unity-Protector/total) ![tag](https://img.shields.io/github/v/tag/Z1029-oRangeSumMer/O-Z-Unity-Protector)

**Ether Uprotector** is a tool providing custom encryption for **Unity**, maintained by **Ether Team**

**The program is based on [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48), requires [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48) environment, please make sure your computer has NET Runtime or NET SDK properly installed**

English | [简体中文](README_zh-cn.md)

## Project lists

- [Ether-IL2CPP](Ether_IL2CPP)
- [Ether-Obfuscator](Ether_Obfuscator)
- [Ether_IL2CPP_Auto](Ether_IL2CPP_Auto)
- [Ether_IL2CPP_GUI](Ether_IL2CPP_GUI)

## What's New

1. The UnityPackge feature has been added to this update. Now you can import our UnityPackge and configure your encryption scheme in Unity's visual interface.
   > ***Now you can enjoy `Ether-IL2CPP` and `Ether-Obfuscator` dual-safety***
   >
   > *We support all versions of Unity after `2018.2`*
   >
   > For datailed usage [Ether_Obfuscator](Ether_Obfuscator)

   ![UnityConfig](pics/Unity%20Config.png)

2. In the `Mono Obfuscate` feature, We added the `ErrorMethod` function, it will prevent Dnspy and other decompilers from working properly (*See below picture*)

|New                       |description|
|--------------------------|----|
|MethodError               |Break Method makes Dnspy and other decompilers unable to restore C # code|

   > after ErrorMethod: 

   > ![ErrorMethod](pics/ErrorMethod.png)

3. Fixed the following bugs

- Fix bug with invalid `Enable` switch

- Error reported in Unity 2022

- Fixed the problem of sometimes throwing `NullReferenceException` when switching scenes in Unity

## How to use `Ether_Obfuscator` and how to deal with problems

1. Download our `UnityPackage` from **Release**
2. Configure `Config.json` and `KeyFunc.json` properly, just as configuring `Ether_Obfuscator` (if you don't know how to configure,please find out in [Ether_Obfuscator](Ether_Obfuscator/README.md))
3. You only need to build projects like normal ones, and O&Z will automatically help you complete the confusion of IL2CPP
4. There are still some bugs in this function. At present, the following points are known:
   - When you use O&Z IL2CPP Obfuscator to build your project, an error may occur in a function (and it may or may not occur during the construction, for example, an error may be reported during the construction of the same project, and it can be compiled in sequence later). This is caused by the random problem of ControlFlow. If you are patient, you can try several times. If you always report an error in a certain function, You can try to add the name of this function in Config.json, which will make ControlFlow skip this function, such as the following:

   - ![err1](pics/err1.png)

   - You can add the HandleShoot function to `ignore_ControlFlow_Method`

   - ![config](pics/config.png)
5. This function can confuse your project before IL2CPP is executed (the strength is equivalent to `Ether_Obfuscator`), which can make your project more secure when building cpp. Even if your program receives attacks like `il2cppdumper`, your methods and fields are still in a confused state, and the code file stored in `il2Cpp` is still confused by functions like `ControlFlow`, which reduces its readability and protects your game security to the greatest extent
   > The best effect can be obtained by cooperating with `Ether-IL2CPP`

## Preview

1. We are trying to modify the `MonoBehave` class to confuse `Class`

## Todo List

1. Encrypt `AssetBundle` resources
2. Modify `IL2CPP` operating mechanism
3. ***We're ready to launch `Unity AssetStore`!***
4. ......

Coming soon awa！

## Special Thanks

The projects and project cases that this project refers to are as follows. Thank you very much to all open source authors!

- [dnlib](https://github.com/0xd4d/dnlib)
- [MindLate](https://github.com/Sato-Isolated/MindLated)
- [ConfuserEx](https://github.com/yck1509/ConfuserEx)
- BeeByte Obfuscation
- [BitMono](https://github.com/sunnamed434/BitMono)
- [BitDotNet(PEPacker)](https://github.com/0x59R11/BitDotNet)

## Contact

If you have any questions or suggestions, you can contact the author's QQ account for feedback.

You can also ask questions directly in issue

Looking forward to your suggestion!

Group：957552913 (QQ)
