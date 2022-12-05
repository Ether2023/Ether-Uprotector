#include "ozil2_script_mgr.h"
#include <codecvt>
#include "utils.h"
#include "cpp_mgr.h"

using namespace cpp_mgr;

namespace ozil2_script_mgr
{
	int str2opcode(wstring cmd) {
		if (cmd == L"insert_method_at_from_file") {
			return 100;
		}
		if (cmd == L"insert_line") {
			return 101;
		}
		if (cmd == L"insert_from_file") {
			return 102;
		}
		if (cmd == L"insert_header") {
			return 103;
		}
		if (cmd == L"repl_line") {
			return 104;
		}
		if (cmd == L"repl_line_from_file") {
			return 105;
		}
		if (cmd == L"swap_line") {
			return 106;
		}
		if (cmd == L"contain_in_line") {
			return 200;
		}
		if (cmd == L"find_str_in_file") {
			return 201;
		}
		if (cmd == L"find_method_in_file") {
			return 202;
		}
		if (cmd == L"read_file_int32_lines") {
			return 203;
		}
		if (cmd == L"#null") {//deleted
			return 114514;
		}
		if (cmd[0] == '$') {
			return 1001;
		}
		if (cmd == L"printd") {
			return 10;
		}
		if (cmd == L"prints") {
			return 11;
		}
		return -1;
	}

	int parse_int(vector<oz_script_var>& vars, wstring ws) {
		if (ws[0] == '$') {//int
			wstring rs = ws.substr(1);
			for (auto a : vars) {
				if (a.n == rs) {
					return a.intv;
				}
			}
			change_value(vars, ws, 0);
			return 0;
		}
		else {
			string intg;
			std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
			intg = conv.to_bytes(ws);
			return atoi(intg.c_str());
		}
	}

	string parse_str(wstring ws) {
		if (ws == L"\\n") {
			return "\n";
		}
		std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
		string s = conv.to_bytes(ws);
		return s;
	}

	string parse_fp(wstring ws) {
		return "./src_res/" + parse_str(ws);
	}

	void change_value(vector<oz_script_var>& vars, wstring name, int v) {
		bool declared = false;
		for (int i = 0; i < vars.size(); i++) {
			if (vars[i].n == name.substr(1)) {
				vars[i].intv = v;
				declared = true;
			}
		}
		if (!declared) {
			vars.push_back(oz_script_var(name.substr(1), v));
		}
	}

	void run_line(vector<wstring>& lines, vector<oz_script_var>& vars ,wstring c) {
		if (c.length() == 0) {
			//empty line
			return;
		}
		else if (c[0] == '#') {
			//comment
			return;
		}
		else {
			vector<wstring> cmd;
			utils::string_split_w(c, L" ", cmd);

			int opcode = ozil2_script_mgr::str2opcode(cmd[0]);

			int ind = 0;
			vector<int> is;
			switch (opcode) {
			case 100:
				insert_method_at_from_file(lines, cmd[1], cmd[2], parse_fp(cmd[3]).c_str());
				break;
			case 101:
				insert_line(lines, parse_int(vars, cmd[1]), cmd[2]);
				break;
			case 102:
				insert_from_file(lines, parse_int(vars, cmd[1]), parse_fp(cmd[2]).c_str());
				break;
			case 103:
				insert_header(lines, cmd[1]);
				break;
			case 104:
				repl_line(lines, parse_int(vars, cmd[1]), cmd[2]);
				break;

			case 105:
				repl_line_from_file(lines, parse_int(vars, cmd[1]), parse_fp(cmd[2]).c_str());
				break;
			case 106:
				swap_line(lines, parse_int(vars, cmd[1]), parse_int(vars, cmd[2]));
				break;


			case 200:
				change_value(vars,
					cmd[3],
					contain_in_line(lines, cmd[1], parse_int(vars,cmd[2])));
				break;
			case 201:
				change_value(vars,
					cmd[3],
					find_str_in_file(lines, cmd[1], parse_int(vars,cmd[2])));
				break;
			case 202:
				change_value(vars,
					cmd[2],
					find_method_in_file(lines, cmd[1]));
				break;

			case 203:
				
				utils::read_file_int32_lines(parse_fp(cmd[1]).c_str(), is);
				change_value(vars, cmd[3], is[parse_int(vars, cmd[2])-1]);
				break;

			/*case 1000:
				if (cmd[2] == L"=") {
					vars.push_back(oz_script_var(cmd[1], parse_int(vars, cmd[3])));
				}
				else {
					oz_script_var v = oz_script_var(cmd[1], parse_int(vars, cmd[2]));
					vars.push_back(v);
				}
				break;*/
			case 1001:
				if (cmd[1] == L"=") {
					change_value(vars,cmd[0], parse_int(vars, cmd[2]));
				}
				if (cmd[1] == L"+=") {
					change_value(vars, cmd[0], parse_int(vars, cmd[0])+ parse_int(vars, cmd[2]));
				}
				if (cmd[1] == L"-=") {
					change_value(vars, cmd[0], parse_int(vars, cmd[0]) - parse_int(vars, cmd[2]));
				}
				if (cmd[1] == L"*=") {
					change_value(vars, cmd[0], parse_int(vars, cmd[0]) * parse_int(vars, cmd[2]));
				}
				if (cmd[1] == L"/=") {
					change_value(vars, cmd[0], parse_int(vars, cmd[0]) / parse_int(vars, cmd[2]));
				}

				break;
			case 10:
				printf("%d", parse_int(vars, cmd[1]));
				break;
			case 11:
				printf("%s", parse_str(cmd[1]).c_str());
				break;
			case -1:
				return;
				break;
			}
		}
	}
};
