#pragma once

#include <fstream>
#include <string>
#include <io.h>
#include <vector>
#include <codecvt>
#include <windows.h>

#include "../encrypt_config.h"

using namespace std;

namespace il2cpp_lib_mgr
{
    static encrypt_config conf;

	void proc_lib(const char* p, encrypt_config cfg);
	void restore_lib(const char* p, bool isFirstCall = true);

	bool find_proc_metadatahead(const char* p);
};

