#include "cpp_mgr.h"
#include "utils.h"
#include "ozil2_script_mgr.h"

namespace cpp_mgr {

	int find_method_in_file(vector<wstring> lines, wstring method_name) {
		int num = 1;
		for (auto line : lines) {
			bool find = false;
			int index = line.find(method_name);
			if (index < line.length()) {
				if (lines[num].find(L"{") != string::npos) {
					find = true;
				}
			}
			else {
				bool find = false;
			}
			if (find) {
				return num;
			}
			num++;
		}

		return -1;//Not found
	}

	int find_str_in_file(vector<wstring> lines, wstring str,int off) {
		int num = 1;
		if (off > lines.size()) {
			//out of file
			return -1;
		}
		for (auto line : lines) {
			if (num >= off) {
				bool find = false;
				if (line.find(str) != string::npos) {
					find = true;
				}
				else {
					bool find = false;
				}
				if (find) {
					return num;
				}
			}
			num++;
		}

		return -1;//Not found
	}

	bool contain_in_line(vector<wstring> lines, wstring str, int line_num) {
		if (line_num > lines.size()) {
			return false;
		}
		bool flag = lines[line_num - 1].find(str) != string::npos;//index = line_num-1
		return flag;
	}

	void insert_lines(vector<wstring>& lines, int line_num, vector<wstring> strs) {
		for (int i = 0; i < strs.size(); i++) {
			lines.insert(lines.begin() + line_num+i, strs[i]);
		}
	}

	void insert_line(vector<wstring>& lines, int line_num, wstring str) {
		lines.insert(lines.begin() + line_num, str);
	}

	void insert_from_file(vector<wstring>& lines, int line_num, const char* fp) {
		vector<wstring> ins;
		utils::read_file_lines_w(fp, ins);
		insert_lines(lines, line_num, ins);
	}

	void insert_header(vector<wstring>& lines, wstring head) {
		wstring inc = L"#include \"" + head +L"\"";
		lines.insert(lines.begin(), inc);
	}

	void insert_method_at_from_file(vector<wstring>& lines,wstring m_n, wstring v_2,const char* fp) {
		int line_a = find_method_in_file(lines, m_n);
		int insert_a = line_a;
		while (!contain_in_line(lines, v_2, insert_a)) {
			insert_a++;
			assert(insert_a < 10000);
		}
		insert_from_file(lines, insert_a, fp);
	}

	void repl_line(vector<wstring>& lines, int ln, wstring s) {
		lines[ln-1] = s;
	}

	void repl_line_from_file(vector<wstring>& lines, int ln, const char* fp) {
		vector<wstring> a;
		utils::read_file_lines_w(fp, a);
		lines[ln -1] = a[0];
	}

	void swap_line(vector<wstring>& lines, int ln1, int ln2) {
		wstring ss = lines[ln1-1];
		lines[ln1-1] = lines[ln2-1];
		lines[ln2-1] = ss;
	}

	void proc_cpp(const char* ifp, const char* ofp, const char* sfp) {
		vector<wstring> lines;
		utils::read_file_lines_w(ifp, lines);

		vector<wstring> l_script;
		utils::read_file_lines_w(sfp, l_script);

		std::vector<oz_script_var> vars;

		for (auto c : l_script) {
			ozil2_script_mgr::run_line(lines, vars, c);
		}

		utils::write_file_lines_w(ofp, lines);
	}
}