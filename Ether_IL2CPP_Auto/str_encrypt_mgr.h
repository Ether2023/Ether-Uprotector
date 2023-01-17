#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <random>
#include <Windows.h>
#include <assert.h>
#include "utils.h"

using namespace std;

#pragma once
namespace str_encrypt_mgr
{
	const char* get_string_encrypt(const char* str, int key);
	wstring create_decrypt_get_method(wstring str);
}

