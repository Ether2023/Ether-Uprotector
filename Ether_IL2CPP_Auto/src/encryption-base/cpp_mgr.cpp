#include "cpp_mgr.h"
#include "utils.h"
#include "ozil2_script_mgr.h"
#include "static_res.h"

namespace cpp_mgr {

	int find_method_in_file(vector<string> lines, string method_name) {
		int num = 1;
		for (auto line : lines) {
			bool find = false;
			int index = line.find(method_name);
			if (index < line.length()) {
				if (lines[num].find("{") != string::npos) {
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

	int find_str_in_file(vector<string> lines, string str,int off) {
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

	bool contain_in_line(vector<string> lines, string str, int line_num) {
		if (line_num > lines.size()) {
			return false;
		}
		bool flag = lines[line_num - 1].find(str) != string::npos;//index = line_num-1
		return flag;
	}

	void insert_lines(vector<string>& lines, int line_num, vector<string> strs) {
		for (int i = 0; i < strs.size(); i++) {
			lines.insert(lines.begin() + line_num+i, strs[i]);
		}
	}

	void insert_line(vector<string>& lines, int line_num, string str) {
		lines.insert(lines.begin() + line_num, str);
	}

	void insert_from_file(vector<string>& lines, int line_num, const char* fp) {
		vector<string> ins;
        // TODO: improve this code
        if(0==strcmp(fp, "mdc_init.cpp")){
            cout<<"11144555114"<<endl;
            utils::string_split(string((const char*)_mdc_init_cpp), "\n", ins);
        }
        if(0==strcmp(fp, "front_head.h")){
            cout<<"11144555114"<<endl;
            utils::string_split(string((const char*)_front_head_h), "\n", ins);
        }
		// utils::read_file_lines(fp, ins);
		insert_lines(lines, line_num, ins);
	}

	void insert_header(vector<string>& lines, string head) {
		string inc = "#include \"" + head +"\"";
		lines.insert(lines.begin(), inc);
	}

	void insert_method_at_from_file(vector<string>& lines,string m_n, string v_2,const char* fp) {
		int line_a = find_method_in_file(lines, m_n);
		int insert_a = line_a;
		while (!contain_in_line(lines, v_2, insert_a)) {
			insert_a++;
			assert(insert_a < 10000);
		}
		insert_from_file(lines, insert_a, fp);
	}

	void repl_line(vector<string>& lines, int ln, string s) {
		lines[ln-1] = s;
	}

	void repl_line_from_file(vector<string>& lines, int ln, const char* fp) {
		vector<string> a;
		utils::read_file_lines(fp, a);
		lines[ln -1] = a[0];
	}

	void swap_line(vector<string>& lines, int ln1, int ln2) {
		string ss = lines[ln1-1];
		lines[ln1-1] = lines[ln2-1];
		lines[ln2-1] = ss;
	}

	void repl_all(vector<string>& lines, string find, string repl) {
		for (int i = 0; i < lines.size(); i++) {
			string s = lines[i].c_str();
			utils::str_replace_all(s, find, repl);
			lines[i] = s;
		}
	}

	void repl_all_from_to(vector<string>& lines, int s, int e, string find, string repl) {
		for(int i=s-1;i<e-1;i++){
			// -1 means line num not index
			string s = lines[i];
			utils::str_replace_all(s, find, repl);
			lines[i] = s;
		}
	}

	void enc_cstr_all(vector<string>& lines, string find) {
		string findstr = "\"" + find + "\"";
		for (int i = 0; i < lines.size(); i++) {
			string s = lines[i];
			utils::str_replace_all(s, findstr, str_encrypt_mgr::create_decrypt_get_method(find));
			lines[i] = s;
		}
	}

	void proc_cpp(const char* ifp, const char* ofp, const char* script) {
		vector<string> cpplines;
		utils::read_file_lines(ifp, cpplines);

		vector<string> l_script;
		utils::string_split(string(script), "\n", l_script);

		std::vector<oz_script_var> vars;
		int line_num = 0;
		for (auto c : l_script) {
			line_num++;
			try {
				ozil2_script_mgr::run_line(cpplines, vars, c);
			}
			catch (exception e) {
				cout << "[cpp_mgr] error at line " << line_num << "\n ,cmd:" << c << "\n ,exception:" << e.what() << endl;
				exit(0);
			}
		}

		utils::write_file_lines(ofp, cpplines);
	}
}