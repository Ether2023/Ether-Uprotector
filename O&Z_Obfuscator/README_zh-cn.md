# O-Z-Mono Obfuscator
本次更新为测试更新,理论上支持任何版本的Mono程序集组件，如果有bug请联系作者

## 可使用的功能

|                       |说明|
|--------------------------|----|
|ControlFlow               |控制流程加密 |
|NumObfus                  |整数预设混淆|
|LocalVariables2Field      |局部变量混淆为字段|
|StringCrypt               |字符串加密|
|ObfusFunc                 |类&方法&字段混淆|
|Anti De4dot               |使得反混淆器失效|
|Anti Anti-ILDASM          |使得反汇编器失效|
|PEPacker                  |去除NET标志使得ILSpy，DnSpy等软件无法正确识别NET程序集|
|ErrorMethod               |加固方法使得Dnspy等反编译器无法还原您的代码|

> 在下方查看详细介绍以及使用方法
> 
## 使用方法(Unity用户请从第5条开始阅读)
1. 配置Config文件(或者在unity内配置您的方案)
   ![Config](img/config.png)
2. 使用参数 **MonoObfus**  来加密你的dll程序集
   ~~~
   O&Z_IL2CPP_Security.exe input MonoObfus
   ~~~
3. Enjoy Safe！ xD
4. 如果你想使用ObfusFunc参数来获取更加强大的混淆，请仔细阅 **使用方法以及注意事项**
5. 对于Unity用户,我们提供了UnityPackage以供各位使用([下载请点击我](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases))
6. 在unity内配置您的Config.Asset(一般位于插件文件夹的根目录) **(请仔细阅读好下方每一条注意事项)**
   
   ![UConfig](img/Unity%20Config.png)
7. 确定一切正常之后直接**build**,我们提供的脚本会自动帮您完成一切xD

## 使用方法以及注意事项
   1. 您需要配置 **keyfunc.json**,来完成本程序对于您项目的适配,对于unity使用者，你同要可以配置Config.Asset文件对本程序进行配置
   ![KeyFunc](img/keyfunc.png)
   2. 在配置keyfunc.json中，我们已经预先配置好了Unity中的大部分生命周期函数以及关键回调，这些都将会作为skip的部分跳过混淆 **（我们在Unity的Config.Asset同样配置好了这些）**
   3. 我们为您提供了三种可以自定义的部分，用于适配您的程序
       - 忽略的方法(customignoreMethod): **此项采用白名单的模式**，如果您不需要混淆某个方法或者某个方法不能被混淆(例如使用了反射)，可以将方法名称添加到这里
       - 忽略的字段(customignoreField): **此项采用白名单的模式**，如果您不需要混淆某个字段或者某个字段不能被混淆(例如使用了反射)，可以将字段名称添加到这里
       - 需要混淆的类(customignoreClass): **此项采用黑名单的模式**,因为涉及到Unity预制体的影响,类名一般来说不能够轻易混淆(*详细原因见下方*),如果你需要混淆某个类名，可以将类名添加到这里
   4. **需要注意的事项**
       - 在Unity中，GameObject或者prefabs初始绑定了脚本，则该脚本的类名不可混淆，方法名和字段名可以混淆
       - 在Unity中，GameObject或者prefabs初始没有绑定脚本，但是在代码中动态添加了脚本，则该脚本的类名、方法名和字段名都可以混淆 
       - 如果该脚本中涉及到了UI的事件响应(如Button.OnClick),则该脚本的类名和该方法名都不可混淆,字段名可以混淆
       - Unity的生命周期方法和回调方法不能混淆,上方的忽略列表包含了大多数常用的生命周期和回调方法，如果有遗漏，可以自行添加
       - Unity中的Invoke等特殊方法所调用的函数方法不可混淆,同理协程类的方法也不可混淆,请自行添加到自定义忽略列表
       - 部分涉及反射类的代码不能混淆,如System.Reflection(GetField,GetMethod,Invoke等),请自行添加到自定义忽略列表
       - Native层里直接调用C#或通过Unity内置API发送事件到C#的类和方法不可混淆(大多数在移动平台中)
       - 一些特殊插件对应的脚本不可混淆，例如xLua和与之绑定的C#脚本
       - 你可以将一些关键的方法(例如涉及到*游戏内购，广告的接入，游戏全局管理控制，游戏资源的管理，游戏本地化保存，与服务器云交互*等等)写到一个不涉及Unity预制体或者UI事件等情况的专用的脚本中，并对该脚本的类进混淆
   5. 正确的配置好keyfunc，可以最大程度为您的游戏带来安全，谢谢您的使用！如果有任何问题，欢迎联系作者QQ，添加讨论群或者在issue提出
   6. 关于PEPacker功能,我们目前暂时仅对Windows平台的Mono编译提供自动加固(Android平台或者其他平台的Mono编译请自行使用PEPack功能对程序集进行加固，**同时记得对您的apk进行二次签名**)
   
      > Android平台需要先对apk进行解压，找到Assmebly-CSharp.dll，使用手动的方法对它进行加固，同时二次打包您的apk并且签名
       ![PEPack](img/pepackdo.png)

