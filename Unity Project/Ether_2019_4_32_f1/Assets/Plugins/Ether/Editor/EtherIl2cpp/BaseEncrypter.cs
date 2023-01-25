using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace Ether.Il2cpp
{
    public class BaseEncrypter
    {
        public bool hasError = false;

        int uniqueId;

        public EtherIl2cppConfig config;

        public string FilePath
        {
            get; private set;
        }

        public string TempPath
        {
            get
            {
                string p = Application.persistentDataPath + "/" + uniqueId.ToString("0000000000") + "/";
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

        public BaseEncrypter(string path, EtherIl2cppConfig config)
        {
            FilePath = path;
            this.config = config;
            uniqueId = UnityEngine.Random.Range(1, int.MaxValue);
        }

        public virtual void Process()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new Exception("No file selected");
            }
            if (!File.Exists(FilePath))
            {
                throw new Exception("No file selected");
            }
            ProcessFile();

            if (hasError)
            {
                throw new Exception("Error occurred");
            }
            else PostProcess();

        }

        protected virtual void ProcessFile()
        {
            //such as zip file
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
            Utilitys.OpenDirectory(op);
        }

        protected void ErrorMsg(string s)
        {
            hasError = true;
            //删除临时文件夹
            Directory.Delete(TempPath, true);
        }
    }
}
