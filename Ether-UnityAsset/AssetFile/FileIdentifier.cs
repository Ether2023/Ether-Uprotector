using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.AssetFile
{
    public class FileIdentifier : IVersionAble<AssetsFileFormatVersion>
    {
        public AssetsFileFormatVersion Version { get; private set; }
        public string BufferedPath { get; private set; }
        public Guid Guid { get; private set; }
        public int AssetType { get; private set; }
        public string PathName { get; private set; }
        public FileIdentifier(AssetsFileFormatVersion _Version, string _BufferedPath, Guid _Guid, int _AssetType, string _PathName)
        {
            Version = _Version;
            BufferedPath = _BufferedPath;
            Guid = _Guid;
            AssetType = _AssetType;
            PathName = _PathName;
        }
        public FileIdentifier(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            BufferedPath = _Reader.ReadStringToNull();
            Guid = new Guid(_Reader.ReadBytes(16));
            AssetType = _Reader.ReadInt32();
            PathName = _Reader.ReadStringToNull();
        }
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.WriteStringToNull(BufferedPath);
            _Writer.Write(Guid.ToByteArray());
            _Writer.Write(AssetType);
            _Writer.WriteStringToNull(PathName);
        }
        public override int GetHashCode()
        {
            return BufferedPath.GetHashCode() ^ Guid.GetHashCode() ^ AssetType.GetHashCode() ^ PathName.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is FileIdentifier))
            {
                return false;
            }

            if (BufferedPath == ((FileIdentifier)_Other).BufferedPath && Guid == ((FileIdentifier)_Other).Guid && AssetType == ((FileIdentifier)_Other).AssetType)
            {
                return PathName == ((FileIdentifier)_Other).PathName;
            }

            return false;
        }
    }
}
