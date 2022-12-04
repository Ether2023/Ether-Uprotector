// OZ_Il2cpp_Console.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include <Windows.h>
#include "utils.h"
#include "cpp_mgr.h"

using namespace std;

int main(int argc, char* const argv[], const char* optstr)
{
    //Enable chs output
    wcout.imbue(locale("chs"));

    //Dbg
    cpp_mgr::proc_cpp("MetadataCache.cpp", "test.cpp", "./oz_scripts/mdc.ozs");
    system("pause");

    if (argc == 1) {
        //Welcome text
        cout << "# 本软件用于自动生成libil2cpp加密代码" << endl;
        cout << "# 实验性功能,支持几乎所有的unity版本,但加密强度可能不如正常版" << endl;
        cout << "Usage:" << endl;
        utils::show_help();
        system("pause");
    }
    else if(argc>=2) {
        
    }
}