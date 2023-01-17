#include "utils.h"

#define CONFIG_FILE_NAME "EtherIl2cpp.ini"
#define CONFIG_ENCRYPTION L"Encryption"
#define CONFIG_DEBUG L"Debug"

namespace utils {
	void show_help() {
		cout << "\t--proclib-p     <libil2cpp_path>                                            \t生成EtherLibil2cpp" << endl;
		cout << "\t--restorelib-p  <libil2cpp_path>                                            \t还原libil2cpp" << endl;
		cout << "\t--enc-p         <il2cpp_binary> <metadata> <binary_output> <metadata_output>\t加密游戏本体" << endl;
		cout << "\t--version                                                                   \t获取当前版本" << endl;
		cout << "\t--apiversion                                                                \t获取当前调用Api版本" << endl;
	}

    void try_create_config() {
        ini.SetUnicode();
        if (_access(CONFIG_FILE_NAME, 0) != 0)
        {
            ini.SetValue(CONFIG_ENCRYPTION, L"Encrypt_Key", L"REPLACE_IT_WITH_A_CUSTOM_KEY");
            ini.SetBoolValue(CONFIG_ENCRYPTION, L"Enable_CheckSum_WIN32", true);
            ini.SetBoolValue(CONFIG_ENCRYPTION, L"Enable_CheckSum_ANDROID", false);
            ini.SetBoolValue(CONFIG_ENCRYPTION, L"Enable_StringsEncrypt", false);
            ini.SetBoolValue(CONFIG_DEBUG, L"Show_MB_Error", false);
            ini.SaveFile(CONFIG_FILE_NAME);
        }
        
    }

    void load_config() {
        try_create_config();
        ini.LoadFile(CONFIG_FILE_NAME);
        show_mb_err = ini.GetBoolValue(CONFIG_DEBUG, L"Show_MB_Error", false);
        wstring key = ini.GetValue(CONFIG_ENCRYPTION, L"Encrypt_Key");
        binary_encrypt_mgr::encrypt_key = utils::encode_getbytes(key);
        //ini.SaveFile("oz_il2cpp.ini");
    }

    bool is_show_mb_err() {
        return show_mb_err;
    }

    void read_file_lines(const char* fp, std::vector<string>& lines) {
        fstream fs;
        fs.open(fp, ios::in);
        if (!fs) {
            cout << "[utils::read_file_lines] Failed to open file "<<fp << endl;
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
            cout << "[utils::read_file_lines] Failed to open file " << fp << endl;
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
            printf("[utils::read_file] Failed to read file %s\n", path);
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
        fopen_s(&pFile, path, "wb+");
        if (pFile == NULL) {
            printf("[utils::write_file] Failed to open file %s\n", path);
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
            return false;
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
        read_file_lines(fp, l);
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

    wstring user_select_file(const wchar_t* title, const wchar_t* filter)
    {
        TCHAR szBuffer[MAX_PATH] = { 0 };
        OPENFILENAME file = { 0 };
        file.hwndOwner = NULL;
        file.lStructSize = sizeof(file);
        file.lpstrFilter = filter;//L"所有文件(*.*)\0*.*\0Exe文件(*.exe)\0*.exe\0";//要选择的文件后缀 
        file.lpstrInitialDir = L"";//默认的文件路径 
        file.lpstrFile = szBuffer;//存放文件的缓冲区 
        file.nMaxFile = sizeof(szBuffer) / sizeof(*szBuffer);
        file.nFilterIndex = 0;
        file.lpstrTitle = title;//L"114514";
        file.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST | OFN_EXPLORER;//标志如果是多选要加上OFN_ALLOWMULTISELECT
        BOOL bSel = GetOpenFileName(&file);

        //WideCharToMultiByte()
        return wstring(file.lpstrFile);
    }

    string user_select_file_a(const char* title, const char* filter) {
        std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
        wstring r = user_select_file(conv.from_bytes(title).c_str(), conv.from_bytes(filter).c_str());
        return conv.to_bytes(r);
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