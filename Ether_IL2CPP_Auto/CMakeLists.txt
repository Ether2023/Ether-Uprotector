cmake_minimum_required(VERSION 3.24)
project(Ether_IL2CPP_Auto)

set(CMAKE_CXX_STANDARD 14)

# build exe
#add_executable(EtherIl2cpp src/main.cpp src/encryption-base/binary_encrypt_mgr.cpp src/encryption-base/utils.cpp src/ether_il2cpp_api.h src/encryption-base/cpp_mgr.cpp src/encryption-base/encryption.cpp src/encryption-base/il2cpp_lib_mgr.cpp src/encryption-base/ozil2_script_mgr.cpp src/encryption-base/str_encrypt_mgr.cpp src/ether_il2cpp_api.cpp src/encryption-base/static_res.h)

# build dll
add_library(EtherIl2cpp SHARED src/main.cpp src/encryption-base/binary_encrypt_mgr.cpp src/encryption-base/utils.cpp src/ether_il2cpp_api.h src/encryption-base/cpp_mgr.cpp src/encryption-base/encryption.cpp src/encryption-base/il2cpp_lib_mgr.cpp src/encryption-base/ozil2_script_mgr.cpp src/encryption-base/str_encrypt_mgr.cpp src/ether_il2cpp_api.cpp src/encryption-base/static_res.h)

set(CMAKE_EXE_LINKER_FLAGS "-static")
