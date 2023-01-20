#include "ether_il2cpp_api.h"

ETHER_EXPORT int __stdcall get_version(){
    return CUR_VERSION;
}

ETHER_EXPORT int __stdcall get_api_version(){
    return API_VERSION;
}

ETHER_EXPORT bool __stdcall process_libil2cpp(const char* path, const char* config){
    try {
        il2cpp_lib_mgr::proc_lib(path);
    }
    catch(const char* e){
        cout << "err : " << e << endl;
        restore_libil2cpp(path);
        return false;
    }
    return true;
}

ETHER_EXPORT bool __stdcall restore_libil2cpp(const char* path) {
    try {
        il2cpp_lib_mgr::restore_lib(path);
    }
    catch (const char *e) {
        return false;
    }
    return true;
}

ETHER_EXPORT bool __stdcall encrypt_win( const char* game_dir,const char* game_exe_name, const char* config) {
    try {
        // input
        std::string il2_bin_path = std::string(game_dir)+"/GameAssembly.dll";
        std::string uni_bin_path = std::string(game_dir)+"/UnityPlayer.dll";

        std::string game_name = std::string(game_exe_name);
        utils::str_replace_all(game_name, ".exe", "");
        std::string game_data_path = std::string(game_dir)+"/"+game_name+"_Data";

        std::string metadata_path = game_data_path+"/il2cpp_data/Metadata/global-metadata.dat";

        binary_encrypt_mgr::binary_encrypt(il2_bin_path.c_str(), metadata_path.c_str(), il2_bin_path.c_str(), metadata_path.c_str());
    }
    catch (const char *e) {
        cout << "err : " << e << endl;
        return false;
    }
    return true;
}

ETHER_EXPORT bool __stdcall encrypt_android(const char* apk_unpack, const char* config){
    try {
        // input
        std::string il2_bin_path = std::string(apk_unpack)+"/lib/arm64-v8a/libunity.so";
        std::string uni_bin_path = std::string(apk_unpack)+"/lib/arm64-v8a/libil2cpp.so";
        std::string game_data_path = std::string(apk_unpack)+"/assets/bin/Data/";

        std::string metadata_path = game_data_path+"/Managed/Metadata/global-metadata.dat";

        binary_encrypt_mgr::binary_encrypt(il2_bin_path.c_str(), metadata_path.c_str(), il2_bin_path.c_str(), metadata_path.c_str());
    }
    catch (const char *e) {
        cout << "err : " << e << endl;
        return false;
    }
    return true;
}