## 关于O&Z IL2CPP Obfuscator的使用方法以及问题处理
1. 从[Release](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases)中下载我们的unitypackage
2. 正确的配置**Config.json**和**KeyFunc.json**,就像你配置O&Z-MonoObfuscator那样(如不明白配置,可以跳转到[O&Z-MonoObfuscator](O%26Z_Obfuscator/README.md)了解)
3. 您只需要像正常的构建生成项目一样，O&Z 会自动帮你完成IL2CPP的混淆
4. 本功能还存在一些bug，目前已知有以下几点
   - 使用O&Z IL2CPP Obfuscator构建您的项目时，可能会出现某一个函数的报错(而且是构建时可能发生，也可能不发生，例如同一工程构建时候报错了，过一会又可以顺序编译了)，这是由于ControlFlow的随机问题造成的，如果您有耐心可以多尝试几次，如果一直在某一个报错，可以尝试在Config.json中添加该函数的名称，这样会让ControFlow跳过这个函数,例如以下这种情况
   ![err1](img/err1.png)
   可以将HandleShoot函数添加到**ignore_ControlFlow_Method**中
   ![config](img/config.png)
5. 本功能可以在IL2CPP执行之前对您的项目进行一次混淆(强度等同于O&Z Monoobfus)，可以使您的项目构建cpp时更加安全，即使您的程序收到了il2cppdumper之类的攻击，您的方法和字段任然处于混淆状态，而且il2cpp储存的代码文件任然被ControlFlow等功能混淆，使其可读性降低，最大程度保护您的游戏安全
   > 配合**O&Z-IL2CPP**可以获得最佳效果哦

## 加密参数说明
 - ControlFlow(控制流程加密)

   使用本方法将对您的程序函数方法进行流程混淆，不改变执行流程，但是可以打乱编译流程以及IL码的顺序，可以做到干扰破解
   ![ControlFlow](img/controlflow.png)

 - NumObfus(整数预设混淆)
  
   使用本方法将加密您程序集中的所有int类型预设常量（即明文数据，例如int num = 8, 8即为预设常量）
   ![NumObfus](img/numobfus.png)

 - LocalVariables2Field(局部变量混淆为字段)

   使用本方法对您的程序函数的局部变量进行混淆加密，进一步降低代码可读性
   ![LocalVariables2Field](img/localv2f.png)

 - StringCrypt(字符串加密)

   使用本方法加密您游戏内的字符串常量，每一个字符串都单独对应一个单独和密码和单独的解密函数，使破解难度上升
   ![Stringobfus](img/strobfus.png)

 - ObfusFunc(类&方法&字段混淆)

   使用本方法加密您项目中所有的函数，类，甚至是参数，使程序的不可读性达到最高（我们采用了Unity函数名堆积作为字典，使得这种方法混淆的函数难以被反混淆器识别为Obfuscated或JunkFunc）

   ![ObfusFunc](img/funcobfus.png)

 - Anti-De4dot(侦测de4dot)

   使用本方法加固您的程序集，使得de4dot无法对齐完成还原以及反混淆

   ![Anti-de4dot](img/Antide4.png)

 - Anti-ILDASM(反反汇编器)
   
   使用本方法，通过MS提供的SuppressIldasmAttribute使反编译器无法正常工作

 - PEPacker(封装程序集,仅对mono编译方式有用)
   
   使用本方法，封装NET程序集，去除NET符号，使得反编译器无法正确识别NET程序集

  ![PEPacker](img/pepack.png)

  - ErrorMethod(加固方法)
  
   使用本方法将加固您的方法使得Dnspy等反编译软件无法还原您的代码

   ![ErrorMethod](img/ErrorMethod.png)