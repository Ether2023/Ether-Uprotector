#pragma once


#include <fstream>
#include <sstream>
#include <io.h>
#include <vector>
#include <windows.h>
#include <assert.h>
#include "str_encrypt_mgr.h"
#include "../encrypt_config.h"

using namespace std;

namespace cpp_mgr
{
    static encrypt_config conf;

	//Cpp file tools
	int find_method_in_file(vector<string> lines, string method_name);
	int find_str_in_file(vector<string> lines, string str, int off);
	bool contain_in_line(vector<string> lines, string str, int line_num);
	void insert_line(vector<string>& lines, int line_num, string str);
	void insert_lines(vector<string>& lines, int line_num, vector<string> strs);
	void insert_header(vector<string>& lines, string head);
	void insert_from_file(vector<string>& lines, int line_num, const char* fp);
	void repl_line(vector<string>& lines, int ln, string s);
	void insert_method_at_from_file(vector<string>& lines, string m_n, string v_2, const char* fp);
	void proc_cpp(const char* ifp, const char* ofp, const char* script, encrypt_config config);
	void repl_line_from_file(vector<string>& lines, int ln, const char* fp);
	void swap_line(vector<string>& lines, int ln1, int ln2);
	void repl_all(vector<string>& lines, string find, string repl);
	void repl_all_from_to(vector<string>& lines, int s, int e, string find, string repl);
	void enc_cstr_all(vector<string>& lines, string find);
}

