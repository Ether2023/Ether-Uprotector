using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ether_UnityAsset.AssetFile.Object;
using Ether_UnityAsset.Endian;
using UnityEditor;
namespace Ether_UnityAsset.AssetFile
{
    public class AssetsFileHeader : IVersionAble<AssetsFileFormatVersion>
    {
        public ulong MetadataSize { get; set; }
        public long FileSize { get; set; }
        public AssetsFileFormatVersion Version { get; private set; }
        public long DataOffset { get; set; }
        public EndianType EndianType { get; private set; }
        public byte[] Reserved { get; private set; }
        public uint UnknownBlob { get; private set; }
        public uint FromBundle { get; private set; }
        public AssetsFileHeader(AssetsFile _AssetsFile, EndianBinaryReader _Reader)
        {
            MetadataSize = _Reader.ReadUInt32();
            FileSize = _Reader.ReadUInt32();
            Version = (AssetsFileFormatVersion)_Reader.ReadUInt32();
            DataOffset = _Reader.ReadUInt32();
            EndianType = ((_Reader.ReadByte() != 0) ? EndianType.BigEndian : EndianType.LittleEndian);
            Reserved = _Reader.ReadBytes(3);
            if (Version >= AssetsFileFormatVersion.v2020_1_AndUp)
            {
                MetadataSize = _Reader.ReadUInt32();
                FileSize = _Reader.ReadInt64();
                DataOffset = _Reader.ReadInt64();
                _Reader.Endian = EndianType;
                UnknownBlob = _Reader.ReadUInt32();
                FromBundle = _Reader.ReadUInt32();
            }
        }
        public void Write(AssetsFile _AssetsFile, EndianBinaryWriter _Writer)
        {
            if (Version < AssetsFileFormatVersion.v2020_1_AndUp)
            {
                _Writer.Write((uint)MetadataSize);
                _Writer.Write((uint)FileSize);
                _Writer.Write((uint)Version);
                _Writer.Write((uint)DataOffset);
            }
            else
            {
                _Writer.Write(0u);
                _Writer.Write(0u);
                _Writer.Write((uint)Version);
                _Writer.Write(0u);
            }

            _Writer.Write((byte)((EndianType != 0) ? 1 : 0));
            _Writer.Write(Reserved);
            if (Version >= AssetsFileFormatVersion.v2020_1_AndUp)
            {
                _Writer.Write((uint)MetadataSize);
                _Writer.Write(FileSize);
                _Writer.Write(DataOffset);
                _Writer.Endian = EndianType;
                _Writer.Write(UnknownBlob);
                _Writer.Write(FromBundle);
            }
        }
        public override int GetHashCode()
        {
            return MetadataSize.GetHashCode() ^ FileSize.GetHashCode() ^ Version.GetHashCode() ^ DataOffset.GetHashCode() ^ EndianType.GetHashCode() ^ Reserved.GetHashCode() ^ UnknownBlob.GetHashCode() ^ FromBundle.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is AssetsFileHeader))
            {
                return false;
            }

            if (MetadataSize == ((AssetsFileHeader)_Other).MetadataSize && FileSize == ((AssetsFileHeader)_Other).FileSize && Version == ((AssetsFileHeader)_Other).Version && DataOffset == ((AssetsFileHeader)_Other).DataOffset && EndianType == ((AssetsFileHeader)_Other).EndianType && Reserved.SequenceEqual(((AssetsFileHeader)_Other).Reserved) && UnknownBlob == ((AssetsFileHeader)_Other).UnknownBlob)
            {
                return FromBundle == ((AssetsFileHeader)_Other).FromBundle;
            }

