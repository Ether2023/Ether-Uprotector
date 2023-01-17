#include <iostream>
#include <stdio.h>
#include <string.h>
#include <string> 

namespace encryption {
	uint32_t crc32(char* buf, int len);
	void oz_encryption(char* buf, int len, const char* key, int keylen);
	const char* get_string_decrypt(const char* str, int key);

#if _WIN32
	uint32_t check_sum_gameassembly(const char* binpath, uint32_t crc);
#endif

#if IL2CPP_TARGET_ANDROID
	uint32_t check_sum_libil2cpp(const char* binpath, uint32_t crc);
#endif
	void kill_process();
}

