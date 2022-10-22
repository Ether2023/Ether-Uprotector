# O-Z-IL2CPP
O&Z IL2cpp 是由 **Z1029[QQ:3408708525]** **和[oRangeSumMer](https://space.bilibili.com/79045701)[QQ:2286401259]** 共同制作的针对Unity IL2CPP编译进行的客制化和加密

交流群：957552913（QQ）
# Support Unity Version

| Il2Cpp Version | Unity Version                | Support        |
| -------------- | ---------------------------- |--------------  |   
| 24.0           | 2017.x - 2018.2.x            |                |
| 24.1           | 2018.3.x - 2018.4.x          |                |
| 24.2           | 2019.1.x - 2019.2.x          |                |
| 24.3           | 2019.3.x, 2019.4.x, 2020.1.x |✔️             |
| 24.4           | 2019.4.x and 2020.1.x        |✔️             |
| 27.0           | 2021.2.x                     |                |
| 27.1           | 2020.2.x - 2020.3.x          |                |
| 27.2           | 2021.1.x, 2021.2.x           |                |
| 28             | 2021.3.x, 2022.1.x           |✔️             |

如果需要了解您使用的unity Metadata版本，可以使用CheckVersion参数来查看您的Metadata版本

***如果你想让我们添加对您使用的Unity版本的支持，可以联系作者QQ哦***

~~~
O&Z_IL2CPP_Security InputMetadataFilePath CheckVersion

例如:
O&Z_IL2CPP_Security global-metadata.dat CheckVersion
~~~

![Version](Asset/CheckVersion.png "IL2CPP版本")


## What's New
1.新增了对Metadata版本检测功能


## 预告
1. ~~我们会在之后的版本中提供检测您的IL2CPP_Version的功能（coming soon！）~~
2. 我们会在近期更新中重构IL2CPP的结构体
3. ***我们正在开发UI版本以及Json配置等功能(可以自定义加密流程，以及密钥等)(coming soon!)***

预告指的是在近期版本会更新的条目，更多长期目标可以查看下文的未来规划 ***qwq***
## 加密流程
1. 我们重新定义和声明了新的Header并且将他们加密后隐藏在了文件之中使得破解者获取获取原始Header的过程变得困难（此次改动基于第4条的混淆Header）
2. 加密Metadata内的String部分防止关键的类和方法名被获取，这同样适用于防止IL2CPPDumper的攻击（即使头部的混淆失效，同样可以提供二次保护）
3. 加密Metadata内的StringLiteral部分，防止您的游戏文本或者字符串密钥等关键字符串受到攻击
4. 我们混淆了Header并且隐藏了sanity和verison等关键参数，使得IL2CPPDumper等软件无法正确识别Metadata文件


## 加密效果
Il2CPP Dumper测试效果

![IL2CPPTest](Asset/il2cppdumpertest2.png "IL2CPPDumper测试")

模拟受到攻击,攻击者还原头部之后

![IL2CPPTest](Asset/il2cppdumpertest.png "IL2CPPDumper测试")

还原头部后由IL2CPPDumper获取的Dump.cs展示

![dump.cs](Asset/dump.cs.png "dump.cs")

Origin_Header

![OriginHeader](Asset/Header.png "Origin Header")

O&Z_Header

![O&Z_Header](Asset/FrontHeader.png "After Crypted Header")

## 使用方法
1. 编译出VS工程,或者直接下载[Release](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases)内的exe程序
2. 首先对Unity工程进行一次生成，得到原始的 **globa-metadata.dat** 文件 *(一般位于 你的项目名称_Data\il2cpp_data\Metadata\ 文件夹内)*
3. 在生成的可执行文件的文件夹内使用命令行，并且正确输入您的IL2CPP版本,你将得到加密后的Metadata文件

~~~
"O&Z_IL2CPP_Security" 原始global-metadata.dat文件路径 Crypt 加密后输出文件的路径

例如 "O&Z_IL2CPP_Security" "global-metadata.dat" Crypt "global-metadata.dat.crypted"

Please input your il2cpp version(v24.5/v29):
v24

~~~
4. 使用 **libil2cpp** 覆盖Unity安装目录下的同名文件夹 **\Unity XXXXX\Editor\Data\il2cpp\libil2cpp\\**
5. 再次启动Unity，重新生成一遍需要加密的项目
6. 使用加密后的 Metadata 文件替换掉新生成项目下的 **globa-metadata.dat**文件
7. 享受**O&Z IL2cpp**给你带来的安全! :D

## 未来的规划
1. ~~重定位Header在Metadata中的位置（甚至是重构Metadata Loader System）~~
2. ~~将Metadata整体进行加密~~
3. 对AssetBundle资源进行加密
4. 修改IL2CPP的运行机制
5. 对原始Assembly-Csharp.dll进行混淆
6. . . . . . .

敬请期待 awa！

## 联系作者
如果你有任何问题或者建议，可以联系作者的QQ账号进行反馈哦！

也可以直接在issue提问

期待你的建议！
