using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.AssetFile.Object
{
    public class ObjectInfo : IVersionAble<AssetsFileFormatVersion>
    {
        public AssetsFileFormatVersion Version { get; private set; }
        public long PathId { get; private set; }
        public long ByteOffset { get; private set; }
        public uint ByteCount { get; private set; }
        public SerializedType SerializedType { get; private set; }
        public int ClassId => SerializedType.ClassId;
        public Guid TypeGuid => SerializedType.TypeGuid;
        public ObjectInfo(AssetsFileFormatVersion _Version, long _PathId, long _ByteOffset, uint _ByteCount, SerializedType _SerializedType)
        {
            Version = _Version;
            PathId = _PathId;
            ByteOffset = _ByteOffset;
            ByteCount = _ByteCount;
            SerializedType = _SerializedType;
        }
        public ObjectInfo(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            _Reader.AlignStream();
            PathId = _Reader.ReadInt64();
            if (_AssetsFile.Version < AssetsFileFormatVersion.v2020_1_AndUp)
            {
                ByteOffset = _Reader.ReadUInt32();
            }
            else
            {
                ByteOffset = _Reader.ReadInt64();
            }

            ByteCount = _Reader.ReadUInt32();
            int index = _Reader.ReadInt32();
            SerializedType = _AssetsFileMetadata.SerializedTypes[index];
        }
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.AlignStream();
            _Writer.Write(PathId);
            if (_AssetsFile.Version < AssetsFileFormatVersion.v2020_1_AndUp)
            {
                _Writer.Write((uint)ByteOffset);
            }
            else
            {
                _Writer.Write((ulong)ByteOffset);
            }

            _Writer.Write(ByteCount);
            int value = -1;
            for (int i = 0; i < _AssetsFileMetadata.SerializedTypes.Count; i++)
            {
                if (SerializedType == _AssetsFileMetadata.SerializedTypes[i])
                {
                    value = i;
                    break;
                }
            }

            _Writer.Write(value);
        }
        public override int GetHashCode()
        {
            return PathId.GetHashCode() ^ ByteOffset.GetHashCode() ^ ByteCount.GetHashCode() ^ SerializedType.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is ObjectInfo))
            {
                return false;
            }

            if (PathId == ((ObjectInfo)_Other).PathId && ByteOffset == ((ObjectInfo)_Other).ByteOffset && ByteCount == ((ObjectInfo)_Other).ByteCount)
            {
                return SerializedType.ClassId == ((ObjectInfo)_Other).SerializedType.ClassId;
            }

            return false;
        }
    }
}
