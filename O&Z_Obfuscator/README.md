# O-Z-Mono Obfuscator
本次更新为测试更新,理论上支持任何版本的Mono程序集组件，如果有bug请联系作者

## 使用方法
1. 配置Config文件

   ![Config](img/config.png)
2. 使用参数 **MonoObfus**  来加密你的dll程序集
   ~~~
   O&Z_IL2CPP_Security.exe input MonoObfus
   ~~~
3. Enjoy Safe！ xD
4. 如果你想使用ObfusFunc参数来获取更加强大的混淆，请仔细阅 **读加密参数说明-ObfusFunc** 中的条例

## 加密参数说明
 - ControlFlow(控制流程加密)

   使用本方法将对您的程序函数方法进行流程混淆，不改变执行流程，但是可以打乱编译流程以及IL码的顺序，可以做到干扰破解
   ![ControlFlow](img/controlflow.png)

 - NumObfus(整数预设混淆)
  
   使用本方法将加密您程序集中的所有int类型预设常量（即明文数据，例如int num = 8, 8即为预设常量）
   ![NumObfus](img/numobfus.png)

 - LocalVariables2Field(局部变量混淆为方法)

   使用本方法对您的程序函数的局部变量进行混淆加密，进一步降低代码可读性
   ![LocalVariables2Field](img/localv2f.png)

 - StringCrypt(字符串加密)

   使用本方法加密您游戏内的字符串常量，每一个字符串都单独对应一个单独和密码和单独的解密函数，使破解难度上升
   ![Stringobfus](img/strobfus.png)

 - ObfusFunc(类&方法&字段混淆)

   使用本方法加密您项目中所有的函数，类，甚至是参数，使程序的不可读性达到最高（我们采用了Unity函数名堆积作为字典，使得这种方法混淆的函数难以被反混淆器识别为Obfuscated或JunkFunc）

   ![ObfusFunc](img/funcobfus.png)
   ### 使用方法以及注意事项
   1. 您需要配置 **keyfunc.json**,来完成本程序对于您项目的适配
   ![KeyFunc](img/keyfunc.png)
   2. 在配置keyfunc.json中，我们已经预先配置好了Unity中的大部分生命周期函数以及关键回调，这些都将会作为skip的部分跳过混淆
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
       - 你可以将一些关键的方法(例如涉及到*游戏内购，广告的接入，游戏全局管理控制，游戏资源的管理，游戏本地化保存，与服务器云交互*等等)写到一个不涉及Unity预制体或者UI事件等情况的专用的脚本中，并对该脚本的类进行混淆
    5. 正确的配置好keyfunc，可以最大程度为您的游戏带来安全，谢谢您的使用！如果有任何问题，欢迎联系作者QQ，添加讨论群或者在issue提出
