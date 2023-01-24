using Ether_UnityAsset.AssetFile.Object;
using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.AssetFile
{
    public class AssetsFile : UnityFile<AssetsFileFormatVersion>
    {
        public AssetsFileHeader Header { get; private set; }
        public new AssetsFileFormatVersion Version => Header.Version;
        public AssetsFileMetadata AssetsFileMetadata { get; private set; }
        public List<IObject> Objects { get; private set; }
        public AssetsFile(UnityFileReader _Reader)
            : base(_Reader)
        {
            Header = new AssetsFileHeader(this, _Reader);
            _Reader.Endian = Header.EndianType;
            AssetsFileMetadata = new AssetsFileMetadata(this, _Reader);
            if (_Reader.Position < 4096)
            {
                while (_Reader.Position < 4096)
                {
                    _Reader.ReadByte();
                }
            }
            else if (_Reader.Position % 16 == 0L)
            {
                _Reader.Position += 16L;
            }
            else
            {
                _Reader.AlignStream(16);
            }

            Objects = new List<IObject>();
            List<ObjectInfo> list = AssetsFileMetadata.ObjectInfos.OrderBy((ObjectInfo o) => o.ByteOffset).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                ObjectInfo objectInfo = list[i];
                ObjectReader reader = new ObjectReader(_Reader, this, objectInfo);
                if (objectInfo.ClassId == 115)
                {
                    MonoScript item = new MonoScript(reader);
                    Objects.Add(item);
                }
                else
                {
                    DefaultObject item2 = new DefaultObject(reader);
                    Objects.Add(item2);
                }
            }
        }
        public List<TObject> GetObjects<TObject>() where TObject : IObject
        {
            return Objects.OfType<TObject>().ToList();
        }
        public override void Write(UnityFileWriter Writer)
        {
            base.Write(Writer);
            WriteObjects(out var _ObjectData, out var _ObjectInfos);
            int num = _ObjectData.Length;
            AssetsFileMetadata.ObjectInfos = _ObjectInfos;
            WriteMetadata(out var _Metadata);
            int num2 = _Metadata.Length;
            Header.MetadataSize = (ulong)num2;
            WriteHeader(out var _HeaderData);
            int i = _HeaderData.Length + num2;
            if (i >= 4096)
            {
                i = ((i % 16 != 0) ? (i + (16 - i % 16)) : (i + 16));
            }
            else
            {
                for (; i < 4096; i++)
                {
                }
            }

            int num3 = i + num;
            Header.FileSize = num3;
            WriteHeader(out _HeaderData);
            Writer.Write(_HeaderData);
            Writer.Write(_Metadata);
            while (Writer.Position < i)
            {
                Writer.Write(new byte[1]);
            }

            Writer.Write(_ObjectData);
        }
        private bool WriteObjects(out byte[] Object, out List<ObjectInfo> ObjectInfos)
        {
            MemoryStream memoryStream = new MemoryStream();
            ObjectWriter objectWriter = new ObjectWriter(memoryStream, Header.EndianType, this);
            List<ObjectInfo> list = new List<ObjectInfo>();
            for (int i = 0; i < Objects.Count; i++)
            {
                long pathId = Objects[i].PathId;
                long position = objectWriter.Position;
                Objects[i].Write(objectWriter);
                uint byteCount = (uint)(objectWriter.Position - position);
                SerializedType serializedType = null;
                for (int j = 0; j < AssetsFileMetadata.SerializedTypes.Count; j++)
                {
                    if (Objects[i].TypeGuid == AssetsFileMetadata.SerializedTypes[j].TypeGuid)
                    {
                        serializedType = AssetsFileMetadata.SerializedTypes[j];
                        break;
                    }
                }

                ObjectInfo item = new ObjectInfo(Version, pathId, position, byteCount, serializedType);
                list.Add(item);
                if (i != Objects.Count - 1)
                {
                    objectWriter.AlignStream(8);
                }
            }

            list = list.OrderBy((ObjectInfo o) => o.PathId).ToList();
            Object = memoryStream.ToArray();
            ObjectInfos = list;
            return true;
        }
        private bool WriteMetadata(out byte[] _Metadata)
        {
            MemoryStream memoryStream = new MemoryStream();
            EndianBinaryWriter writer = new EndianBinaryWriter(memoryStream, Header.EndianType);
            AssetsFileMetadata.Write(this, writer);
            _Metadata = memoryStream.ToArray();
            return true;
        }
        private bool WriteHeader(out byte[] _HeaderData)
        {
            MemoryStream memoryStream = new MemoryStream();
            EndianBinaryWriter writer = new EndianBinaryWriter(memoryStream);
            Header.Write(this, writer);
            _HeaderData = memoryStream.ToArray();
            return true;
        }
        public override int GetHashCode()
        {
            return Header.GetHashCode() ^ AssetsFileMetadata.GetHashCode() ^ Objects.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is AssetsFile))
            {
                return false;
            }

            if (Header.Equals(((AssetsFile)_Other).Header) && AssetsFileMetadata.Equals(((AssetsFile)_Other).AssetsFileMetadata))
            {
                return Objects.SequenceEqual(((AssetsFile)_Other).Objects);
            }

            return false;
        }
    }
}
