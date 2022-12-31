# O-Z-Unity Protector

O&Z Protector is a tool providing custom encryption for Unity, maintained by **Z1029 [QQ:3408708525]** and **[oRangeSumMer](https://space.bilibili.com/79045701) [QQ:2286401259]**

**The program is based on [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48), requires [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48) environment, please make sure your computer has NET Runtime or NET SDK properly installed**

中文版本Readme请戳[这里](README_zh-cn.md)

> ***Some documents have not been translated yet, so please look forward to it.***

## Project lists

- [O&Z-IL2CPP](OZ_IL2CPP_GUI)
- [O&Z-MonoObfuscator](O%26Z_Obfuscator)
- [O&Z-Auto Generate Console](OZ_Il2cpp_Console)

## What's New (**O&Z IL2CPP Obfuscator Major Update!** && O&Z_Il2cpp_Console Released!)

1. The [UnityPackge](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases) feature has been added to this update. Now you can import our UnityPackge and configure your encryption scheme in Unity's visual interface.
   > ***Now you can enjoy `O&Z-IL2CPP` and `O&Z-MonoObfuscator` dual-safety***
   >
   > For datailed usage [O&Z-MonoObfuscator](O%26Z_Obfuscator)

   ![UnityConfig](pics/Unity%20Config.png)

2. We have lowered the minimum version requirement of Unity, and now our obfuscator can support all versions after 2018.2

   > You can use **O&Z IL2CPP Obfuscator** even if the O&Z-IL2CPP has not yet adapted to the Unity version you are using, because O&Z IL2CPP Obfuscator currently supports all Unity versions after **2018.2**

3. In the `Mono Obfuscate` feature, we added the detection of the `De4dot antiobfuscator`. Configuring the `Anti-de4dot` feature prevents your assembly from being restored by `de4dot`! `PEPack` is also included, which removes your NET assembly flag and makes your assembly unrecognized by the decompiler (*See below picture*)

|New                       |Description|
|--------------------------|----|
|Anti De4dot               |Invalidate anti-confuser|
|Anti Anti-ILDASM          |Invalidate disassembler|
|PEPacker                  |Removing the NET flag makes ILSpy, DnSpy and other software unable to correctly recognize the NET assembly|

   > After de4dot:
   >
   > ![De4dot](pics/Antide4.png)
   >
   > After PEPacker
   >
   > ![PEPack](pics/pepack.png)

1. For `OZ_ Il2cpp_ Console`We have successfully developed the `Auto-Genertate IL2cpp` feature and have been successful (temporarily in beta), now you can use `O&Z IL2CPP` to encrypt any version of Unity!

   **Support Unity version**

   | Il2Cpp Version | Unity Version                | Support        |
   | -------------- | ---------------------------- |--------------  |
   | 24.0           | 2017.x - 2018.2.x            | ✔️             |
   | 24.1           | 2018.3.x - 2018.4.x          | ✔️             |
   | 24.2           | 2019.1.x - 2019.2.x          | ✔️             |
   | 24.3           | 2019.3.x, 2019.4.x, 2020.1.x | ✔️             |
   | 24.4           | 2019.4.x and 2020.1.x        | ✔️             |
   | 27.0           | 2021.2.x                     | ✔️             |
   | 27.1           | 2020.2.x - 2020.3.x          | ✔️             |
   | 27.2           | 2021.1.x, 2021.2.x           | ✔️             |
   | 28             | 2021.3.x, 2022.1.x           | ✔️             |

   > `OZ_Il2cpp_Console` supports any original version of Unity (unmodified)
   >
   > If there are incompatibilities or bugs with your Unity version, welcome to propose issue or contact z9
   >
   > **Before testing this function, don't forget to manually back up your `libil2cpp` file to avoid inconvenience xD**

## How to use `O&Z IL2CPP Obfuscator` and how to deal with problems

1. Download our `UnityPackage` from [release](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases)
2. Configure `Config.json` and `KeyFunc.json` properly, just as configuring `O&Z-MonoObfuscator` (if you don't know how to configure,please find out in [O&Z-MonoObfuscator](O%26Z_Obfuscator/README.md))
3. You only need to build projects like normal ones, and O&Z will automatically help you complete the confusion of IL2CPP
4. There are still some bugs in this function. At present, the following points are known:
   - When you use O&Z IL2CPP Obfuscator to build your project, an error may occur in a function (and it may or may not occur during the construction, for example, an error may be reported during the construction of the same project, and it can be compiled in sequence later). This is caused by the random problem of ControlFlow. If you are patient, you can try several times. If you always report an error in a certain function, You can try to add the name of this function in Config.json, which will make ControlFlow skip this function, such as the following:

   - ![err1](pics/err1.png)

   - You can add the HandleShoot function to `ignore_ControlFlow_Method`

   - ![config](pics/config.png)
5. This function can confuse your project before IL2CPP is executed (the strength is equivalent to `O&Z Monoobfus`), which can make your project more secure when building cpp. Even if your program receives attacks like `il2cppdumper`, your methods and fields are still in a confused state, and the code file stored in `il2Cpp` is still confused by functions like `ControlFlow`, which reduces its readability and protects your game security to the greatest extent
   > The best effect can be obtained by cooperating with `O&Z-IL2CPP`

## Preview

1. ~~UI window interface is almost complete!~~
2. ~~Testing support for all unit versions of `il2cpp`, automatically generating `libil2cpp`~~
3. *We are going to rewrite the Mono virtual machine to encrypt Unity Mono's JIT, AOT, etc. at the bottom*

## Todo List

1. Encrypt `AssetBundle` resources
2. Modify `IL2CPP` operating mechanism
3. We are trying to modify the `MonoBehave` class to confuse `Class`
4. ***We're ready to launch `Unity AssetStore`!***
5. ......

Coming soon awa！

## Special Thanks

The projects and project cases that this project refers to are as follows. Thank you very much to all open source authors!

- [dnlib](https://github.com/0xd4d/dnlib)
- [MindLate](https://github.com/Sato-Isolated/MindLated)
- [ConfuserEx](https://github.com/yck1509/ConfuserEx)
- BeeByte Obfuscation
- BitMono
- [BitDotNet(PEPacker)](https://github.com/0x59R11/BitDotNet)

## Contact

If you have any questions or suggestions, you can contact the author's QQ account for feedback.

You can also ask questions directly in issue

Looking forward to your suggestion!

Group：957552913 (QQ)
