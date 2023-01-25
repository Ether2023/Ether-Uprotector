#pragma once

#include "encryption-base/il2cpp_lib_mgr.h"
#include "encryption-base/cpp_mgr.h"
#include "encryption-base/binary_encrypt_mgr.h"
#include "encryption-base/utils.h"
#include "encrypt_config.h"
#include "../include/json.h"

#define DEVELOPMENT_VERSION 1

// cur version but I think it's useless
constexpr int CUR_VERSION = 107;

// only change this when api changed
// must equal to GUI_app's api version
constexpr int API_VERSION = 105;

#ifndef DLL_EXPORT
#define ETHER_EXPORT __declspec(dllexport)
#else
#define ETHER_IMPORT __declspec(dllimport)
#endif

extern "C"{
ETHER_EXPORT int __stdcall get_version();
ETHER_EXPORT int __stdcall get_api_version();
ETHER_EXPORT bool __stdcall process_libil2cpp(const char* path, const char* config);
ETHER_EXPORT bool __stdcall restore_libil2cpp(const char* path, const char* config);

ETHER_EXPORT bool __stdcall encrypt_win( const char* game_dir,const char* game_exe_name, const char* config);

ETHER_EXPORT bool __stdcall encrypt_android(const char* input_apk_unpack, const char* config);
}

