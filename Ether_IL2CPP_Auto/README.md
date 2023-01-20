# Ether_IL2CPP_Auto

中文说明请戳[这里](README-zh-cn.md)

#### Experimental function, not perfect yet, please wait

Automatically generate `libil2cpp code`, encrypt `global-metadata.dat` with one key, and support most unity versions

## Protect Content

|Function                       |Support|
|---------------------------|----|
|`global-metadata` file encryption protection|✔️   |
|Hide `MetadataHeader`         |✔️   |
|Confusing `MetadataHeader` order     |✔️   |
|Multiple pointers prevent dump from leaving the original file     |✔️   |
|`GameAssembly.dll` check sum for Windows |✔️ |
|`libil2cpp.so`check sum for Android | |
|Obfuscate `il2cpp-api` | |
|`String` encryption                 |    |
|`StringLiteral` Encryption             |    |

## Usage

#### 1.Use dllexport api (recommended)

| API                                                          | Function                                                 |
| ------------------------------------------------------------ | -------------------------------------------------------- |
| int get_version()                                            | Get current version                                      |
| int get_api_version()                                        | Get current api version                                  |
| bool process_libil2cpp(string path, string config)           | Generate `EtherLibil2cpp`                                |
| bool restore_libil2cpp(string path)                          | Restore to original `libil2cpp`                          |
| bool encrypt_win(string game_dir,string game_exe_name, string config) | Encrypt game for `Windows`                               |
| bool encrypt_android(string input_apk_unpack, string config) | Encrypt game for `Android `(Unpack apk file is required) |

We're working on GUI programs. Coming soon~

#### Use command line calls 

| Command                                                 | Function                                                |
| ------------------------------------------------------- | ------------------------------------------------------- |
| `--proclib-p     <libil2cpp_path>`                      | Generate `EtherLibil2cpp`                               |
| `--restorelib-p  <libil2cpp_path> `                     | Restore to original `libil2cpp`                         |
| `--enc-android-p <apk_unpack_path>            <config>` | Encrypt game for `Android`(Unpack apk file is required) |
| `--enc-win-p     <game_path>       <exe_name> <config>` | Encrypt game for `Windows`                              |
| `--version`                                             | Get current version                                     |
| `--apiversion`                                          | Get current api version                                 |

## Supported Unity Version

unity2017.1.x - 2022.1.x