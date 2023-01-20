#include <locale>
#include "utils.h"
#include "static_res.h"

#define CONFIG_FILE_NAME "EtherIl2cpp.ini"
#define CONFIG_ENCRYPTION "Encryption"

namespace utils {
	void show_help() {
		cout << "\t--proclib-p     <libil2cpp_path >                      \t生成EtherLibil2cpp" << endl;
		cout << "\t--restorelib-p  <libil2cpp_path >                      \t还原libil2cpp" << endl;
		cout << "\t--enc-android-p <apk_unpack_path> <config  >           \t加密apk" << endl;
		cout << "\t--enc-win-p     <game_path      > <exe_name> <config>  \t加密exe" << endl;
		cout << "\t--version                                              \t获取当前版本" << endl;
		cout << "\t--apiversion                                           \t获取当前调用Api版本" << endl;
	}

    void try_create_config() {
        ini.SetUnicode();
        if (_access(CONFIG_FILE_NAME, 0) != 0)
        {
            ini.SetValue(CONFIG_ENCRYPTION, "Encrypt_Key", "REPLACE_IT_WITH_A_CUSTOM_KEY");
            ini.SetBoolValue(CONFIG_ENCRYPTION, "Enable_CheckSum_WIN32", true);
            ini.SetBoolValue(CONFIG_ENCRYPTION, "Enable_CheckSum_ANDROID", false);
            ini.SetBoolValue(CONFIG_ENCRYPTION, "Enable_StringsEncrypt", false);
            ini.SaveFile(CONFIG_FILE_NAME);
        }
        
    }

    void load_config() {
        try_create_config();
        ini.LoadFile(CONFIG_FILE_NAME);
        string key = ini.GetValue(CONFIG_ENCRYPTION, "Encrypt_Key");
        binary_encrypt_mgr::encrypt_key = key;
        //ini.SaveFile("oz_il2cpp.ini");
    }

    void read_file_lines(const char* fp, std::vector<string>& lines) {
        fstream fs;
        fs.open(fp, ios::in);
        if (!fs) {
            string e = "[utils::read_file_lines] failed to open file " + string(fp);
            throw e.c_str();
        }
        string line;
        while (getline(fs, line)) {
            lines.push_back(line);
        }
        fs.close();
    }

    void read_file_lines_w(const char* fp, std::vector<wstring>& lines) {
        fstream fs;
        fs.open(fp, ios::in);
        if (!fs) {
            string e = "[utils::read_file_lines] failed to open file " + string(fp);
            throw e.c_str();
        }
        string line;
        while (getline(fs, line)) {
            wstring wb = utils::encode_getstring(line);
            lines.push_back(wb);
        }
        fs.close();
    }

    void find_fold(const char* mainDir, std::vector<string>& files){
        files.clear();
        intptr_t hFile; //win10 need long long or intptr_t, long will show error
        _finddata_t fileinfo;

        char findDir[_MAX_PATH];
        strcpy_s(findDir, mainDir);
        strcat_s(findDir, "\\*.*");

        if ((hFile = _findfirst(findDir, &fileinfo)) != -1)
        {
            do
            {
                if ((fileinfo.attrib & _A_SUBDIR))//find fold
                {
                    if (fileinfo.name[0] == '.') //avoid . ..
                        continue;
                    char filename[_MAX_PATH];
                    strcpy_s(filename, mainDir);
                    strcat_s(filename, "\\");
                    strcat_s(filename, fileinfo.name);
                    string temfilename = filename;
                    files.push_back(temfilename);
                    //cout << temfilename << endl;
                }
            } while (_findnext(hFile, &fileinfo) == 0);
            _findclose(hFile);
        }
    }

    void find_file(const char* mainDir, std::vector<string>& files)
    {
        files.clear();
        intptr_t hFile; //win10 need long long or intptr_t, long will show error
        _finddata_t fileinfo;

        char findDir[_MAX_PATH];
        strcpy_s(findDir, mainDir);
        strcat_s(findDir, "\\*.*");

        if ((hFile = _findfirst(findDir, &fileinfo)) != -1)
        {
            do
            {
                if (!(fileinfo.attrib & _A_SUBDIR))//find fold
                {
                    if (fileinfo.name[0] == '.') //avoid . ..
                        continue;
                    char filename[_MAX_PATH];
                    strcpy_s(filename, mainDir);
                    strcat_s(filename, "\\");
                    strcat_s(filename, fileinfo.name);
                    string temfilename = filename;
                    files.push_back(temfilename);
                    //cout << temfilename << endl;
                }
            } while (_findnext(hFile, &fileinfo) == 0);
            _findclose(hFile);
        }
    }

