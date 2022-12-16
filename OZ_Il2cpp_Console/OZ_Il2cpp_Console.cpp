// OZ_Il2cpp_Console.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include <Windows.h>
#include "utils.h"
#include "il2cpp_lib_mgr.h"
#include "cpp_mgr.h"
#include "metadata_mgr.h"


using namespace std;



int main(int argc, char* const argv[], const char* optstr)
{
    //Enable chs output
    wcout.imbue(locale("chs"));

    utils::load_config();

    //Dbg
    if (argc == 1) {
        //cpp_mgr::proc_cpp("il2cpp-metadata.h", "il2cpp-metadata_enc.h", "./oz_scripts/metadataheader.ozs");
        cout << "为了调试方便,输入下面序号即可快速使用功能\n1.把global-metadata放在exe同目录即可加密" << endl;
        cout << "2.输入unity的libil2cpp的目录自动处理libil2cpp(可能需要管理员模式运行)" << endl;
        char imp[50];
        cin >> imp;
        if (strstr(imp, "1")) {
            metadata_mgr::proc_metadata("global-metadata.dat", "g_encrypted.dat");
        }
        if (strstr(imp, "2")) {
            string lujing;
            cout << "输入libil2cpp路径,如\"D://Unity2018.4.1/Editor/Data/il2cpp/libil2cpp\" (注意此模式下不能有空格)" << endl;
            cin >> lujing;
            il2cpp_lib_mgr::proc_lib(lujing.c_str());
        }

        system("pause");
    }

    if (argc == 1) {
        //Welcome text
        cout << "# 本软件用于自动生成libil2cpp加密代码" << endl;
        cout << "# 实验性功能,支持几乎所有的unity版本,暂时加密强度不如原版本" << endl;
        cout << "Usage:" << endl;
        utils::show_help();
        system("pause");
    }
    else if(argc>=2) {
        if (strcmp(argv[1], "--proclib-p") == 0) {
            il2cpp_lib_mgr::proc_lib(argv[2]);
        }
        if (strcmp(argv[1], "--restorelib-p") == 0) {
            il2cpp_lib_mgr::restore_lib(argv[2]);
        }
        if (strcmp(argv[1], "--encmetadata-p") == 0) {
            metadata_mgr::proc_metadata(argv[2], argv[3]);
        }
        //system("pause");
    }
}

