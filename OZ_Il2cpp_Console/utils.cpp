#include "utils.h"

namespace utils {
	void show_help() {
		cout << "暂时懒得写了awa" << endl;
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
        std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
        while (getline(fs, line)) {
            wstring wb = conv.from_bytes(line);
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
                    cout << temfilename << endl;
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
                    cout << temfilename << endl;
                }
            } while (_findnext(hFile, &fileinfo) == 0);
            _findclose(hFile);
        }
    }

    size_t read_file(const char* path, char** buf) {
        FILE* pFile = NULL;
        long fileSize = 0;
        char* buffer;

        fopen_s(&pFile, path, "rb+");
        if (pFile == NULL) {
            printf("[utils::read_file] Failed to open file %s", path);
            return 0;
        }
        fseek(pFile, 0, SEEK_END);//读到最后
        fileSize = ftell(pFile);//获取当前位置（就是文件的大小了）
        rewind(pFile);//返回到0 pos
        buffer = (char*)malloc(sizeof(char) * fileSize + 1);
        if (buffer == NULL) {
            printf("[utils::read_file] Failed to open file %s", path);
            return 0;
        }
        size_t result = fread(buffer, 1, fileSize, pFile);
        if (result != fileSize) {
            printf("[utils::read_file] Failed to read file %s", path);
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
            printf("[utils::write_file] Failed to open file %s", path);
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
}