#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include <io.h>
#include <vector>
#include <codecvt>
#include <Windows.h>

using namespace std;

namespace il2cpp_lib_mgr
{
	void proc_lib(const char* p);
	void restore_lib(const char* p);
};

