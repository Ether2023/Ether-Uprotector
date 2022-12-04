#pragma once

#include <iostream>
#include <fstream>
#include <io.h>
#include <vector>
#include <Windows.h>
#include <assert.h>
#include <algorithm>

using namespace std;

class oz_script_var {

public:
	oz_script_var(wstring na, int va) {
		n = na;
		intv = va;
	}

	wstring n;
	int intv;
};


namespace ozil2_script_mgr
{
	int str2opcode(wstring cmd);
	int parse_int(vector<oz_script_var>& vars, wstring ws);
	string parse_str(wstring ws);
	string parse_fp(wstring ws);
	void run_line(vector<wstring>& lines, vector<oz_script_var>& vars, wstring c);
	void change_value(vector<oz_script_var>& vars, wstring name, int v);
};


