#pragma once

#include "encryption-base/utils.h"
#include "encryption-base/il2cpp_lib_mgr.h"
#include "encryption-base/cpp_mgr.h"
#include "encryption-base/binary_encrypt_mgr.h"
#include "encryption-base/str_encrypt_mgr.h"
#include "ether_il2cpp_api.h"
#include "encrypt_config.h"
#include "../include/json.h"

using namespace std;

int main(int argc, char* const argv[]) {
    //Enable chs output
    setlocale(LC_ALL, "");
    cout<<encryption::get_string_decrypt("N[J#SG^5", 852143741)<<endl;
    if (argc == 1) {
        //Welcome text
        cout << "# This program is used to add EtherEncryption in to libil2cpp and encrypt binary files." << endl;
        cout << "Usage:" << endl;
        utils::show_help();
        system("pause");
    }
    else if(argc>=2) {
        if (strcmp(argv[1], "--proclib-p") == 0) {
            process_libil2cpp(argv[2], "");
        }
        if (strcmp(argv[1], "--restorelib-p") == 0) {
            restore_libil2cpp(argv[2], "");
        }
        if (strcmp(argv[1], "--enc-android-p") == 0) {
            encrypt_android(argv[2], argv[3]);
        }
        if (strcmp(argv[1], "--enc-win-p") == 0) {
            encrypt_win(argv[2], argv[3], argv[4]);
        }
        if (strcmp(argv[1], "--dec-p") == 0) {
            cout << "not supported." << endl;
            exit(ERR_CODE_FAIL);
        }
        if (strcmp(argv[1], "--version") == 0) {
            cout << to_string(get_version()) << endl;
        }
        if (strcmp(argv[1], "--apiversion") == 0) {
            cout << to_string(get_api_version()) << endl;
        }
        if (strcmp(argv[1], "--dev") == 0) {
#if DEVELOPMENT_VERSION
            cout << "true" << endl;
#else
            cout << "false" << endl;
#endif
        }
    }
    return 0;
}
