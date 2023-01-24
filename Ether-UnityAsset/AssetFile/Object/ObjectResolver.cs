using Ether_UnityAsset.Endian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Ether_UnityAsset.AssetFile.Object
{
    public class ObjectReader : EndianBinaryReader
    {
        public AssetsFile AssetsFile { get; private set; }
        public ObjectInfo ObjectInfo { get; private set; }
        public BuildTarget BuildTarget => AssetsFile.AssetsFileMetadata.TargetPlatform;
        public ObjectReader(EndianBinaryReader _Reader, AssetsFile _AssetsFile, ObjectInfo _ObjectInfo)
            : this(_Reader.BaseStream, _Reader.Endian, _AssetsFile, _ObjectInfo)
        {
        }
        public ObjectReader(Stream _Stream, EndianType _EndianType, AssetsFile _AssetsFile, ObjectInfo _ObjectInfo)
            : base(_Stream, _EndianType)
        {
            AssetsFile = _AssetsFile;
            ObjectInfo = _ObjectInfo;
            base.Position = AssetsFile.Header.DataOffset + ObjectInfo.ByteOffset;
        }
    }
    public class ObjectWriter : EndianBinaryWriter
    {
        public AssetsFile AssetsFile { get; private set; }
        public BuildTarget BuildTarget => AssetsFile.AssetsFileMetadata.TargetPlatform;
        public ObjectWriter(EndianBinaryWriter _Writer, AssetsFile _AssetsFile)
            : this(_Writer.BaseStream, _Writer.Endian, _AssetsFile)
        {
        }
        public ObjectWriter(Stream _Stream, EndianType _EndianType, AssetsFile _AssetsFile)
            : base(_Stream, _EndianType)
        {
            AssetsFile = _AssetsFile;
        }
    }
}
