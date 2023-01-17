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
|String encryption                 |    |
|Const string Encryption             |    |

## Usage

### Use command line calls (recommended)

|Command                                                   |Function                       |
|-------------------------------------------------------|---------------------------|
|--proclib-p     <libil2cpp_path>                       |Generate `ozlibil2cpp`            |
|--encmetadata-p <metadata_path> <metadata_output_path> |Encrypt `global-metadata.dat` file|

We're working on GUI programs. Please wait

### Use temporary debugging

Run exe directly, enter temporary code and use it directly

## Supported Unity Version

unity2017.1.x - 2022.1.x