#pragma once
#include <iostream>
#include <fstream>
#include <io.h>
#include <vector>
#include <Windows.h>
#include <assert.h>
#include <algorithm>
#include <codecvt>
#include "utils.h"
#include "cpp_mgr.h"
#include "xxtea.h"
#include <ctime>
#include <random>

namespace metadata_mgr
{
	void proc_metadata(const char* ifp, const char* ofp);
	void swap_header_int32(char* data, int p1, int p2);
	void generate_ozmetadata_header(char* data, size_t len_o);
	void proc_strings(char* data, size_t len);
	void proc_stringslit(char* data, size_t len);
};

//FrontHeader
#pragma pack(push, p1, 4)
typedef struct FrontHeader
{
	char sign[24];
	int64_t offset;
	int32_t length;
	unsigned char key[32];
} FrontHeader;
#pragma pack(pop, p1)