#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <io.h>
#include <vector>
#include <codecvt>
#include <Windows.h>

using namespace std;

namespace utils {
	//main func utils
	void show_help();

	//file utils
	void read_file_lines(const char* fp, std::vector<string>& lines);
	void read_file_lines_w(const char* fp, std::vector<wstring>& lines);
	void find_fold(const char* mainDir, std::vector<string>& files);
	void find_file(const char* mainDir, std::vector<string>& files);
	size_t read_file(const char* path, char** buf);
	bool write_file(const char* path, const char* buf, size_t len);
	bool write_file_lines(const char* path, vector<string> lines);
	bool write_file_lines_w(const char* path, vector<wstring> lines);

	//byte tools
	void dump(void* ptr, int buflen);
	void hex_dump(void* ptr, int buflen);

	//other
	void string_split(string str, const char* spl, vector<string>& v);
	void string_split_w(wstring str, const wchar_t* spl, vector<wstring>& v);
}