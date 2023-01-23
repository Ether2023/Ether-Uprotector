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
    public static class MonoUtils
    {
        public static AssetsFile LoadAsset(string path)
        {
            UnityFileReader var_Reader = new UnityFileReader(path);
            AssetsFile var_AssetsFile = new AssetsFile(var_Reader);
            var_Reader.Close();
            return var_AssetsFile;
        }
        public static bool IsMonoBehaviour(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.MonoBehaviour") return true;
            else return false;
        }
        public static bool IsNetworkBehaviour(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.Networking.NetworkBehaviour") return true;
            else return false;
        }
        public static bool IsScriptableObject(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.ScriptableObject") return true;
            else return false;
        }
        public static bool IsPlayable(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.Playables.Playable") return true;
            else return false;
        }
        public static bool IsPlayableAsset(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.Playables.PlayableAsset") return true;
            else return false;
        }
        public static bool IsPlayableBehaviour(TypeDef type)
        {
            if (type.BaseType.FullName == "UnityEngine.Playables.PlayableBehaviour") return true;
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
        public static void SetMonoMapToAssetFile(AssetsFile assetsFile, Dictionary<string, string> Maps)
        {
            List<MonoScript> MonoScriptList = assetsFile.GetObjects<MonoScript>();

            for(int i = 0; i < MonoScriptList.Count; i++)
            {
                string str;
                if (Maps.ContainsKey(MonoScriptList[i].Name) && MonoScriptList[i].AssemblyName == "Assembly-CSharp.dll" && Maps.TryGetValue(MonoScriptList[i].Name,out str))
                {
                    MonoScriptList[i].UpdateType(MonoScriptList[i].AssemblyName, string.IsNullOrEmpty(MonoScriptList[i].Namespace) ? "" : MonoScriptList[i].Namespace, string.IsNullOrEmpty(str) ? "" : str);
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
    [Serializable]
    public class MonoClass
    {
        public string Assembly;
        public string Namespace;
        public string Name;
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
