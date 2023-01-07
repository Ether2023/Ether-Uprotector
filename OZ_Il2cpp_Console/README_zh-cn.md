# OZ-Il2Cpp-Console

Experimental function, not perfect yet, please wait

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

| Il2Cpp Version | Unity Version                | Support        |
| -------------- | ---------------------------- |--------------  |
| 24.0           | 2017.x - 2018.2.x            | ✔️             |
| 24.1           | 2018.3.x - 2018.4.x          | ✔️             |
| 24.2           | 2019.1.x - 2019.2.x          | ✔️             |
| 24.3           | 2019.3.x, 2019.4.x, 2020.1.x |✔️              |
| 24.4           | 2019.4.x and 2020.1.x        |✔️              |
| 27.0           | 2021.2.x                     | ✔️             |
| 27.1           | 2020.2.x - 2020.3.x          | ✔️             |
| 27.2           | 2021.1.x, 2021.2.x           | ✔️             |
| 28             | 2021.3.x, 2022.1.x           |✔️              |
