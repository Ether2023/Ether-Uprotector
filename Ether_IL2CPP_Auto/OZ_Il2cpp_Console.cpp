// OZ_Il2cpp_Console.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include <Windows.h>
#include "utils.h"
#include "il2cpp_lib_mgr.h"
#include "cpp_mgr.h"
#include "binary_encrypt_mgr.h"
#include "str_encrypt_mgr.h"

#define DEVELOPMENT_VERSION 1;

// useless
constexpr int CURRENT_VERSION = 200;
// only change this when api changed
// must equal to GUI_app's api version
constexpr int API_VERSION = 105;

using namespace std;

int main(int argc, char* const argv[], const char* optstr)
{
    //Enable chs output
    wcout.imbue(locale("chs"));
    utils::load_config();
   

#if DEVELOPMENT_VERSION
    //Dbg
    if (argc == 1) {
        cout << "你正在使用开发版本!!!\n\n\n";
        //cpp_mgr::proc_cpp("il2cpp-metadata.h", "il2cpp-metadata_enc.h", "./oz_scripts/metadataheader.ozs");
        cout << "为了调试方便, 输入下面序号即可快速使用功能, 但是建议使用「命令行调用」"<<endl;
        cout << "1.手动选择文件加密" << endl;
        cout << "2.输入unity的libil2cpp的目录自动「处理」libil2cpp\t(可能需要管理员模式运行)" << endl;
        cout << "3.输入unity的libil2cpp的目录「还原」处理过的libil2cpp\t(可能需要管理员模式运行)" << endl;
        char imp[50];
        cin >> imp;
        if (strstr(imp, "1")==0) {
            string uapath = utils::user_select_file_a("Il2cpp binary file", "*.*");
            string gbpath = utils::user_select_file_a("global-metadata.dat", "*.dat");
            string gbout = utils::user_select_file_a("[Output] global-metadata.dat", "*.*");
            binary_encrypt_mgr::binary_encrypt(uapath.c_str(), gbpath.c_str(), "UserAssembly_encrypted.dll", gbout.c_str());
        }
        if (strstr(imp, "2")==0) {
            string lujing;
            cout << "输入libil2cpp路径,如\"D://Unity2018.4.1/Editor/Data/il2cpp/libil2cpp\" (注意此模式下不能有空格)" << endl;
            cin >> lujing;
            il2cpp_lib_mgr::proc_lib(lujing.c_str());
        }
        if (strstr(imp, "3")==0) {
            string lujing;
            cout << "输入libil2cpp路径,如\"D://Unity2018.4.1/Editor/Data/il2cpp/libil2cpp\" (注意此模式下不能有空格)" << endl;
            cin >> lujing;
            il2cpp_lib_mgr::restore_lib(lujing.c_str());
        }

    }
#endif

    if (argc == 1) {
        //Welcome text
        cout << "# This program is used to add EtherEncryption in to libil2cpp and encrypt binary files." << endl;
        cout << "Usage:" << endl;
        utils::show_help();
        system("pause");
    }
    else if(argc>=2) {
        if (strcmp(argv[1], "--proclib-p") == 0) {
            try {
                il2cpp_lib_mgr::proc_lib(argv[2]);
            }
            catch (const char* e) {
                cout << "err : " << e << endl;
                exit(ERR_CODE_FAIL);
            }
        }
        if (strcmp(argv[1], "--restorelib-p") == 0) {
            try {
                il2cpp_lib_mgr::restore_lib(argv[2]);
            }
            catch (const char* e) {
                cout << "err : " << e << endl;
                exit(ERR_CODE_FAIL);
            }
        }
        if (strcmp(argv[1], "--enc-p") == 0) {
            try {
                binary_encrypt_mgr::binary_encrypt(argv[2], argv[3], argv[4], argv[5]);
            }
            catch(const char* e){
                cout << "err : " << e << endl;
                exit(ERR_CODE_FAIL);
            }
        }
        if (strcmp(argv[1], "--dec-p") == 0) {
            cout << "not supported." << endl;
            exit(ERR_CODE_FAIL);
        }
        if (strcmp(argv[1], "--version") == 0) {
            cout << to_string(CURRENT_VERSION) << endl;
        }
        if (strcmp(argv[1], "--apiversion") == 0) {
            cout << to_string(API_VERSION) << endl;
        }
        if (strcmp(argv[1], "--dev") == 0) {
#if DEVELOPMENT_VERSION
            cout << "true" << endl;
#else
            cout << "false" << endl;
#endif
        }
    }
}

