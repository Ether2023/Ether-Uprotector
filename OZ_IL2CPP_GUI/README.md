# OZ-IL2CPP-GUI

## Encryption Process

1. We redefined and declared new headers and hide them in files after encrypting them, making it difficult for crackers to get access to the original header
2. We confused the Header and hide key parameters such as sanity and verison, which prevented software such as `IL2CPPDumper` from correctly identifying metadata files

Comming soon: Encrypt String and StringLiteral sections within Metadata

## Usage

1. Download the source code and compile the project using VS, or download direcctly from  [Release](https://github.com/Z1029-oRangeSumMer/O-Z-IL2CPP/releases)
2. Make sure `.NET 6.0` is installed
3. Launch `OZ_IL2CPP_GUI.exe`
![gui_application](pics/gui_main.png)
4. Click Install and select the corresponding `Unity.exe`. The installation path can be found in the `Unity Hub`
5. Launch Unity, change the script backend to Il2cpp in BuildSettings, and build the project
6. Click `select a exe file` or `select an apk file`, select the game just built, and click `encrypt`
7. If it is an apk, it needs to be manually signed after encryption, and the exe can be run directly (if the software cannot run at this time, welcome to issue for bug feedback)
8. Enjoy the safety that `O&Z IL2cpp` brought to you! :D

## Supported Unity Version

| Il2Cpp Version | Unity Version                | Support        |
| -------------- | ---------------------------- |--------------  |
| 24.0           | 2017.x - 2018.2.x            |✔️              |
| 24.1           | 2018.3.x - 2018.4.x          |✔️              |
| 24.2           | 2019.1.x - 2019.2.x          |✔️              |
| 24.3           | 2019.3.x, 2019.4.x, 2020.1.x |✔️             |
| 24.4           | 2019.4.x and 2020.1.x        |✔️             |
| 27.0           | 2021.2.x                     |✔️              |
| 27.1           | 2020.2.x - 2020.3.x          | ✔️              |
| 27.2           | 2021.1.x, 2021.2.x           |✔️              |
| 28             | 2021.3.x, 2022.1.x           |✔️             |
