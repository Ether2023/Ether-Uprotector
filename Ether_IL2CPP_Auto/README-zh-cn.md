# Ether_IL2CPP_Auto

#### 实验性功能, 暂不完善, 敬请期待

自动生成libil2cpp代码, 一键加密global-metadata.dat, 支持绝大多数unity版本

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
| bool process_libil2cpp(string path, string config)           | 生成EtherLibil2cpp            |
| bool restore_libil2cpp(string path)                          | 恢复libil2cpp                 |
| bool encrypt_win(string game_dir,string game_exe_name, string config) | Windows平台加密               |
| bool encrypt_android(string input_apk_unpack, string config) | Android平台加密 (需先解压apk) |

我们正在全力开发GUI程序哦,敬请期待

#### 2.生成exe使用命令行调用(临时调试使用)
生成exe文件, 通过命令行调用
| 命令行                                                  | 功能                          |
| ------------------------------------------------------- | ----------------------------- |
| `--proclib-p     <libil2cpp_path>`                      | 生成EtherLibil2cpp            |
| `--restorelib-p  <libil2cpp_path> `                     | 恢复libil2cpp                 |
| `--enc-android-p <apk_unpack_path>            <config>` | Android平台加密 (需先解压apk) |
| `--enc-win-p     <game_path>       <exe_name> <config>` | Windows平台加密               |
| `--version`                                             | 获取当前版本                  |
| `--apiversion`                                          | 获取当前调用Api版本           |

## 支持的Unity版本
unity2017.1.x - 2022.1.x