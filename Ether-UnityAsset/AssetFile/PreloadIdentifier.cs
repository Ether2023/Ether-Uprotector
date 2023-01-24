using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.AssetFile
{
    public class PreloadIdentifier : IVersionAble<AssetsFileFormatVersion>
    {
        public AssetsFileFormatVersion Version { get; private set; }
        public int FileId { get; private set; }
        public long PathId { get; private set; }
        public PreloadIdentifier(AssetsFileFormatVersion _Version, int _FileId, long _PathId)
        {
            Version = _Version;
            FileId = _FileId;
            PathId = _PathId;
        }
        public PreloadIdentifier(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            FileId = _Reader.ReadInt32();
            _Reader.AlignStream();
            PathId = _Reader.ReadInt64();
            _Reader.AlignStream();
        }
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.Write(FileId);
            _Writer.AlignStream();
            _Writer.Write(PathId);
            _Writer.AlignStream();
        }
        public override int GetHashCode()
        {
            return FileId.GetHashCode() ^ PathId.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is PreloadIdentifier))
            {
                return false;
            }

            if (FileId == ((PreloadIdentifier)_Other).FileId)
            {
                return PathId == ((PreloadIdentifier)_Other).PathId;
            }

            return false;
        }
    }
}
