#include "metadata_mgr.h"

#define MAX_HEADER_LENGTH 512

namespace metadata_mgr
{
	void proc_metadata(const char* ifp, const char* ofp) {
		char* data_o;
		size_t s_o = utils::read_file(ifp, &data_o);

		if (s_o == 0) {
			cout << "[metadata_mgr] open file fail" << endl;
			return;
		}

		//Check sanity
		if (*((int*)data_o) != 0xFAB11BAF) {
			cout << "[metadata_mgr] not a invaild metadatafile" << endl;
			if (utils::is_show_mb_err()) {
				MessageBox(NULL, L"无效的metadata文件", L"错误", MB_ICONERROR);
			}
			return;
		}

		//多来点空间,反正1024就1kb
		char* data = (char*)malloc(s_o+1024);
		memcpy(data, data_o, s_o);
		mt19937 rand = mt19937(std::random_device()());
		//Fill
		for (int i = 0; i < 1024; i++) {
			data[i + s_o] = rand();
		}

		// 从这里开始,处理data

		// 先处理string,否则header变了就不知道了
		//proc_strings(data, s_o);
		//proc_stringslit(data, s_o);


		// 处理Header
		//vector<wstring> head_script;
		//utils::read_file_lines_w("./oz_scripts/gbmd_head.ozs", head_script);

		//This method is only for debug
		for (int i = 1; i <= 29; i++) {
			swap_header_int32(data, i, 60-i);
			cout << "[metadata_mgr] dbg:swap line " << i << " " << 60 - i << endl;
		}

		// 生成head
		generate_ozmetadata_header(data, s_o);

		utils::write_file(ofp, data, s_o + 1024);

		cout << "[metadata_mgr] Encrypt global-metadata.dat file succ" << endl;
	}

	void swap_header_int32(char* data, int p1,int p2) {
		int* head = (int*)data;
		int h = head[p1-1];
		head[p1-1] = head[p2-1];
		head[p2-1] = h;
	}

	void generate_ozmetadata_header(char* data, size_t len_o) {
		//Copy header
		//Start head = data + len_o;
		memcpy((data + len_o), data, MAX_HEADER_LENGTH);

		//Encrypt header
		/*size_t hl;
		xxtea_encrypt(data, MAX_HEADER_LENGTH, "o&z_il2cpp_encryption", &hl);*/
		const char* header_key = "o&z_il2cpp_encryption";
		int keylen = strlen(header_key);
		for (int i = 0; i < MAX_HEADER_LENGTH; i++) {
			data[i + len_o] ^= header_key[i % keylen];
		}

		//Create header
		FrontHeader* fh = (FrontHeader*)malloc(sizeof(FrontHeader));
		char sign[] = "o&z_il2cpp_protection";
		strcpy_s(fh->sign, sign);

		fh->length = MAX_HEADER_LENGTH;
		fh->offset = len_o;
		
		mt19937 rand = mt19937(std::random_device()());
		for (int i = 0; i < 32; i++) {
			fh->key[i] = rand();
			fh->key[i] = 0;
		}

		//Destroy original header
		for (int i = 0; i < 200; i++) {
			data[i] = rand();
			data[i] = 0;
		}

		memcpy(data, fh, sizeof(FrontHeader));
	}

	void proc_strings(char* data, size_t len) {
		// int32_t sanity; 
		// int32_t version;
		// int32_t stringLiteralOffset; // string data for managed code
		// int32_t stringLiteralCount;
		// int32_t stringLiteralDataOffset;
		// int32_t stringLiteralDataCount;
		// int32_t stringOffset; // string data for metadata
		// int32_t stringCount;
		int* head = (int*)data;
		int stringsOffset = head[6];
		int stringsCount = head[7];
		cout << "[metadata_mgr] StringsOffset : " << stringsOffset <<endl;
		cout << "[metadata_mgr] StringsCount : " << stringsCount <<endl;

		int key = 114;

		for (int i = 0; i < stringsCount; i++) {
			char x = *(data + stringsOffset +i);
			if (x != 0 && x != key) {
				*(data + stringsOffset +i) ^= key;
			}
		}
	}

	void proc_stringslit(char* data, size_t len) {
		int* head = (int*)data;
		int stringLiteralOffset = head[2]; // string data for managed code
		int stringLiteralCount = head[3];
		int stringLiteralDataOffset = head[4];
		int stringLiteralDataCount = head[5];


	}
};