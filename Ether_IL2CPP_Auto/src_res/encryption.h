#include <iostream>
#include <fstream>
#include <iostream>

namespace encryption {
	uint32_t crc32(char* buf, int len);
	void oz_encryption(char* buf, int len, const char* key, int keylen);
	void oz_encryption_new(char** buf, int len, const char* key, int keylen);
	const char* get_string_decrypt(const char* str, int key);

#if _WIN32
	uint32_t check_sum_gameassembly(const char* binpath, uint32_t crc);

	void kill_process();
#endif
}

