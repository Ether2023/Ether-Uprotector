using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEditor;

namespace OZ_Obfuscator.Unity
{
    public interface IObject
    {
        //
        // 摘要:
        //     All objects have a unique path id in its file.
        long PathId { get; }

        //
        // 摘要:
        //     The guid of the type this object belongs too.
        Guid TypeGuid { get; }

        //
        // 摘要:
        //     Write the object to a stream.
        //
        // 参数:
        //   _Writer:
        void Write(ObjectWriter _Writer);
    }
    public enum AssetsFileFormatVersion
    {
        //
        // 摘要:
        //     Added support for stripped objects.
        v5_0_1_AndUp = 0xF,
        //
        // 摘要:
        //     Refactoring of class id.
        v5_5_0a_AndUp,
        //
        // 摘要:
        //     Refactoring type data.
        v5_5_0b_AndUp,
        //
        // 摘要:
        //     Refactoring of shareable type tree data.
        v2019_1a_AndUp,
        //
        // 摘要:
        //     Added flags for type trees nodes.
        v2019_1_AndUp,
        //
        // 摘要:
        //     Refactoring of serialized types.
        v2019_2_AndUp,
        //
        // 摘要:
        //     Added storing of type dependencies.
        v2019_3_AndUp,
        //
        // 摘要:
        //     Added support for large files.
        v2020_1_AndUp
    }
    public class AssetsFile : UnityFile<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     The header of the assets file.
        public AssetsFileHeader AssetsFileHeader { get; private set; }

        //
        // 摘要:
        //     The version is inside the header.
        public new AssetsFileFormatVersion Version => AssetsFileHeader.Version;

        //
        // 摘要:
        //     The metadata of the assets file.
        public AssetsFileMetadata AssetsFileMetadata { get; private set; }

        //
        // 摘要:
        //     The unity objects inside the assets file.
        public List<IObject> Objects { get; private set; }

        //
        // 摘要:
        //     Read the bundle file data from the _Reader.
        //
        // 参数:
        //   _Reader:
        public AssetsFile(UnityFileReader _Reader)
            : base(_Reader)
        {
            AssetsFileHeader = new AssetsFileHeader(this, _Reader);
            _Reader.Endian = AssetsFileHeader.EndianType;
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

        //
        // 摘要:
        //     Return a list of objects int the asset file of type TObject.
        //
        // 类型参数:
        //   TObject:
        public List<TObject> GetObjects<TObject>() where TObject : IObject
        {
            return Objects.OfType<TObject>().ToList();
        }

        //
        // 摘要:
        //     Write the assets file.
        //
        // 参数:
        //   _Writer:
        public override void Write(UnityFileWriter _Writer)
        {
            base.Write(_Writer);
            WriteObjects(out var _ObjectData, out var _ObjectInfos);
            int num = _ObjectData.Length;
            AssetsFileMetadata.ObjectInfos = _ObjectInfos;
            WriteMetadata(out var _Metadata);
            int num2 = _Metadata.Length;
            AssetsFileHeader.MetadataSize = (ulong)num2;
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
            AssetsFileHeader.FileSize = num3;
            WriteHeader(out _HeaderData);
            _Writer.Write(_HeaderData);
            _Writer.Write(_Metadata);
            while (_Writer.Position < i)
            {
                _Writer.Write(new byte[1]);
            }

            _Writer.Write(_ObjectData);
        }

        //
        // 摘要:
        //     Writes the objects in _ObjectData and returns the object infos. Returns if the
        //     writing succeded.
        //
        // 参数:
        //   _ObjectData:
        //
        //   _ObjectInfos:
        private bool WriteObjects(out byte[] _ObjectData, out List<ObjectInfo> _ObjectInfos)
        {
            MemoryStream memoryStream = new MemoryStream();
            ObjectWriter objectWriter = new ObjectWriter(memoryStream, AssetsFileHeader.EndianType, this);
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
            _ObjectData = memoryStream.ToArray();
            _ObjectInfos = list;
            return true;
        }

        //
        // 摘要:
        //     Write the metadata to a stream and return it over _Metadata.
        //
        // 参数:
        //   _Metadata:
        private bool WriteMetadata(out byte[] _Metadata)
        {
            MemoryStream memoryStream = new MemoryStream();
            EndianBinaryWriter writer = new EndianBinaryWriter(memoryStream, AssetsFileHeader.EndianType);
            AssetsFileMetadata.Write(this, writer);
            _Metadata = memoryStream.ToArray();
            return true;
        }

        //
        // 摘要:
        //     Write the header to a stream and return it over _HeaderData.
        //
        // 参数:
        //   _HeaderData:
        private bool WriteHeader(out byte[] _HeaderData)
        {
            MemoryStream memoryStream = new MemoryStream();
            EndianBinaryWriter writer = new EndianBinaryWriter(memoryStream);
            AssetsFileHeader.Write(this, writer);
            _HeaderData = memoryStream.ToArray();
            return true;
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return AssetsFileHeader.GetHashCode() ^ AssetsFileMetadata.GetHashCode() ^ Objects.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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

            if (AssetsFileHeader.Equals(((AssetsFile)_Other).AssetsFileHeader) && AssetsFileMetadata.Equals(((AssetsFile)_Other).AssetsFileMetadata))
            {
                return Objects.SequenceEqual(((AssetsFile)_Other).Objects);
            }

            return false;
        }
    }
    public class AssetsFileHeader : IVersionAble<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     Size of the metadata in bytes.
        public ulong MetadataSize { get; set; }

