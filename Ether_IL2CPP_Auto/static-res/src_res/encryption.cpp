#include "encryption.h"

uint32_t CRC32_POLYNOMIAL = 0xEDB88320;

namespace encryption {
    uint32_t crc32(char* buf, int len)
    {
        //make table
        uint32_t table[256];
        int i, j;
        for (i = 0; i < 256; i++) {
            for (j = 0, table[i] = i; j < 8; j++) {
                table[i] = (table[i] >> 1) ^ ((table[i] & 1) ? CRC32_POLYNOMIAL : 0);
            }
        }

        uint32_t crc = 0;
        crc = ~crc;
        for (int i = 0; i < len; i++) {
            crc = (crc >> 8) ^ table[(crc ^ buf[i]) & 0xff];
        }

        return ~crc;
    }

    void oz_encryption(char* buf, int len, const char* key, int keylen) {
        char* result = buf;
        for (int a = 0; a < len; a++) {
            result[a] ^= key[a % keylen]^a*a +29*a+15;
        }
        return;

        int i = 0, j = 0, k = 0;
        char t = 0;
        char s[256]{};//sbox
        char keybox[256]{};
        char tmp = 0;

        //init key
        for (i = 0; i < 256; i++) {
            s[i] = i;
            keybox[i] = key[i % keylen];
        }
        //proc sbox
        for (i = 0; i < 256; i++) {
            j = (j + s[i] + keybox[i]) % 256;
            tmp = s[i];
            s[i] = s[j];//swap s[i] and s[j]
            s[j] = tmp;
        }
        //crypt
        for (k = 0; k < len; k++)
        {
            i = (i + 1) % 256;
            j = (j + s[i]) % 256;
            tmp = s[i];
            s[i] = s[j];//swap s[x] and s[y]
            s[j] = tmp;
            t = (s[i] + s[j]) % 256;
            result[k] ^= s[t];
        }
    }


#if _WIN32
    void check_sum_gameassembly(uint32_t crc32, uint32_t crc64) {
        char* this_binary;
        int sz_this = 0;
        //const char* binpath = fp;
        FILE* pFile;
        fopen_s(&pFile, encryption::get_string_decrypt("F\\l\\>jldjYkt-[ic", -667221561), "rb");
        if (pFile == NULL) {
            kill_process();
        }
        fseek(pFile, 0, SEEK_END);
        sz_this = ftell(pFile);
        rewind(pFile);
        this_binary = (char*)malloc(sizeof(char) * sz_this + 1);
        if (this_binary == NULL) {
            kill_process();
        }
        size_t result = fread(this_binary, 1, sz_this, pFile);
        if (result != sz_this) {
            kill_process();
        }
        fclose(pFile);

        //std::string platform = "Unknown";
        int architecture = -1;

        if (((int*)this_binary)[0] == 0x905A4D) {// MZ
            //platform = "WIN32";

            int pe_addr = 0;
            while (true) {
                if (*((int*)(this_binary + pe_addr)) == 0x4550) {// PE
                    break;
                }
                pe_addr += sizeof(int);
                if (pe_addr > result) {
                    break;
                }
            }
            if (this_binary[pe_addr + sizeof(int)] == 0x4C) {// PE..L
                architecture = 32;
            }
            if (this_binary[pe_addr + sizeof(int)] == 0x64) {// PE..d
                architecture = 64;
            }
        }
        else{// not win32 dll
            kill_process();
        }

        uint32_t crc_get = encryption::crc32(this_binary, sz_this);
        free(this_binary);
        if ((architecture == 32 && crc_get == crc32) || (architecture == 64 && crc_get == crc64) ) {
            // check sum success
        }
        else{
            // fail
            kill_process();
        }
    }

    void kill_process() {
        int* nullpt = (int*)0;
        for(int i = 0; i < 2147183647; i++) {
            *(nullpt+i) = 0xCC;
            if (i % 114514 == 0) {
                kill_process();
            }
        }
        throw 1919810;
        exit(-1);
    }
#endif

#if IL2CPP_TARGET_ANDROID
    void check_sum_libil2cpp(uint32_t crc32, uint32_t crc64) {

	}
#endif

    const char* get_string_decrypt(const char* str, int key) {
        char* c = (char*)malloc(strlen(str) + 1);
        c[strlen(str)] = 0;// end of str
        strcpy_s(c, strlen(str) + 1, str);
        int final_key[10]{};
        int i,j;
        for (i = 0; i < 10; i++) {
            final_key[i] += (key ^ i * i + 63 * i) % 10;
        }
        for (i = 0, j = 0; c[j]; j++, i = (i + 1) % 10) {

            c[j] -= final_key[i];

            if (c[j] < 32) c[j] += 90;
        }
        return c;
    }
}