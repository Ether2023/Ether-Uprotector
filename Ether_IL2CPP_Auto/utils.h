#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <io.h>
#include <vector>
#include <codecvt>
#include <Windows.h>
#include "SimpleIni.h"

#include "err_code.h"
#include "binary_encrypt_mgr.h"

using namespace std;

static CSimpleIni ini;

namespace utils {
	static bool show_mb_err = false;

	// main func
	void load_config();
	void show_help();
	bool is_show_mb_err();

	// file utils
	void read_file_lines(const char* fp, std::vector<string>& lines);
	void read_file_lines_w(const char* fp, std::vector<wstring>& lines);
	void find_fold(const char* mainDir, std::vector<string>& files);
	void find_file(const char* mainDir, std::vector<string>& files);
	size_t read_file(const char* path, char** buf);
	bool write_file(const char* path, const char* buf, size_t len);
	bool write_file_lines(const char* path, vector<string> lines);
	bool write_file_lines_w(const char* path, vector<wstring> lines);
	void read_file_int32_lines(const char* fp, vector<int>& v);

	// bin tools
	void dump(void* ptr, int buflen);
	void hex_dump(void* ptr, int buflen);
	wstring encode_getstring(string s);
	string encode_getbytes(wstring s);
	string& str_replace_all(string& src, const string old_value, const string new_value);
	wstring& wstr_replace_all(wstring& src, const wstring old_value, const wstring new_value);

	// other
	void string_split(string str, const char* spl, vector<string>& v);
	void string_split_w(wstring str, const wchar_t* spl, vector<wstring>& v);

	// user
	string user_select_file_a(const char* title, const char* filter);
}