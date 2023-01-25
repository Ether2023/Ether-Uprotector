#pragma once

#include <stdio.h>
#include <string.h>
#include <string>

namespace encryption {
    uint32_t crc32(char* buf, int len);
    void oz_encryption(char* buf, int len, const char* key, int keylen);
    const char* get_string_decrypt(const char* str, int key);

#if _WIN32
    void check_sum_gameassembly(uint32_t crc32, uint32_t crc64);
#endif

#if IL2CPP_TARGET_ANDROID
    void check_sum_libil2cpp(uint32_t crc32, uint32_t crc64);
#endif
    void kill_process();
}

