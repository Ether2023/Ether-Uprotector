# Ether_IL2CPP_Auto

中文说明请戳[这里](README-zh-cn.md)

#### Experimental function, not perfect yet, please wait

Automatically generate `libil2cpp code`, encrypt `global-metadata.dat` with one key, and support most unity versions

## Protect Sample

[Windows Build](./Examples/EtherIl2cppBuild.zip)

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

#### 1.Use dllexport api

| API                                                          | Function                                                 |
| ------------------------------------------------------------ | -------------------------------------------------------- |
| int get_version()                                            | Get current version                                      |
| int get_api_version()                                        | Get current api version                                  |
| bool process_libil2cpp(string path, string encrypt_config_json) | Generate `EtherLibil2cpp`                                |
| bool restore_libil2cpp(string path, string encrypt_config_json) | Restore to original `libil2cpp`                          |
| bool encrypt_win(string game_dir,string game_exe_name, string encrypt_config) | Encrypt game for `Windows`                               |
| bool encrypt_android(string input_apk_unpack, string encrypt_config) | Encrypt game for `Android `(Unpack apk file is required) |

We're working on GUI programs. Coming soon~

#### 2.Use our temporary program made with Unity GUI

Download this [WindowsBuild](./Examples/EtherIl2cppBuild.zip), or build by your self with unity project [UnitySDK](./Examples/EtherIl2cppSDK.zip)

Update `EtherIl2cpp.dll`(Core module) : Replace new `EtherIl2cpp.dll` to `EtherIl2cpp_Data/Plugins/EtherIl2cpp.dll`, then reinstall EtherIl2cpp

## Supported Unity Version

unity2017.1.x - 2022.1.x