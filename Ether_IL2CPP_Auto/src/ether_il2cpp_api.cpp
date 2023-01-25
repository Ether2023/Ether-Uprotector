#include "ether_il2cpp_api.h"

ETHER_EXPORT int __stdcall get_version(){
    return CUR_VERSION;
}

ETHER_EXPORT int __stdcall get_api_version(){
    return API_VERSION;
}

ETHER_EXPORT bool __stdcall process_libil2cpp(const char* path, const char* cfg){
    encrypt_config conf;
    utils::load_config(string(cfg), conf);
    try {
        il2cpp_lib_mgr::proc_lib(path, conf);
    }
    catch(const char* e){
        cout << "err : " << e << endl;
        restore_libil2cpp(path, cfg);
        cout << e << endl;
        utils::close_log_file(conf.logfile.c_str());
        return false;
    }
    utils::close_log_file(conf.logfile.c_str());
    return true;
}

ETHER_EXPORT bool __stdcall restore_libil2cpp(const char* path, const char* cfg) {
    encrypt_config conf;
    utils::load_config(string(cfg), conf);
    try {
        il2cpp_lib_mgr::restore_lib(path);
    }
    catch (const char *e) {
        cout << e << endl;
        utils::close_log_file(conf.logfile.c_str());
        return false;
    }
    utils::close_log_file(conf.logfile.c_str());
    return true;
}

ETHER_EXPORT bool __stdcall encrypt_win( const char* game_dir,const char* game_exe_name, const char* cfg) {
    encrypt_config conf;
    utils::load_config(string(cfg), conf);
    try {
        // input
        std::string il2_bin_path = std::string(game_dir)+"/GameAssembly.dll";
        std::string uni_bin_path = std::string(game_dir)+"/UnityPlayer.dll";

        std::string game_name = std::string(game_exe_name);
        utils::str_replace_all(game_name, ".exe", "");
        std::string game_data_path = std::string(game_dir)+"/"+game_name+"_Data";

        std::string metadata_path = game_data_path+"/il2cpp_data/Metadata/global-metadata.dat";

        binary_encrypt_mgr::encrypt_binary(conf,
                                           uni_bin_path, uni_bin_path,
                                           "", "",
                                           il2_bin_path, il2_bin_path,
                                           "", "",
                                           metadata_path, metadata_path);
    }
    catch (const char *e) {
        cout << e << endl;
        utils::close_log_file(conf.logfile.c_str());
        return false;
    }
    utils::close_log_file(conf.logfile.c_str());
    return true;
}

ETHER_EXPORT bool __stdcall encrypt_android(const char* apk_unpack, const char* cfg){
    encrypt_config conf;
    utils::load_config(string(cfg), conf);
    try {
        // input
        std::string uni64_bin_path = std::string(apk_unpack)+"/lib/arm64-v8a/libunity.so";
        std::string il264_bin_path = std::string(apk_unpack)+"/lib/arm64-v8a/libil2cpp.so";

        std::string uni32_bin_path = std::string(apk_unpack)+"/lib/armeabi-v7a/libunity.so";
        std::string il232_bin_path = std::string(apk_unpack)+"/lib/armeabi-v7a/libil2cpp.so";

        std::string game_data_path = std::string(apk_unpack)+"/assets/bin/Data/";

        std::string metadata_path = game_data_path+"/Managed/Metadata/global-metadata.dat";

        binary_encrypt_mgr::encrypt_binary(conf,
                                           uni32_bin_path, uni32_bin_path,
                                           uni64_bin_path, uni64_bin_path,
                                           il232_bin_path, il232_bin_path,
                                           il264_bin_path, il264_bin_path,
                                           metadata_path, metadata_path);
    }
    catch (const char *e) {
        cout << e << endl;
        utils::close_log_file(conf.logfile.c_str());
        return false;
    }
    utils::close_log_file(conf.logfile.c_str());
    return true;
}