using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Ether_Obfuscator.Unity;

namespace Ether_Obfuscator.Ofbuscators.UnityMonoBehavior
{
    public struct MonoSwapMap
    {
        public string OriginName;
        public string ObfusName;
        public bool Set;
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
            for (int i = 0; i < Maps.Count; i++)
            {
                for (int j = 0; j < MonoScriptList.Count; j++)
                {
                    if(MonoScriptList[j].Name == Maps[i].OriginName && MonoScriptList[j].AssemblyName == "Assembly-CSharp.dll")
                    {
                        MonoScriptList[j].UpdateType(MonoScriptList[j].AssemblyName, MonoScriptList[j].Namespace, Maps[i].ObfusName);
                        //Maps.Remove(Maps[i]);
                        Maps[i] = new MonoSwapMap
                        {
                            OriginName = Maps[i].OriginName,
                            ObfusName = Maps[i].ObfusName,
                            Set = true
                        };
                    }
                }
            }
            /*
            for (int i = 0; i < MonoScriptList.Count; i++)
            {
                string Name = MonoScriptList[i].Name;
                for(int j=0;j< Maps.Count;j++)
                {
                    if(Name == Maps[j].OriginName && MonoScriptList[j].AssemblyName == "Assembly-CSharp.dll")
                    {
                        MonoScriptList[j].UpdateType("Assembly-CSharp.dll", MonoScriptList[j].Namespace, Maps[j].ObfusName);
                        Maps.Remove(Maps[j]);
                    }
                }
            }
            */
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
