using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.AssetFile
{
    public class SerializedType : IVersionAble<AssetsFileFormatVersion>
    {
        public AssetsFileFormatVersion Version { get; private set; }
        public bool IsRefactoredType { get; private set; }
        //     MonoScript, ...
        public int ClassId { get; private set; }
        public bool IsStrippedType { get; private set; }
        public short ScriptTypeIndex { get; private set; }
        public Guid ScriptGuid { get; private set; }
        public Guid TypeGuid { get; private set; }
        public SerializedType(AssetsFileFormatVersion _Version, bool _IsRefactoredType, int _ClassId, bool _IsStrippedType, short _ScriptTypeIndex, Guid _ScriptGuid, Guid _TypeGuid)
        {
            Version = _Version;
            IsRefactoredType = _IsRefactoredType;
            ClassId = _ClassId;
            IsStrippedType = _IsStrippedType;
            ScriptTypeIndex = _ScriptTypeIndex;
            ScriptGuid = _ScriptGuid;
            TypeGuid = _TypeGuid;
        }
        public SerializedType(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader, bool _IsRefactoredType)
        {
            Version = _AssetsFile.Version;
            IsRefactoredType = _IsRefactoredType;
            ClassId = _Reader.ReadInt32();
            if (Version >= AssetsFileFormatVersion.v5_5_0a_AndUp)
            {
                IsStrippedType = _Reader.ReadBoolean();
            }

            if (Version >= AssetsFileFormatVersion.v5_5_0b_AndUp)
            {
                ScriptTypeIndex = _Reader.ReadInt16();
            }

            if (IsRefactoredType && ScriptTypeIndex >= 0)
            {
                ScriptGuid = new Guid(_Reader.ReadBytes(16));
            }
            else if ((Version < AssetsFileFormatVersion.v5_5_0a_AndUp && ClassId < 0) || (Version >= AssetsFileFormatVersion.v5_5_0a_AndUp && ClassId == 114))
            {
                ScriptGuid = new Guid(_Reader.ReadBytes(16));
            }

            TypeGuid = new Guid(_Reader.ReadBytes(16));
            if (_AssetsFileMetadata.EnableTypeTree)
            {
                throw new Exception("Reading type trees is not supported yet!");
            }
        }
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.Write(ClassId);
            if (Version >= AssetsFileFormatVersion.v5_5_0a_AndUp)
            {
                _Writer.Write(IsStrippedType);
            }

            if (Version >= AssetsFileFormatVersion.v5_5_0b_AndUp)
            {
                _Writer.Write(ScriptTypeIndex);
            }

            if (IsRefactoredType && ScriptTypeIndex >= 0)
            {
                _Writer.Write(ScriptGuid.ToByteArray());
            }
            else if ((Version < AssetsFileFormatVersion.v5_5_0a_AndUp && ClassId < 0) || (Version >= AssetsFileFormatVersion.v5_5_0a_AndUp && ClassId == 114))
            {
                _Writer.Write(ScriptGuid.ToByteArray());
            }

            _Writer.Write(TypeGuid.ToByteArray());
            if (_AssetsFileMetadata.EnableTypeTree)
            {
                throw new Exception("Writing type trees is not supported yet!");
            }
        }
        public override int GetHashCode()
        {
            return IsRefactoredType.GetHashCode() ^ ClassId.GetHashCode() ^ IsStrippedType.GetHashCode() ^ ScriptTypeIndex.GetHashCode() ^ ScriptGuid.GetHashCode() ^ TypeGuid.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is SerializedType))
            {
                return false;
            }

            if (IsRefactoredType == ((SerializedType)_Other).IsRefactoredType && ClassId == ((SerializedType)_Other).ClassId && IsStrippedType == ((SerializedType)_Other).IsStrippedType && ScriptTypeIndex == ((SerializedType)_Other).ScriptTypeIndex && ScriptGuid == ((SerializedType)_Other).ScriptGuid)
            {
                return TypeGuid == ((SerializedType)_Other).TypeGuid;
            }

            return false;
        }
    }
}
