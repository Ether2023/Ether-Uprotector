# Ether Uprotector

![Ether-Uprotector](https://socialify.git.ci/Ether2023/Ether-Uprotector/image?description=1&font=Inter&forks=1&issues=1&logo=https%3A%2F%2Fraw.githubusercontent.com%2FEther2023%2FEther-Uprotector%2Fmain%2Flogo.png&name=1&owner=1&pattern=Signal&pulls=1&stargazers=1&theme=Light)

![qq](https://img.shields.io/badge/oRangeSumMer(QQ)-2286401259-green) ![qq](https://img.shields.io/badge/Z1029(QQ)-3408708525-green) ![qqgroup](https://img.shields.io/badge/QQGroup-957552913-orange) [![Bilibili](https://img.shields.io/badge/bilibili-%E6%A9%99%E4%B9%8B%E5%A4%8F-blue)](https://space.bilibili.com/79045701) ![email](https://img.shields.io/badge/Email-2286401259%40qq.com-yellowgreen)

**Ether Uprotector** is a tool providing custom encryption for **Unity**, maintained by **Ether Team**

**The program is based on [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48), requires [Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48) environment, please make sure your computer has NET Runtime or NET SDK properly installed**

English | [简体中文](README_zh-cn.md)

## Project List
- [Ether_Obfuscator](Ether_Obfuscator)
- [Ether_IL2CPP_Auto](Ether_IL2CPP_Auto)
- ~~[[Obsolete]Ether_IL2CPP_GUI](Ether_IL2CPP_GUI)~~
- ~~[[Obsolete]Ether-IL2CPP](Ether_IL2CPP)~~

## How to Start ?
[Click Me](Ether_Obfuscator) to learn how to properly configure and use the Ether Upprotector

## What's New
1. The new UI configuration interface makes your configuration easier
   
2. **Automatically analyze** the project and generate the method name and field name to be ignored according to the project generation configuration (including `GUI`, `Animation` and `Reflection`)

   ![newui](pics/newui.png)
 
3. Support the confusion of `Monobehaviour`

   ![monoobfus](pics/obfusmono.png)

4. `EtherIl2CPP` still has some bugs, we will fix them and release EtherIl2cpp at `v1.6std2`

## How to use deal with problems in `Ether_Obfuscator`

1. There may be a bug in the `ControlFlow` function. If Unity throws an exception, you can try to **close the `ControlFlow` function**
2. If an individual method throws an exception during the IL2CPP construction process

    ![err1](pics/err1.png)

    You can add the HandleShoot function to `ignore_ControlFlow_Method`

   ![config](pics/cfignore.png)

## Preview

1. We are planning and making `AssetProtection`

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
- OPS Obfuscator

## Contact

If you have any questions or suggestions, you can contact the author's QQ account for feedback.

You can also ask questions directly in issue

Looking forward to your suggestion!

Group：957552913 (QQ)
