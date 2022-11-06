using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OZ_IL2CPP_GUI
{
    class BaseEncrypter
    {
        public bool hasError = false;

        int uniqueId;

        public string FilePath
        {
            get; private set;
        }

        public string TempPath
        {
            get
            {
                string p = Path.GetTempPath() + "/" + uniqueId.ToString("0000000000") + "/";
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
                return p;
            }
        }

        public virtual string GetOutputPath()
        {
            string ext = Path.GetExtension(FilePath);
            string p = Path.GetFileNameWithoutExtension(FilePath) + "_encrypted" + ext;

            p = Path.GetDirectoryName(FilePath) + "/" + p;

            string dir = Path.GetDirectoryName(p);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return p;
        }

        public BaseEncrypter(string path)
        {
            FilePath = path;

            //For debug only
            //Utilitys.ShowMsg(path);

            uniqueId = new Random().Next(int.MinValue, int.MaxValue);
        }

        public virtual void Process()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                Utilitys.ShowError("No file selected");
                return;
            }
            if (!File.Exists(FilePath))
            {
                Utilitys.ShowError("File not found");
                return;
            }
            ProcessFile();

            if (hasError)
            {
                Utilitys.ShowError("加密过程发生错误!");
            }
            else PostProcess();

        }

        protected virtual void ProcessFile()
        {
            //such as zip file
        }

        protected static void system(string cmd)
        {
            Utilitys.system(cmd);
        }

        protected static string InvokeOZIL2CPPSecurity(params string[] cmd)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string s in cmd)
            {
                sb.Append(" ");
                sb.Append(s);
            }
            string cmds = /*"\"O&Z_IL2CPP_Security.exe\"" + */sb.ToString();
            string res = Utilitys.ExecCmd("\"O&Z_IL2CPP_Security.exe\"", cmds);
            return res;
            //Utilitys.ShowMsg(res);
        }

        protected bool EncryptMetadataFile(string fp, string ofp)
        {
            string s = InvokeOZIL2CPPSecurity("\"" + fp + "\"", "Crypt", "\"" + ofp + "\"");

            if (!File.Exists(ofp))
            {
                ErrorMsg("加密Metadata错误:\n"+s);
                return false;
            }
            return true;
        }

        protected void PostProcess()
        {
            OutputFile(GetOutputPath());
        }

        protected virtual void OutputFile(string p)
        {
            //删除临时文件夹
            Directory.Delete(TempPath, true);

            string op;
            if (Directory.Exists(p))
                op = p;
            else op = Path.GetDirectoryName(p);
            Utilitys.ShowMsg("加密完成!");
            Utilitys.OpenDirectory(op);
        }

        protected void ErrorMsg(string s)
        {
            Utilitys.ShowError(s);
            hasError = true;
            try
            {
                //删除临时文件夹
                Directory.Delete(TempPath, true);
            }
            catch(Exception e)
            {
                Utilitys.ShowError($"删除临时文件夹错误\n路径:{TempPath}\nEx:{e.ToString()}");
            }
        }
    }
}
