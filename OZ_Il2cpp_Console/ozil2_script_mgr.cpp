#include "ozil2_script_mgr.h"

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
		return -1;
	}
};
