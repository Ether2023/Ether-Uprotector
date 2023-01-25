#pragma once
#include <fstream>
#include <io.h>
#include <vector>
#include <windows.h>
#include <assert.h>
#include <algorithm>
#include <codecvt>
#include "utils.h"
#include "cpp_mgr.h"
#include "encryption.h"
#include <ctime>
#include <random>

#include "err_code.h"
#include "binary_encrypt_mgr.h"
#include "../encrypt_config.h"

namespace binary_encrypt_mgr {
    static std::string encrypt_key;

    void encrypt_binary(encrypt_config conf, string ui1, string uo1, string ui2, string uo2
    , string bi1, string bo1, string bi2, string bo2,
                        string mi, string mo);

    void swap_header_int32(char *data, int p1, int p2);

    void generate_ozmetadata_header(char *data, size_t len_o, uint32_t crc_x32, uint32_t crc_x64);

    void proc_strings(char *data, size_t len);

    void proc_stringslit(char *data, size_t len);

    void proc_binary(const char *bi, const char *bo, uint32_t &crc_x32, uint32_t &crc_x64);

    void proc_metadata(const char *mdi, const char *mdo, uint32_t crc_x32, uint32_t crc_x64);

    void proc_unity_binary(const char *bi, const char *bo);
}

//FrontHeader
#pragma pack(push, p1, 4)
typedef struct FrontHeader
{
	int64_t sanity;// it must be 8102084797857888879
	int64_t offset;
	int32_t length;
	uint32_t crc_x32;
	uint32_t crc_x64;
	char key[128];// it can't be more
} FrontHeader;
#pragma pack(pop, p1)