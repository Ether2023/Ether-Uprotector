#pragma once

#include <iostream>
#include <fstream>
#include <io.h>
#include <vector>
#include <windows.h>
#include <assert.h>
#include <algorithm>

using namespace std;

class oz_script_var {

public:
	oz_script_var(string na, int va) {
		n = na;
		intv = va;
	}

	string n;
	int intv;
};


namespace ozil2_script_mgr
{
	int str2opcode(string cmd);
	int parse_int(vector<oz_script_var>& vars, string s);
	string parse_str(string s);
	string parse_fp(string s);
	void run_line(vector<string>& lines, vector<oz_script_var>& vars, string c);
	void change_value(vector<oz_script_var>& vars, string name, int v);
};


