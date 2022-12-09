# O-Z-Unity Protector
O&Z Protector 是由 **Z1029[QQ:3408708525]** **和[oRangeSumMer](https://space.bilibili.com/79045701)[QQ:2286401259]** 共同制作的针对Unity进行的客制化和加密

交流群：957552913（QQ）

***本程序基于[Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48)开发，运行需要[Net6.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0) & [NETFramework4.8](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48)环境，请确保您的PC正确安装了NET Runtime或者NET SDK***

## >>>[O&Z-IL2CPP(Click me!)](O%26Z_IL2CPP_Security/README.md)<<<

## >>>[O&Z-MonoObfuscator(Click me!)](O%26Z_Obfuscator/README.md)<<<

## What's New
1. **修复了MonoObfuscate 功能使用过程中无法对路径中含有空格的程序集使用**
2. 在MonoObfuscate功能中，我们添加了对类，方法，字段名的混淆是的代码的不可读性和破解难度上升到了最高，而且此方法可以兼容Unity（需要自定义配置keyfunc.json文件）！
   ![obfusfunc](O%26Z_Obfuscator/img/funcobfus.png)

## 预告
1. UI窗口界面即将完成！
2. 正在测试对于所有unity版本il2cpp的支持,自动生成libil2cpp
3. ~~我们正在编写O&ZMonoObfus的函数与方法名称的混淆~~
4. 我们准备重写Mono虚拟机，在底层对Unity Mono的JIT，AOT等进行加密
5. 我们正在尝试对于IL2CPP生成方式中，在生成IL代码时，插入MonoObfuscate的功能，使得IL2CPP获得最佳保险

## 未来的规划
1. 对AssetBundle资源进行加密
2. 修改IL2CPP的运行机制
3. 对原始Assembly-Csharp.dll进行混淆
4. ***我们正式准备上架Unity AssetStore啦!**
5. . . . . . .

敬请期待 awa！

## 联系作者
如果你有任何问题或者建议，可以联系作者的QQ账号进行反馈哦！

也可以直接在issue提问

期待你的建议！
