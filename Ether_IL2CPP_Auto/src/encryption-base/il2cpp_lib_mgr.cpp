#include "il2cpp_lib_mgr.h"
#include "utils.h"
#include "cpp_mgr.h"
#include "static_res.h"

#define EXT_OZ_BACKUP ".ether_il2cpp_bak"

namespace il2cpp_lib_mgr
{
	void proc_lib(const char* p) {
		string sp = string(p);
		// libil2cpp

		cout << "[il2cpp_lib_mgr] " << "start generate at" << " path:" << p << endl;
		
		// step 1
		cout << "[il2cpp_lib_mgr] " << "finding il2cpp-metadata.h...\n";
		bool succ = false;
		succ = find_proc_metadatahead(sp.c_str());
		if (!succ) {
			succ = find_proc_metadatahead((sp + "/vm").c_str()); // for unity 2022
		}

		if (!succ) {
			cout << "[il2cpp_lib_mgr] " << "failed to find il2cpp-metadata.h in path:" << sp << endl;
			return;
		}
		cout << "done" << endl;

		// step 2
		cout << "[il2cpp_lib_mgr] " << "finding MetadataCache.cpp...\n";
		succ = false;
		vector<string> f_in_vm;
		utils::find_file((sp + "/vm/").c_str(), f_in_vm);
		
		for (auto s : f_in_vm) {
			vector<string> lines;
			utils::read_file_lines(s.c_str(), lines);
			// MetadataCache.cpp
			if (cpp_mgr::find_str_in_file(lines, "IL2CPP_ASSERT(s_GlobalMetadataHeader->sanity", 0) != -1) {
				cout << "done" << endl;
				CopyFileA(s.c_str(), (s + EXT_OZ_BACKUP).c_str(), FALSE);
				// make backup
				succ = true;
				cout << "[il2cpp_lib_mgr] " << "processing MetadataCache.cpp...\n";
                // "./oz_scripts/mdc.ozs"
				cpp_mgr::proc_cpp((s + EXT_OZ_BACKUP).c_str(), s.c_str(), (const char*)_mdc_ozs);
				cout << "done" << endl;
				break;
			}
			
		}

		if (!succ) {
			cout << "[il2cpp_lib_mgr] " << "failed to find MetadataCache.cpp" << " path:" << sp + "/vm/" << endl;
			return;
		}

		// move includes(oz_encryption)
        utils::write_file((sp+"/vm/encryption.cpp").c_str(), (char*)_encryption_cpp, sizeof(_encryption_cpp));
        utils::write_file((sp+"/vm/encryption.h").c_str(), (char*)_encryption_h, sizeof(_encryption_h));
		// CopyFileA("./src_res/encryption.cpp", (sp+"/vm/encryption.cpp").c_str(), FALSE);
		// CopyFileA("./src_res/encryption.h", (sp+"/vm/encryption.h").c_str(), FALSE);

		// congratulations
		cout << "[il2cpp_lib_mgr] " << "generate oz libil2cpp succ" << endl;

		// TODO : Now ,only 2 files need to be changed, maybe more in the future.
		//        So add a command file to describe its logic
	}

	bool find_proc_metadatahead(const char* p) {
		string sp = string(p);
		vector<string> fs;
		utils::find_file(sp.c_str(), fs);

		//Check bak
		for (auto s : fs) {
			if (s.find(EXT_OZ_BACKUP) != string::npos) {
                string err = (string("[il2cpp_lib_mgr] already installed. please uninstall first. path:") + sp).c_str();
                //cout << err << endl;
                throw err.c_str();
            }
		}

		for (auto s : fs) {
			vector<string> lines;
			utils::read_file_lines(s.c_str(), lines);
			//il2cpp-metadata.h
			if (cpp_mgr::find_str_in_file(lines, "typedef struct Il2CppGlobalMetadataHeader", 0) != -1) {
				//make backup
				CopyFileA(s.c_str(), (s + EXT_OZ_BACKUP).c_str(), FALSE);

                // "./oz_scripts/metadataheader.ozs"
				cpp_mgr::proc_cpp((s + EXT_OZ_BACKUP).c_str(), s.c_str(), (const char*)_metadataheader_ozs);
				return true;
			}

		}
		
		return false;
	}

	void restore_lib(const char* p, bool isFirstCall) {
		string sp = string(p);
		//libil2cpp

		vector<string> fs;
		vector<string> ds;
		utils::find_file(sp.c_str(), fs);
		utils::find_fold(sp.c_str(), ds);
		bool succ = false;

		//Check bak
		for (auto s : fs) {
			if (s.find(EXT_OZ_BACKUP) == s.length() - strlen(EXT_OZ_BACKUP)) {
				string ops = s;
				int a = s.find(EXT_OZ_BACKUP);
				string rps = (string(ops.c_str()));
				rps = rps.replace(rps.begin() + a, rps.begin() + a + strlen(EXT_OZ_BACKUP), "");
				cout << "[il2cpp_lib_mgr] " << "found bak at" << " path:" << ops <<"\n\t\tmove to "<<rps << endl;
				BOOL b = CopyFileA(s.c_str(), rps.c_str(), FALSE);
				b = b==TRUE && DeleteFileA(s.c_str())==TRUE;
				if (b == FALSE) {
					cout << "[il2cpp_lib_mgr] " << "failed to move file"<<endl;
					return;
				}
			}
		}

		for (auto s : ds) {
			restore_lib(s.c_str(), false);
		}

		DeleteFileA((sp + "/vm/encryption.cpp").c_str());
		DeleteFileA((sp + "/vm/encryption.h").c_str());

		if(isFirstCall) cout << "[il2cpp_lib_mgr] " << "restore libil2cpp succ" << endl;
	}
};