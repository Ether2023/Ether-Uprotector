# Ether_IL2CPP_Auto

## 实验性功能,暂时还不完善,敬请期待

自动生成libil2cpp代码,一键加密global-metadata.dat,支持绝大多数unity版本

## 保护内容

|功能                       |支持|
|---------------------------|----|
|global-metadata文件加密保护|✔️   |
|隐藏MetadataHeader         |✔️   |
|混淆MetadataHeader顺序     |✔️   |
|多指针防止dump出原文件     |✔️   |
|字符串加密                 |    |
|常量字符串加密             |    |

## 使用方法

1.使用命令行调用(推荐)

|命令                                                   |功能                       |
|-------------------------------------------------------|---------------------------|
|--proclib-p     <libil2cpp_path>                       |生成ozlibil2cpp            |
|--encmetadata-p <metadata_path> <metadata_output_path> |加密global-metadata.dat文件|

我们正在全力开发GUI程序哦,敬请期待

2.使用临时调试
直接运行exe,输入临时代号直接使用

## 支持的Unity版本
unity2017.1.x - 2022.1.x