            return false;
        }
    }
    public class AssetsFileMetadata : IVersionAble<AssetsFileFormatVersion>
    {
        public AssetsFileFormatVersion Version { get; private set; }
        public string UnityVersion { get; private set; }
        public BuildTarget TargetPlatform { get; private set; }
        public bool EnableTypeTree { get; private set; }
        public List<SerializedType> SerializedTypes { get; set; }
        public List<ObjectInfo> ObjectInfos { get; set; }
        public List<PreloadIdentifier> Preloads { get; set; }
        public List<FileIdentifier> Externals { get; set; }
        public List<SerializedType> RefactoredSerializedTypes { get; set; }
        public string UserInformation { get; private set; }
        public AssetsFileMetadata(AssetsFile _AssetsFile, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            UnityVersion = _Reader.ReadStringToNull();
            TargetPlatform = (BuildTarget)_Reader.ReadInt32();
            if (!Enum.IsDefined(typeof(BuildTarget), TargetPlatform))
            {
                TargetPlatform = BuildTarget.NoTarget;
            }

            EnableTypeTree = _Reader.ReadBoolean();
            int num = _Reader.ReadInt32();
            SerializedTypes = new List<SerializedType>();
            for (int i = 0; i < num; i++)
            {
                SerializedType item = new SerializedType(_AssetsFile, this, _Reader, _IsRefactoredType: false);
                SerializedTypes.Add(item);
            }

            int num2 = _Reader.ReadInt32();
            ObjectInfos = new List<ObjectInfo>();
            for (int j = 0; j < num2; j++)
            {
                ObjectInfo item2 = new ObjectInfo(_AssetsFile, this, _Reader);
                ObjectInfos.Add(item2);
            }

            int num3 = _Reader.ReadInt32();
            Preloads = new List<PreloadIdentifier>();
            for (int k = 0; k < num3; k++)
            {
                PreloadIdentifier item3 = new PreloadIdentifier(_AssetsFile, this, _Reader);
                Preloads.Add(item3);
            }

            int num4 = _Reader.ReadInt32();
            Externals = new List<FileIdentifier>(num4);
            for (int l = 0; l < num4; l++)
            {
                FileIdentifier item4 = new FileIdentifier(_AssetsFile, this, _Reader);
                Externals.Add(item4);
            }

            if (Version >= AssetsFileFormatVersion.v2019_2_AndUp)
            {
                int num5 = _Reader.ReadInt32();
                RefactoredSerializedTypes = new List<SerializedType>();
                for (int m = 0; m < num5; m++)
                {
                    SerializedType item5 = new SerializedType(_AssetsFile, this, _Reader, _IsRefactoredType: true);
                    RefactoredSerializedTypes.Add(item5);
                }
            }

            UserInformation = _Reader.ReadStringToNull();
        }
        public void Write(AssetsFile _AssetsFile, EndianBinaryWriter _Writer)
        {
            _Writer.WriteStringToNull(UnityVersion);
            _Writer.Write((int)TargetPlatform);
            _Writer.Write(EnableTypeTree);
            _Writer.Write(SerializedTypes.Count);
            for (int i = 0; i < SerializedTypes.Count; i++)
            {
                SerializedTypes[i].Write(_AssetsFile, this, _Writer);
            }

            _Writer.Write(ObjectInfos.Count);
            for (int j = 0; j < ObjectInfos.Count; j++)
            {
                ObjectInfos[j].Write(_AssetsFile, this, _Writer);
            }

            _Writer.Write(Preloads.Count);
            for (int k = 0; k < Preloads.Count; k++)
            {
                Preloads[k].Write(_AssetsFile, this, _Writer);
            }

            _Writer.Write(Externals.Count);
            for (int l = 0; l < Externals.Count; l++)
            {
                Externals[l].Write(_AssetsFile, this, _Writer);
            }

            _Writer.Write(RefactoredSerializedTypes.Count);
            for (int m = 0; m < RefactoredSerializedTypes.Count; m++)
            {
                RefactoredSerializedTypes[m].Write(_AssetsFile, this, _Writer);
            }

            _Writer.WriteStringToNull(UserInformation);
        }
        public override int GetHashCode()
        {
            return UnityVersion.GetHashCode() ^ TargetPlatform.GetHashCode() ^ EnableTypeTree.GetHashCode() ^ SerializedTypes.GetHashCode() ^ ObjectInfos.GetHashCode() ^ Preloads.GetHashCode() ^ Externals.GetHashCode() ^ RefactoredSerializedTypes.GetHashCode() ^ UserInformation.GetHashCode();
        }
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is AssetsFileMetadata))
            {
                return false;
            }

            if (UnityVersion == ((AssetsFileMetadata)_Other).UnityVersion && TargetPlatform == ((AssetsFileMetadata)_Other).TargetPlatform && EnableTypeTree == ((AssetsFileMetadata)_Other).EnableTypeTree && SerializedTypes.SequenceEqual(((AssetsFileMetadata)_Other).SerializedTypes) && ObjectInfos.SequenceEqual(((AssetsFileMetadata)_Other).ObjectInfos) && Preloads.SequenceEqual(((AssetsFileMetadata)_Other).Preloads) && Externals.SequenceEqual(((AssetsFileMetadata)_Other).Externals) && RefactoredSerializedTypes.SequenceEqual(((AssetsFileMetadata)_Other).RefactoredSerializedTypes))
            {
                return UserInformation == ((AssetsFileMetadata)_Other).UserInformation;
            }

            return false;
        }
    }
}
