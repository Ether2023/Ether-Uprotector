#include "il2cpp_lib_mgr.h"
#include "utils.h"
#include "cpp_mgr.h"

namespace il2cpp_lib_mgr
{
	void proc_lib(const char* p) {
		string sp = string(p);
		//libil2cpp

		vector<string> f_in_libd;
		utils::find_file(sp.c_str(), f_in_libd);
		bool succ = false;

		for (auto s : f_in_libd) {
			vector<wstring> lines;
			utils::read_file_lines_w(s.c_str(), lines);
			
			//il2cpp-metadata.h
			if (cpp_mgr::find_str_in_file(lines, L"typedef struct Il2CppGlobalMetadataHeader", 0) != -1) {
				CopyFileA(s.c_str(), (s + ".bak").c_str(), FALSE);
				//make backup

				cpp_mgr::proc_cpp((s + ".bak").c_str(), s.c_str(), "./oz_scripts/metadataheader.ozs");
				succ = true;
				break;
			}
			
		}

		if (!succ) {
			cout << "[il2cpp_lib_mgr] " << "Failed to find il2cpp-metadata.h"<<" Path:"<<p << endl;
			return;
		}

		vector<string> f_in_vm;
		utils::find_file((sp+"/vm/").c_str(), f_in_vm);
		succ = false;
		for (auto s : f_in_vm) {
			vector<wstring> lines;
			utils::read_file_lines_w(s.c_str(), lines);
			//MetadataCache.cpp
			if (cpp_mgr::find_str_in_file(lines, L"IL2CPP_ASSERT(s_GlobalMetadataHeader->sanity == 0xFAB11BAF)", 0) != -1) {
				CopyFileA(s.c_str(), (s + ".bak").c_str(), FALSE);
				//make backup
				succ = true;
				cpp_mgr::proc_cpp((s + ".bak").c_str(), s.c_str(), "./oz_scripts/mdc.ozs");
			}

		}

		if (!succ) {
			cout << "[il2cpp_lib_mgr] " << "Failed to find MetadataCache.cpp"<<" Path:"<<sp+"/vm/" << endl;
			return;
		}

		//Move includes(xxtea)
		CopyFileA("./src_res/xxtea.cpp", (sp+"/vm/xxtea.cpp").c_str(), FALSE);
		CopyFileA("./src_res/xxtea.h", (sp+"/vm/xxtea.h").c_str(), FALSE);

		//Congratulations
		cout << "[il2cpp_lib_mgr] " << "Generate oz libil2cpp succ"<<endl;

		//TODO : Now ,only 2 files need to be changed, maybe more in the future.
		//       So add a command file to describe its logic
	}
};