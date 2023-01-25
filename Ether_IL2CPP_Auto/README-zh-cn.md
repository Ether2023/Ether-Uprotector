# Ether_IL2CPP_Auto

#### 实验性功能, 暂不完善, 敬请期待

自动生成libil2cpp代码, 一键加密global-metadata.dat, 支持绝大多数unity版本

## 保护效果

[Windows Build](./Examples/EtherIl2cppBuild.zip)

## 保护内容

| 功能                                | 支持 |
| ----------------------------------- | ---- |
| `global-metadata`文件加密保护       | ✔️    |
| 隐藏`MetadataHeader`                | ✔️    |
| 混淆`MetadataHeader`顺序            | ✔️    |
| 多指针防止dump出原文件              | ✔️    |
| Windows平台`GameAssembly.dll`防篡改 | ✔️    |
| Android平台`libil2cpp.so`防篡改     |      |
| 混淆`il2cpp-api`                    |      |
| 字符串加密                          |      |
| 常量字符串加密                      |      |

## 使用方法

#### 1.dll调用(推荐)

| 导出函数                                                     | 功能                          |
| ------------------------------------------------------------ | ----------------------------- |
| int get_version()                                            | 获取当前版本                  |
| int get_api_version()                                        | 获取当前调用Api版本           |
| bool process_libil2cpp(string path, string encrypt_config)   | 生成EtherLibil2cpp            |
| bool restore_libil2cpp(string path, string config)           | 恢复libil2cpp                 |
| bool encrypt_win(string game_dir,string game_exe_name, string encrypt_config) | Windows平台加密               |
| bool encrypt_android(string input_apk_unpack, string encrypt_config) | Android平台加密 (需先解压apk) |

我们正在全力开发GUI程序哦,敬请期待

#### 2.使用我们临时制作的UGUI程序 

下载打包版本[Windows](./Examples/EtherIl2cppBuild.zip), 或自行构建Unity工程[UnitySDK](./Examples/EtherIl2cppSDK.zip)

从旧版更新到新版 : 将`EtherIl2cpp.dll`替换`EtherIl2cpp_Data/Plugins/EtherIl2cpp.dll`, 卸载重装EtherIl2cpp


## 支持的Unity版本
unity2017.1.x - 2022.1.x