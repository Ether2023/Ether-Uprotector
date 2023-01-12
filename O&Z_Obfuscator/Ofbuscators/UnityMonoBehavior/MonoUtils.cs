using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OZ_Obfuscator.Unity;

namespace OZ_Obfuscator.Ofbuscators.UnityMonoBehavior
{
    public struct MonoSwapMap
    {
        public string OriginName;
        public string ObfusName;
    }
    public static class MonoUtils
    {
        public static AssetsFile LoadAsset(string path)
        {
            UnityFileReader var_Reader = new UnityFileReader(path);
            AssetsFile var_AssetsFile = new AssetsFile(var_Reader);
            var_Reader.Close();
            return var_AssetsFile;
        }
        public static bool MonoTypeCheck(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.MonoBehaviour") return true;
            else return false;
        }
        public static void SetMonoMapToAssetFile(AssetsFile assetsFile,List<MonoSwapMap> Maps)
        {
            List<MonoScript> MonoScriptList = assetsFile.GetObjects<MonoScript>();
            for (int i = 0; i < MonoScriptList.Count; i++)
            {
                string Name = MonoScriptList[i].Name;
                foreach(var Map in Maps)
                {
                    if(Name == Map.OriginName)
                    {
                        MonoScriptList[i].UpdateType(MonoScriptList[i].AssemblyName, MonoScriptList[i].Namespace, Map.ObfusName);
                    }
                }
            }
        }
        public static void SaveAssetsToFile(AssetsFile _AssetsFile, string _FilePath, bool _Override = true)
        {
            if (_AssetsFile == null)
            {
                throw new ArgumentNullException("_AssetsFile");
            }
            if (string.IsNullOrEmpty(_FilePath))
            {
                throw new ArgumentNullException("_FilePath");
            }
            if (File.Exists(_FilePath) && !_Override)
            {
                throw new FileNotFoundException("There is already a file at: " + _FilePath);
            }
            using (FileStream var_FileStream = new FileStream(_FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var_FileStream.SetLength(0L);
                using (UnityFileWriter var_Writer = new UnityFileWriter(_FilePath, var_FileStream))
                {
                    _AssetsFile.Write(var_Writer);
                }
            }
        }
    }
}
