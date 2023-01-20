#include "str_encrypt_mgr.h"
using namespace std;

namespace str_encrypt_mgr {

	const char* get_string_encrypt(const char* str, int key) {
		char* c = (char*)malloc(strlen(str) + 1);
		strcpy_s(c, strlen(str) + 1, str);
		int final_key[10]{};
		int i, j;
		for (i = 0; i < 10; i++) {
			final_key[i] += (key^i*i + 63*i)%10;
		}
		for (i = 0, j = 0; c[j]; j++, i = (i + 1) % 10) {

			c[j] += final_key[i];

			if (c[j] > 122) c[j] -= 90;
		}
		return c;
	}

	string create_decrypt_get_method(string str) {
		mt19937 rand = mt19937(std::random_device()());
		int key = rand();
		string enced = string(get_string_encrypt(str.c_str(), key));

		// repl symbols such as ",',
		utils::str_replace_all(enced, "\\", "\\\\");
		utils::str_replace_all(enced, "\"", "\\\"");
		utils::str_replace_all(enced, "\'", "\\\'");
		utils::str_replace_all(enced, "\?", "\\\?");
		utils::str_replace_all(enced, "\?", "\\\?");

		string callmethod = "encryption::get_string_decrypt(\"";
		callmethod += enced;
		callmethod += "\", ";
		callmethod += to_string(key);
		callmethod += ")";
		return callmethod;
	}
}