        //
        // 摘要:
        //     Whole file size in bytes.
        public long FileSize { get; set; }

        //
        // 摘要:
        //     The assets file version.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     Byte offset for the objects.
        public long DataOffset { get; set; }

        //
        // 摘要:
        //     The endian of the byte data.
        public EndianType EndianType { get; private set; }

        //
        // 摘要:
        //     Unkown.
        public byte[] Reserved { get; private set; }

        //
        // 摘要:
        //     Unkown.
        public uint UnknownBlob { get; private set; }

        //
        // 摘要:
        //     Is from a bundle.
        public uint FromBundle { get; private set; }

        //
        // 摘要:
        //     Create a assets file header, by passing the owning _AssetsFile and _Reader, to
        //     read data from.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _Reader:
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

        //
        // 摘要:
        //     Write the assets file header, belonging to _AssetsFile with _Writer.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _Writer:
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

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return MetadataSize.GetHashCode() ^ FileSize.GetHashCode() ^ Version.GetHashCode() ^ DataOffset.GetHashCode() ^ EndianType.GetHashCode() ^ Reserved.GetHashCode() ^ UnknownBlob.GetHashCode() ^ FromBundle.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
        //
        // 摘要:
        //     Version to read and write the metadata.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     The unity version of the assets file.
        public string UnityVersion { get; private set; }

        //
        // 摘要:
        //     The unity target platform of the assets file.
        public BuildTarget TargetPlatform { get; private set; }

        //
        // 摘要:
        //     If type tree is enabled, serialized types map their whole field/properties and
        //     dependencies, to allow comptibility between unity versions.
        public bool EnableTypeTree { get; private set; }

        //
        // 摘要:
        //     Serialized types.
        public List<SerializedType> SerializedTypes { get; set; }

        //
        // 摘要:
        //     Info about the serialized objects.
        public List<ObjectInfo> ObjectInfos { get; set; }

        //
        // 摘要:
        //     List of objects need to be preloaded.
        public List<PreloadIdentifier> Preloads { get; set; }

        //
        // 摘要:
        //     List over the external file references.
        public List<FileIdentifier> Externals { get; set; }

        //
        // 摘要:
        //     Refeactored serialized types.
        public List<SerializedType> RefactoredSerializedTypes { get; set; }

        //
        // 摘要:
        //     User informations that are always empty.
        public string UserInformation { get; private set; }

        //
        // 摘要:
        //     Create a assets file metadata, by passing the owning _AssetsFile and _Reader,
        //     to read data from.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _Reader:
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