    size_t read_file(const char* path, char** buf) {
        FILE* pFile = NULL;
        long fileSize = 0;
        char* buffer;

        fopen_s(&pFile, path, "rb");
        if (pFile == NULL) {
            printf("[utils::read_file] Failed to open file %s\n", path);
            return 0;
        }
        fseek(pFile, 0, SEEK_END);//读到最后
        fileSize = ftell(pFile);//获取当前位置（就是文件的大小了）
        rewind(pFile);//返回到0 pos
        buffer = (char*)malloc(sizeof(char) * fileSize + 1);
        if (buffer == NULL) {
            exit(ERR_CODE_MEM);
        }
        size_t result = fread(buffer, 1, fileSize, pFile);
        if (result != fileSize) {
            printf("[utils::read_file] failed to read file %s\n", path);
            return 0;
        }
        *((char*)buffer + fileSize) = 0;
        *buf = buffer;
        if (pFile)
            fclose(pFile);
        return fileSize;
    }

    bool write_file(const char* path, const char* buf, size_t len) {
        FILE* pFile = NULL;
        errno_t er = fopen_s(&pFile, path, "wb+");
        if (pFile == NULL) {
            printf("[utils::write_file] failed to open file code %d path: %s\n", er, path);
            return false;
        }
        fwrite(buf, len, 1, pFile);
        fflush(pFile);
        fclose(pFile);
        return true;
    }

    bool write_file_lines(const char* path, vector<string> lines) {
        fstream fs;
        fs.open(path, ios::out);
        if (!fs) {
            string e = "[utils::write_file_lines] failed to open file " + string(path);
            throw e.c_str();
        }
        for (auto s : lines) {
            fs << s << endl;
        }
        fs.close();
        return true;
    }

    bool write_file_lines_w(const char* path, vector<wstring> lines) {
        fstream fs;
        fs.open(path, ios::out);
        if (!fs) {
            return false;
        }
        std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
        for (auto ws : lines) {
            string s = conv.to_bytes(ws);
            fs << s << endl;
        }
        fs.close();
        return true;
    }

    void read_file_int32_lines(const char* fp, vector<int>& v) {
        vector<string> l;
        //read_file_lines(fp, l);
        // TODO: improve this code
        if(0==strcmp(fp, "metadata_header_repl.txt")){
            utils::string_split(string((const char*)_metadata_header_repl_txt), "\n", l);
        }
        for (auto a : l) {
            v.push_back(atoi(a.c_str()));
        }
    }

    void dump(void* ptr, int buflen){
        unsigned char* buf = (unsigned char*)ptr;
        int i;
        for (i = 0; i < buflen; i++)
        {
            printf("%02x ", buf[i]);
        }
        printf("\n");
    }

    void hex_dump(void* ptr, int buflen){
        unsigned char* buf = (unsigned char*)ptr;
        int i, j;
        for (i = 0; i < buflen; i += 16)
        {
            printf("%06x: ", i);
            for (j = 0; j < 16; j++)
                if (i + j < buflen)
                    printf("%02x ", buf[i + j]);
                else
                    printf("   ");
            printf(" ");
            for (j = 0; j < 16; j++)
                if (i + j < buflen)
                    printf("%c", isprint(buf[i + j]) ? buf[i + j] : '.');
            printf("\n");
        }
    }

    void string_split(string str, const char* spl, vector<string>& v) {
        string st = str;
        while (st.find(spl) != string::npos) {
            int found = st.find(spl);
            v.push_back(st.substr(0, found));
            st = st.substr(found + 1);
        }
        v.push_back(st);
    }

    void string_split_w(wstring str, const wchar_t* spl, vector<wstring>& v) {
        wstring st = str;
        while (st.find(spl) != string::npos) {
            int found = st.find(spl);
            v.push_back(st.substr(0, found));
            st = st.substr(found + 1);
        }
        v.push_back(st);
    }

    wstring encode_getstring(string s) {
        std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
        return conv.from_bytes(s);
    }

    string encode_getbytes(wstring s) {
        std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
        return conv.to_bytes(s);
    }

    string& str_replace_all(string& src, const string old_value, const string new_value) {
        for (string::size_type pos(0); pos != string::npos; pos += new_value.length()) {
            if ((pos = src.find(old_value, pos)) != string::npos) {
                src.replace(pos, old_value.length(), new_value);
            }
            else break;
        }
        return src;
    }

    wstring& wstr_replace_all(wstring& src, const wstring old_value, const wstring new_value) {
        for (wstring::size_type pos(0); pos != wstring::npos; pos += new_value.length()) {
            if ((pos = src.find(old_value, pos)) != wstring::npos) {
                src.replace(pos, old_value.length(), new_value);
            }
            else break;
        }
        return src;
    }
}