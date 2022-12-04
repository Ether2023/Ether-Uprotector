#pragma once

#include <iostream>
#include <fstream>
#include <io.h>
#include <vector>
#include <Windows.h>
#include <assert.h>

using namespace std;

namespace cpp_mgr
{
	//Cpp file tools
	int find_method_in_file(vector<wstring> lines, wstring method_name);
	int find_str_in_file(vector<wstring> lines, wstring str, int off);
	bool contain_in_line(vector<wstring> lines, wstring str, int line_num);
	void insert_line(vector<wstring>& lines, int line_num, wstring str);
	void insert_lines(vector<wstring>& lines, int line_num, vector<wstring> strs);
	void insert_header(vector<wstring>& lines, wstring head);
	void insert_from_file(vector<wstring>& lines, int line_num, const char* fp);
	void repl_line(vector<wstring>& lines, int ln, wstring s);
	void insert_method_at_from_file(vector<wstring>& lines, wstring m_n, wstring v_2, const char* fp);
	void proc_cpp(const char* ifp, const char* ofp, const char* sfp);
}