        //
        // 摘要:
        //     Write the assets file metadata, belonging to _AssetsFile with _Writer.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _Writer:
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

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return UnityVersion.GetHashCode() ^ TargetPlatform.GetHashCode() ^ EnableTypeTree.GetHashCode() ^ SerializedTypes.GetHashCode() ^ ObjectInfos.GetHashCode() ^ Preloads.GetHashCode() ^ Externals.GetHashCode() ^ RefactoredSerializedTypes.GetHashCode() ^ UserInformation.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
    public class SerializedType : IVersionAble<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     The serialized version of the type.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     The type is the newer refactored version.
        public bool IsRefactoredType { get; private set; }

        //
        // 摘要:
        //     Unity class id - Can be anything from GameObject, GameObject, MonoBehaviour,
        //     MonoScript, ...
        public int ClassId { get; private set; }

        //
        // 摘要:
        //     Is stripped outside (older unity version I think).
        public bool IsStrippedType { get; private set; }

        //
        // 摘要:
        //     Index to a script type.
        public short ScriptTypeIndex { get; private set; }

        //
        // 摘要:
        //     A Hash128 Guid. Representing the newer used script guid instead of type guid.
        public Guid ScriptGuid { get; private set; }

        //
        // 摘要:
        //     A Hash128 Guid. Representing the type guid.
        public Guid TypeGuid { get; private set; }

        //
        // 摘要:
        //     Create a serialized type by parameter.
        //
        // 参数:
        //   _Version:
        //
        //   _IsRefactoredType:
        //
        //   _ClassId:
        //
        //   _IsStrippedType:
        //
        //   _ScriptTypeIndex:
        //
        //   _ScriptGuid:
        //
        //   _TypeGuid:
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

        //
        // 摘要:
        //     Create a serialized type, by passing the owning _AssetsFile, _AssetsFileMetadata,
        //     _Reader, to read data from and a flag if it is the newer refactored type.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Reader:
        //
        //   _IsRefactoredType:
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

        //
        // 摘要:
        //     Write a serialized type belonging to a metadata.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Writer:
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

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return IsRefactoredType.GetHashCode() ^ ClassId.GetHashCode() ^ IsStrippedType.GetHashCode() ^ ScriptTypeIndex.GetHashCode() ^ ScriptGuid.GetHashCode() ^ TypeGuid.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
    public class PreloadIdentifier : IVersionAble<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     The serialized version of the type.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     The file id. 0: The current read assets file. 1...n: External file id.
        public int FileId { get; private set; }

        //
        // 摘要:
        //     The path id. Each object has a unique (in the file) path id.
        public long PathId { get; private set; }

        //
        // 摘要:
        //     Create a script identifier by parameters.
        //
        // 参数:
        //   _Version:
        //
        //   _FileId:
        //
        //   _PathId:
        public PreloadIdentifier(AssetsFileFormatVersion _Version, int _FileId, long _PathId)
        {
            Version = _Version;
            FileId = _FileId;
            PathId = _PathId;
        }

        //
        // 摘要:
        //     Create a script identifier, by passing the owning _AssetsFile, _AssetsFileMetadata,
        //     _Reader, to read data from.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Reader:
        public PreloadIdentifier(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            FileId = _Reader.ReadInt32();
            _Reader.AlignStream();
            PathId = _Reader.ReadInt64();
            _Reader.AlignStream();
        }

        //
        // 摘要:
        //     Write a script identifier.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Writer:
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.Write(FileId);
            _Writer.AlignStream();
            _Writer.Write(PathId);
            _Writer.AlignStream();
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return FileId.GetHashCode() ^ PathId.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
    public class FileIdentifier : IVersionAble<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     The serialized version of the type.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     The buffered path, mostly empty.
        public string BufferedPath { get; private set; }

        //
        // 摘要:
        //     The file guid.
        public Guid Guid { get; private set; }

        //
        // 摘要:
        //     The asset type.
        public int AssetType { get; private set; }

        //
        // 摘要:
        //     The file path.
        public string PathName { get; private set; }

        //
        // 摘要:
        //     Create a file identifier by parameter.
        //
        // 参数:
        //   _Version:
        //
        //   _BufferedPath:
        //
        //   _Guid:
        //
        //   _AssetType:
        //
        //   _PathName:
        public FileIdentifier(AssetsFileFormatVersion _Version, string _BufferedPath, Guid _Guid, int _AssetType, string _PathName)
        {
            Version = _Version;
            BufferedPath = _BufferedPath;
            Guid = _Guid;
            AssetType = _AssetType;
            PathName = _PathName;
        }

