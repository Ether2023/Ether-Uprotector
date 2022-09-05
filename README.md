# O-Z-IL2CPP
O&Z IL2cpp 是由 **Z1029[QQ:3408708525]** **和OrangeSummer[QQ:2286401259]** 共同制作的针对Unity IL2CPP编译进行的客制化和加密

本项目目前只针对 **Unity 2019.4.32f1 (64-bit)** (IL2CPP Version:24.4)起作用，之后会逐步适配其他版本的unity engine

## 加密流程
1. 加密Metadata内的String部分防止关键的类和方法名被获取，这同样适用于防止IL2CPPDumper的攻击
2. 加密Metadata内的StringLiteral部分，防止您的游戏文本或者字符串密钥等关键字符串受到攻击
3. 混淆了IL2CPP Header使得大部分Dumper软件无法正确识别Metada (未实现)，且该版本暂不可用

## 加密效果
Il2CPP Dumper测试效果
![IL2CPPTest](Asset/il2cppdumpertest.png "IL2CPPDumper测试")
Dump.cs展示
![dump.cs](Asset/dump.cs.png "dump.cs")

## 使用方法
1. 编译出VS工程
2. 首先对Unity工程进行一次生成，得到原始的 **globa-metadata.dat** 文件 *(一般位于 你的项目名称_Data\il2cpp_data\Metadata\ 文件夹内)*
3. 在生成的可执行文件的文件夹内使用命令行，你将得到加密后的Metadata文件

~~~
"OrangeIL2CPP Pro.exe" 原始global-metadata.dat文件路径 Crypt 加密后输出文件的路径

例如 "OrangeIL2CPP Pro.exe" "global-metadata.dat" Crypt "global-metadata.dat.crypted"
~~~
4. 使用 *\Unity il2cpp code\Unity 2019.4.32f1 (64-bit)\MetadataCache.cpp* 替换掉Unity安装目录下的同名文件 *(\Unity 2019.4.32f1\Editor\Data\il2cpp\libil2cpp\vm\MetadataCache.cpp)*
5. 再次启动Unity，重新生成一遍需要加密的项目
6. 使用加密后的 Metadata 文件替换掉新生成项目下的 **globa-metadata.dat**文件
7. 享受**O&Z IL2cpp**给你带来的安全! :D

## 更新预告
在下一个版本中，我们会做出如下更新
1. 打乱Metadata Header并且对其做出加密
2. 将Metadata整体进行加密
3. 优化代码性能
4. ......

敬请期待 awa！

## 联系作者
如果你有任何问题或者建议，可以联系作者的QQ账号进行反馈哦！


期待你的建议！