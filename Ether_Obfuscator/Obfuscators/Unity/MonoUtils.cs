using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Ether_UnityAsset;
using Ether_UnityAsset.AssetFile;
using Ether_UnityAsset.AssetFile.Object;
namespace Ether_Obfuscator.Obfuscators.Unity
{
    public static class MonoUtils
    {
        public static AssetsFile LoadAsset(string path)
        {
            UnityFileReader Reader = new UnityFileReader(path);
            AssetsFile Asset = new AssetsFile(Reader);
            Reader.Close();
            return Asset;
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
        public static void SetMonoMapToAssetFile(AssetsFile Asset, Dictionary<TypeKey, TypeKey> Map)
        {
            if (Asset == null)
            {
                throw new ArgumentNullException("Asset");
            }
            if (Map == null)
            {
                throw new ArgumentNullException("Map");
            }
            List<MonoScript> MonoScriptList = Asset.GetObjects<MonoScript>();
            for (int i = 0; i < MonoScriptList.Count; i++)
            {
                string Assembly = MonoScriptList[i].AssemblyName.Substring(0, MonoScriptList[i].AssemblyName.Length - 4);
                string Namespace = MonoScriptList[i].Namespace;
                string Name = MonoScriptList[i].Name;
                TypeKey Key = new TypeKey(Assembly, Namespace, Name);
                if (Map.ContainsKey(Key) && Map.TryGetValue(Key, out var ValueTypeKey))
                {
                    MonoScriptList[i].UpdateType(MonoScriptList[i].AssemblyName, string.IsNullOrEmpty(ValueTypeKey.Namespace) ? "" : ValueTypeKey.Namespace, string.IsNullOrEmpty(ValueTypeKey.Name) ? "" : ValueTypeKey.Name);
                }
            }
        }
        public static void SaveAssetsToFile(AssetsFile _AssetsFile, string _FilePath, bool _Override = true)
        {
            if (_AssetsFile == null)
            {
                throw new ArgumentNullException("AssetsFile");
            }
            if (string.IsNullOrEmpty(_FilePath))
            {
                throw new ArgumentNullException("FilePath");
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
    public class MonoType
    {
        public string Assembly;
        public string Namespace;
        public string Name;
        public MonoType(string _assembly,string _namespace,string name) {
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