        //
        // 摘要:
        //     Create a file identifier, by passing the owning _AssetsFile, _AssetsFileMetadata,
        //     _Reader, to read data from.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Reader:
        public FileIdentifier(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryReader _Reader)
        {
            Version = _AssetsFile.Version;
            BufferedPath = _Reader.ReadStringToNull();
            Guid = new Guid(_Reader.ReadBytes(16));
            AssetType = _Reader.ReadInt32();
            PathName = _Reader.ReadStringToNull();
        }

        //
        // 摘要:
        //     Write a file identifier.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Writer:
        public void Write(AssetsFile _AssetsFile, AssetsFileMetadata _AssetsFileMetadata, EndianBinaryWriter _Writer)
        {
            _Writer.WriteStringToNull(BufferedPath);
            _Writer.Write(Guid.ToByteArray());
            _Writer.Write(AssetType);
            _Writer.WriteStringToNull(PathName);
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return BufferedPath.GetHashCode() ^ Guid.GetHashCode() ^ AssetType.GetHashCode() ^ PathName.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
    public class DefaultObject : IObject
    {
        //
        // 摘要:
        //     All data of the object.
        public byte[] DataBlob;

        //
        // 摘要:
        //     All objects have a path id (is not unique!).
        public long PathId { get; private set; }

        //
        // 摘要:
        //     The guid of the type this object belongs too.
        public Guid TypeGuid { get; private set; }

        //
        // 摘要:
        //     Read the object data from _Reader.
        //
        // 参数:
        //   _Reader:
        public DefaultObject(ObjectReader _Reader)
        {
            PathId = _Reader.ObjectInfo.PathId;
            TypeGuid = _Reader.ObjectInfo.TypeGuid;
            DataBlob = _Reader.ReadBytes((int)_Reader.ObjectInfo.ByteCount);
        }

        //
        // 摘要:
        //     Write the object data.
        //
        // 参数:
        //   _Writer:
        public void Write(ObjectWriter _Writer)
        {
            _Writer.Write(DataBlob);
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return DataBlob.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is DefaultObject))
            {
                return false;
            }

            return DataBlob.SequenceEqual(((DefaultObject)_Other).DataBlob);
        }
    }
    public class ObjectInfo : IVersionAble<AssetsFileFormatVersion>
    {
        //
        // 摘要:
        //     The serialized version of the type.
        public AssetsFileFormatVersion Version { get; private set; }

        //
        // 摘要:
        //     All objects have a unique path id in its file.
        public long PathId { get; private set; }

        //
        // 摘要:
        //     The byte offsets for the objects data. Does not include the metadata data offset!
        public long ByteOffset { get; private set; }

        //
        // 摘要:
        //     The amount of bytes this objects consists of.
        public uint ByteCount { get; private set; }

        //
        // 摘要:
        //     The serialized type.
        public SerializedType SerializedType { get; private set; }

        //
        // 摘要:
        //     The unity class id of the serialized type.
        public int ClassId => SerializedType.ClassId;

        //
        // 摘要:
        //     The unique guid of the serialized type. Because there can be multiple custom
        //     serialized types sharing the same class ids (for example being of class MonoBehaviour).
        public Guid TypeGuid => SerializedType.TypeGuid;

        //
        // 摘要:
        //     Create a object info directly by data.
        //
        // 参数:
        //   _Version:
        //
        //   _PathId:
        //
        //   _ByteOffset:
        //
        //   _ByteCount:
        //
        //   _SerializedType:
        public ObjectInfo(AssetsFileFormatVersion _Version, long _PathId, long _ByteOffset, uint _ByteCount, SerializedType _SerializedType)
        {
            Version = _Version;
            PathId = _PathId;
            ByteOffset = _ByteOffset;
            ByteCount = _ByteCount;
            SerializedType = _SerializedType;
        }

        //
        // 摘要:
        //     Create a object info, by passing the owning _AssetsFile, _AssetsFileMetadata,
        //     _Reader, to read data from.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Reader:
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

        //
        // 摘要:
        //     Write a object info belongign to _AssetsFileMetadata.
        //
        // 参数:
        //   _AssetsFile:
        //
        //   _AssetsFileMetadata:
        //
        //   _Writer:
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

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return PathId.GetHashCode() ^ ByteOffset.GetHashCode() ^ ByteCount.GetHashCode() ^ SerializedType.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
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
    public class ObjectWriter : EndianBinaryWriter
    {
        //
        // 摘要:
        //     The belonging assets file.
        public AssetsFile AssetsFile { get; private set; }

        //
        // 摘要:
        //     The build target, read form the metafiles.
        public BuildTarget BuildTarget => AssetsFile.AssetsFileMetadata.TargetPlatform;

        //
        // 摘要:
        //     The object writer needs an endian writer and the assets file.
        //
        // 参数:
        //   _Writer:
        //
        //   _AssetsFile:
        public ObjectWriter(EndianBinaryWriter _Writer, AssetsFile _AssetsFile)
            : this(_Writer.BaseStream, _Writer.Endian, _AssetsFile)
        {
        }

        //
        // 摘要:
        //     The object writer needs a stream, endian and the assets file.
        //
        // 参数:
        //   _Stream:
        //
        //   _EndianType:
        //
        //   _AssetsFile:
        public ObjectWriter(Stream _Stream, EndianType _EndianType, AssetsFile _AssetsFile)
            : base(_Stream, _EndianType)
        {
            AssetsFile = _AssetsFile;
        }
    }
    public class ObjectReader : EndianBinaryReader
    {
        //
        // 摘要:
        //     The belonging assets file.
        public AssetsFile AssetsFile { get; private set; }

        //
        // 摘要:
        //     The belonging object info.
        public ObjectInfo ObjectInfo { get; private set; }

        //
        // 摘要:
        //     The build target, read form the metafiles.
        public BuildTarget BuildTarget => AssetsFile.AssetsFileMetadata.TargetPlatform;

        //
        // 摘要:
        //     The object reader needs a endian reader, the assets file and the object info.
        //
        // 参数:
        //   _Reader:
        //
        //   _AssetsFile:
        //
        //   _ObjectInfo:
        public ObjectReader(EndianBinaryReader _Reader, AssetsFile _AssetsFile, ObjectInfo _ObjectInfo)
            : this(_Reader.BaseStream, _Reader.Endian, _AssetsFile, _ObjectInfo)
        {
        }

        //
        // 摘要:
        //     The object reader needs a stream, endian, the assets file and the object info.
        //
        // 参数:
        //   _Stream:
        //
        //   _EndianType:
        //
        //   _AssetsFile:
        //
        //   _ObjectInfo:
        public ObjectReader(Stream _Stream, EndianType _EndianType, AssetsFile _AssetsFile, ObjectInfo _ObjectInfo)
            : base(_Stream, _EndianType)
        {
            AssetsFile = _AssetsFile;
            ObjectInfo = _ObjectInfo;
            base.Position = AssetsFile.AssetsFileHeader.DataOffset + ObjectInfo.ByteOffset;
        }
    }
    public sealed class MonoScript : NamedObject
    {
        //
        // 摘要:
        //     The order of execution while runtime.
        public int ExecutionOrder { get; set; }

        //
        // 摘要:
        //     The guid.
        public Guid PropertyGuid { get; private set; }

        //
        // 摘要:
        //     The class name.
        public string ClassName { get; private set; }

        //
        // 摘要:
        //     The namespace.
        public string Namespace { get; private set; }

        //
        // 摘要:
        //     The assembly name.
        public string AssemblyName { get; private set; }

        //
        // 摘要:
        //     Create a monoscript by parameter.
        //
        // 参数:
        //   _PathId:
        //
        //   _TypeGuid:
        //
        //   _Name:
        //
        //   _ExecutionOrder:
        //
        //   _PropertyGuid:
        //
        //   _ClassName:
        //
        //   _Namespace:
        //
        //   _AssemblyName:
        public MonoScript(long _PathId, Guid _TypeGuid, string _Name, int _ExecutionOrder, Guid _PropertyGuid, string _ClassName, string _Namespace, string _AssemblyName)
            : base(_PathId, _TypeGuid, _Name)
        {
            ExecutionOrder = _ExecutionOrder;
            PropertyGuid = _PropertyGuid;
            ClassName = _ClassName;
            Namespace = _Namespace;
            AssemblyName = _AssemblyName;
        }

        //
        // 摘要:
        //     Read the objects data from _Reader.
        //
        // 参数:
        //   _Reader:
        public MonoScript(ObjectReader _Reader)
            : base(_Reader)
        {
            ExecutionOrder = _Reader.ReadInt32();
            PropertyGuid = new Guid(_Reader.ReadBytes(16));
            ClassName = _Reader.ReadAlignedString();
            Namespace = _Reader.ReadAlignedString();
            AssemblyName = _Reader.ReadAlignedString();
        }

        //
        // 摘要:
        //     Update the mono scripts type.
        public void UpdateType(string _AssemblyName, string _Namespace, string _TypeName)
        {
            base.Name = _TypeName;
            ClassName = _TypeName;
            Namespace = _Namespace;
            AssemblyName = _AssemblyName;
        }

        //
        // 摘要:
        //     Write the object data.
        //
        // 参数:
        //   _Writer:
        public override void Write(ObjectWriter _Writer)
        {
            base.Write(_Writer);
            _Writer.Write(ExecutionOrder);
            _Writer.Write(PropertyGuid.ToByteArray());
            _Writer.WriteAlignedString(ClassName);
            _Writer.WriteAlignedString(Namespace);
            _Writer.WriteAlignedString(AssemblyName);
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return ExecutionOrder.GetHashCode() ^ PropertyGuid.GetHashCode() ^ ClassName.GetHashCode() ^ Namespace.GetHashCode() ^ AssemblyName.GetHashCode() ^ base.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
        public override bool Equals(object _Other)
        {
            if (!base.Equals(_Other))
            {
                return false;
            }

            if (!(_Other is MonoScript))
            {
                return false;
            }

            if (ExecutionOrder == ((MonoScript)_Other).ExecutionOrder && PropertyGuid == ((MonoScript)_Other).PropertyGuid && ClassName == ((MonoScript)_Other).ClassName && Namespace == ((MonoScript)_Other).Namespace)
            {
                return AssemblyName == ((MonoScript)_Other).AssemblyName;
            }

            return false;
        }
    }
    public class NamedObject : EditorExtension
    {
        //
        // 摘要:
        //     The objects name.
        public string Name { get; set; }

        //
        // 摘要:
        //     Create a named object by parameter.
        //
        // 参数:
        //   _PathId:
        //
        //   _TypeGuid:
        //
        //   _Name:
        public NamedObject(long _PathId, Guid _TypeGuid, string _Name)
            : base(_PathId, _TypeGuid)
        {
            Name = _Name;
        }

        //
        // 摘要:
        //     Read the objects data from _Reader.
        //
        // 参数:
        //   _Reader:
        public NamedObject(ObjectReader _Reader)
            : base(_Reader)
        {
            Name = _Reader.ReadAlignedString();
        }

        //
        // 摘要:
        //     Write the object data.
        //
        // 参数:
        //   _Writer:
        public override void Write(ObjectWriter _Writer)
        {
            base.Write(_Writer);
            _Writer.WriteAlignedString(Name);
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ base.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
        public override bool Equals(object _Other)
        {
            if (!base.Equals(_Other))
            {
                return false;
            }

            if (!(_Other is NamedObject))
            {
                return false;
            }

            return Name == ((NamedObject)_Other).Name;
        }
    }
    public abstract class EditorExtension : Object
    {
        //
        // 摘要:
        //     The origin prefab object.
        public PPtr<EditorExtension> PrefabParentObject { get; private set; }

        //
        // 摘要:
        //     A internal unity prefab.
        public PPtr<Object> PrefabInternal { get; private set; }

        //
        // 摘要:
        //     Create an editor extension by parameter.
        //
        // 参数:
        //   _PathId:
        //
        //   _TypeGuid:
        public EditorExtension(long _PathId, Guid _TypeGuid)
            : base(_PathId, _TypeGuid)
        {
        }

        //
        // 摘要:
        //     Read the object data from _Reader.
        //
        // 参数:
        //   _Reader:
        public EditorExtension(ObjectReader _Reader)
            : base(_Reader)
        {
            if (_Reader.BuildTarget == BuildTarget.NoTarget)
            {
                PrefabParentObject = new PPtr<EditorExtension>(_Reader);
                PrefabInternal = new PPtr<Object>(_Reader);
            }
        }

        //
        // 摘要:
        //     Write the object data.
        //
        // 参数:
        //   _Writer:
        public override void Write(ObjectWriter _Writer)
        {
            base.Write(_Writer);
            if (_Writer.BuildTarget == BuildTarget.NoTarget)
            {
                PrefabParentObject.Write(_Writer);
                PrefabInternal.Write(_Writer);
            }
        }
    }
    public sealed class PPtr<T> where T : Object
    {
        //
        // 摘要:
        //     The file id. 0: The current read assets file. 1...n: External file id.
        public int FileId;

        //
        // 摘要:
        //     The path id. Each object has a unique (in the file) path id.
        public long PathId;

        //
        // 摘要:
        //     Is a reference or not.
        public bool IsNull
        {
            get
            {
                if (FileId >= 0)
                {
                    return PathId == 0;
                }

                return true;
            }
        }

        //
        // 摘要:
        //     Read the object pointer data from _Reader.
        //
        // 参数:
        //   _Reader:
        public PPtr(EndianBinaryReader _Reader)
        {
            FileId = _Reader.ReadInt32();
            PathId = _Reader.ReadInt64();
        }

        //
        // 摘要:
        //     Write the object pointer.
        //
        // 参数:
        //   _Writer:
        public void Write(EndianBinaryWriter _Writer)
        {
            _Writer.Write(FileId);
            _Writer.Write(PathId);
        }
    }
    public class Object : IObject
    {
        //
        // 摘要:
        //     All objects have a unique path id in its file.
        public long PathId { get; private set; }

        //
        // 摘要:
        //     The guid of the type this object belongs too.
        public Guid TypeGuid { get; private set; }

        //
        // 摘要:
        //     Flag over visibilty by camera.
        public uint ObjectHideFlags { get; private set; }

        //
        // 摘要:
        //     Create an object by parameter.
        //
        // 参数:
        //   _PathId:
        //
        //   _TypeGuid:
        public Object(long _PathId, Guid _TypeGuid)
        {
            PathId = _PathId;
            TypeGuid = _TypeGuid;
        }

        //
        // 摘要:
        //     Read the object data from _Reader.
        //
        // 参数:
        //   _Reader:
        public Object(ObjectReader _Reader)
        {
            PathId = _Reader.ObjectInfo.PathId;
            TypeGuid = _Reader.ObjectInfo.TypeGuid;
            if (_Reader.BuildTarget == BuildTarget.NoTarget)
            {
                ObjectHideFlags = _Reader.ReadUInt32();
            }
        }

        //
        // 摘要:
        //     Write an object to _Writer.
        //
        // 参数:
        //   _Writer:
        public virtual void Write(ObjectWriter _Writer)
        {
            if (_Writer.BuildTarget == BuildTarget.NoTarget)
            {
                _Writer.Write(ObjectHideFlags);
            }
        }

        //
        // 摘要:
        //     Hashcode.
        public override int GetHashCode()
        {
            return PathId.GetHashCode() ^ TypeGuid.GetHashCode() ^ ObjectHideFlags.GetHashCode();
        }

        //
        // 摘要:
        //     Equals.
        //
        // 参数:
        //   _Other:
        public override bool Equals(object _Other)
        {
            if (_Other == null)
            {
                return false;
            }

            if (!(_Other is Object))
            {
                return false;
            }

            if (PathId == ((Object)_Other).PathId)
            {
                return TypeGuid == ((Object)_Other).TypeGuid;
            }

            return false;
        }
    }
}
