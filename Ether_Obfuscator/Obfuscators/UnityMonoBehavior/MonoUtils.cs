using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Ether_Obfuscator.Unity;
using UnityEditor;
using MonoScript = Ether_Obfuscator.Unity.MonoScript;
namespace Ether_Obfuscator.Obfuscators.UnityMonoBehavior
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
        public static List<string> GetMonoBehaviorClass(AssetsFile assetsFile)
        {
            List<string> result = new List<string>();
            List<MonoScript> MonoScriptList = assetsFile.GetObjects<MonoScript>();
            foreach(var monoScript in MonoScriptList)
            {
                if(monoScript.AssemblyName == "Assembly-CSharp.dll")
                result.Add(monoScript.Name);
            }
            return result;
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
    public class MonoClass
    {
        public string Assembly { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public MonoClass(string _assembly,string _namespace,string name) {
            Assembly = _assembly;
            Namespace = _namespace;
            Name = name;
        }
    }
    public class UnityAssetReference
    {
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        public UnityAssetReference(string path)
        {
            Path = path;
            FileName = System.IO.Path.GetFileNameWithoutExtension(path);
            FileExtension = System.IO.Path.GetExtension(path);
        }
    }
}
