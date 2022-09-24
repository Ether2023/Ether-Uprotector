using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;

namespace O_Z_IL2CPP_Security
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    class VersionAttribute : Attribute
    {
        public double Min { get; set; } = 0;
        public double Max { get; set; } = 99;
    }
    [AttributeUsage(AttributeTargets.Field)]
    class ArrayLengthAttribute : Attribute
    {
        public int Length { get; set; }
    }
    public enum IL2CPP_Version
    {
        V24_5,
        V29
    };
    public struct MetadataHeader_v24_5
    {
        public uint sanity;
        public int version;
        public uint stringLiteralOffset; // string data for managed code
        public int stringLiteralCount;
        public uint stringLiteralDataOffset;
        public int stringLiteralDataCount;
        public uint stringOffset; // string data for metadata
        public int stringCount;
        public uint eventsOffset; // Il2CppEventDefinition
        public int eventsCount;
        public uint propertiesOffset; // Il2CppPropertyDefinition
        public int propertiesCount;
        public uint methodsOffset; // Il2CppMethodDefinition
        public int methodsCount;
        public uint parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
        public int parameterDefaultValuesCount;
        public uint fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
        public int fieldDefaultValuesCount;
        public uint fieldAndParameterDefaultValueDataOffset; // uint8_t
        public int fieldAndParameterDefaultValueDataCount;
        public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
        public int fieldMarshaledSizesCount;
        public uint parametersOffset; // Il2CppParameterDefinition
        public int parametersCount;
        public uint fieldsOffset; // Il2CppFieldDefinition
        public int fieldsCount;
        public uint genericParametersOffset; // Il2CppGenericParameter
        public int genericParametersCount;
        public uint genericParameterConstraintsOffset; // TypeIndex
        public int genericParameterConstraintsCount;
        public uint genericContainersOffset; // Il2CppGenericContainer
        public int genericContainersCount;
        public uint nestedTypesOffset; // TypeDefinitionIndex
        public int nestedTypesCount;
        public uint interfacesOffset; // TypeIndex
        public int interfacesCount;
        public uint vtableMethodsOffset; // EncodedMethodIndex
        public int vtableMethodsCount;
        public uint interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsCount;
        public uint typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsCount;
        public uint rgctxEntriesOffset; // *
        public int rgctxEntriesCount; // *
        public uint imagesOffset; // Il2CppImageDefinition
        public int imagesCount;
        public uint assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesCount;
        public uint metadataUsageListsOffset; // Il2CppMetadataUsageList
        public int metadataUsageListsCount;
        public uint metadataUsagePairsOffset; // Il2CppMetadataUsagePair
        public int metadataUsagePairsCount;
        public uint fieldRefsOffset; // Il2CppFieldRef
        public int fieldRefsCount;
        public int referencedAssembliesOffset; // public UInt32
        public int referencedAssembliesCount;
        public uint attributesInfoOffset; // Il2CppCustomAttributeTypeRange
        public int attributesInfoCount;
        public uint attributeTypesOffset; // TypeIndex
        public int attributeTypesCount;
        public int unresolvedVirtualCallParameterTypesOffset; // TypeIndex
        public int unresolvedVirtualCallParameterTypesCount;
        public int unresolvedVirtualCallParameterRangesOffset; // Il2CppRange
        public int unresolvedVirtualCallParameterRangesCount;
        public int windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
        public int windowsRuntimeTypeNamesSize;
        public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex
        public int exportedTypeDefinitionsCount;
    }
    public struct MetadataHeader_v29
    {
        public int sanity;
        public int version;
        public int stringLiteralOffset; // string data for managed code
        public int stringLiteralSize;
        public int stringLiteralDataOffset;
        public int stringLiteralDataSize;
        public int stringOffset; // string data for metadata
        public int stringSize;
        public int eventsOffset; // Il2CppEventDefinition
        public int eventsSize;
        public int propertiesOffset; // Il2CppPropertyDefinition
        public int propertiesSize;
        public int methodsOffset; // Il2CppMethodDefinition
        public int methodsSize;
        public int parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
        public int parameterDefaultValuesSize;
        public int fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
        public int fieldDefaultValuesSize;
        public int fieldAndParameterDefaultValueDataOffset; // uint8_t
        public int fieldAndParameterDefaultValueDataSize;
        public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
        public int fieldMarshaledSizesSize;
        public int parametersOffset; // Il2CppParameterDefinition
        public int parametersSize;
        public int fieldsOffset; // Il2CppFieldDefinition
        public int fieldsSize;
        public int genericParametersOffset; // Il2CppGenericParameter
        public int genericParametersSize;
        public int genericParameterConstraintsOffset; // TypeIndex
        public int genericParameterConstraintsSize;
        public int genericContainersOffset; // Il2CppGenericContainer
        public int genericContainersSize;
        public int nestedTypesOffset; // TypeDefinitionIndex
        public int nestedTypesSize;
        public int interfacesOffset; // TypeIndex
        public int interfacesSize;
        public int vtableMethodsOffset; // EncodedMethodIndex
        public int vtableMethodsSize;
        public int interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsSize;
        public int typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsSize;
        public int imagesOffset; // Il2CppImageDefinition
        public int imagesSize;
        public int assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesSize;
        public int fieldRefsOffset; // Il2CppFieldRef
        public int fieldRefsSize;
        public int referencedAssembliesOffset; // int32_t
        public int referencedAssembliesSize;
        public int attributeDataOffset;
        public int attributeDataSize;
        public int attributeDataRangeOffset;
        public int attributeDataRangeSize;
        public int unresolvedVirtualCallParameterTypesOffset; // TypeIndex
        public int unresolvedVirtualCallParameterTypesSize;
        public int unresolvedVirtualCallParameterRangesOffset; // Il2CppMetadataRange
        public int unresolvedVirtualCallParameterRangesSize;
        public int windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
        public int windowsRuntimeTypeNamesSize;
        public int windowsRuntimeStringsOffset; // const char*
        public int windowsRuntimeStringsSize;
        public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex
        public int exportedTypeDefinitionsSize;
    }
    public class StringLiteral
    {
        public uint Length;
        public uint Offset;
    }
    public class ImageDefinition
    {
        public uint nameIndex;
        public int assemblyIndex;

        public int typeStart;
        public uint typeCount;
        public int exportedTypeStart;
        public uint exportedTypeCount;

        public int entryPointIndex;
        public uint token;
        public int customAttributeStart;
        public uint customAttributeCount;
    }
    public class Il2CppAssemblyDefinition
    {
        public int imageIndex;
        public uint token;
        public int referencedAssemblyStart;
        public int referencedAssemblyCount;
        public Il2CppAssemblyNameDefinition aname;
    }
    public class Il2CppAssemblyNameDefinition
    {
        public int nameIndex;
        public int cultureIndex;
        public int publicKeyIndex;
        public uint hash_alg;
        public int hash_len;
        public uint flags;
        public int major;
        public int minor;
        public int build;
        public int revision;
        [ArrayLength(Length = 8)]
        public byte[] public_key_token;
    }
    public class Il2CppWindowsRuntimeTypeNamePair
    {
        public int nameIndex;
        public int typeIndex;
    }
    public class Il2CppTypeDefinition
    {
        public uint nameIndex;
        public uint namespaceIndex;
        public int byvalTypeIndex;
        public int byrefTypeIndex;

        public int declaringTypeIndex;
        public int parentIndex;
        public int elementTypeIndex; // we can probably remove this one. Only used for enums

        public int genericContainerIndex;

        public uint flags;

        public int fieldStart;
        public int methodStart;
        public int eventStart;
        public int propertyStart;
        public int nestedTypesStart;
        public int interfacesStart;
        public int vtableStart;
        public int interfaceOffsetsStart;

        public ushort method_count;
        public ushort property_count;
        public ushort field_count;
        public ushort event_count;
        public ushort nested_type_count;
        public ushort vtable_count;
        public ushort interfaces_count;
        public ushort interface_offsets_count;

        // bitfield to portably encode boolean values as single bits
        // 01 - valuetype;
        // 02 - enumtype;
        // 03 - has_finalize;
        // 04 - has_cctor;
        // 05 - is_blittable;
        // 06 - is_import_or_windows_runtime;
        // 07-10 - One of nine possible PackingSize values (0, 1, 2, 4, 8, 16, 32, 64, or 128)
        // 11 - PackingSize is default
        // 12 - ClassSize is default
        // 13-16 - One of nine possible PackingSize values (0, 1, 2, 4, 8, 16, 32, 64, or 128) - the specified packing size (even for explicit layouts)
        public uint bitfield;
        public uint token;

        public bool IsValueType => (bitfield & 0x1) == 1;
        public bool IsEnum => ((bitfield >> 1) & 0x1) == 1;
    }
    public class AssemblyString
    {
        public byte[]? name;
        public byte[]? culture;
        public byte[]? public_key;
    }
    public class TypeString
    {
        public byte[]? name;
        public byte[]? namespaze;
    }
    public class Tmp
    {
        public Tmp(Type type,object Header)
        {
            FieldInfo[] fields = type.GetFields(); 
            foreach (FieldInfo field in fields)
            {
                Console.WriteLine("Tyoe:" + field.FieldType + " Name:" + field.Name + " Value:" + field.GetValue(Header));
            }
        }
    }
    public class LoadMetadata_v24_5
    {
        public Stream metadatastream;
        public MetadataHeader_v24_5 Header;
        public LoadMetadata_v24_5(Stream i_metadatastream)
        {
            metadatastream = i_metadatastream;
            BinaryReader Reader = new BinaryReader(metadatastream);
            Header.sanity = Reader.ReadUInt32();
            Header.version = Reader.ReadInt32();
            Header.stringLiteralOffset = Reader.ReadUInt32(); // string data for managed code
            Header.stringLiteralCount = Reader.ReadInt32();
            Header.stringLiteralDataOffset = Reader.ReadUInt32();
            Header.stringLiteralDataCount = Reader.ReadInt32();
            Header.stringOffset = Reader.ReadUInt32(); // string data for metadata
            Header.stringCount = Reader.ReadInt32();
            Header.eventsOffset = Reader.ReadUInt32(); // Il2CppEventDefinition
            Header.eventsCount = Reader.ReadInt32();
            Header.propertiesOffset = Reader.ReadUInt32(); // Il2CppPropertyDefinition
            Header.propertiesCount = Reader.ReadInt32();
            Header.methodsOffset = Reader.ReadUInt32(); // Il2CppMethodDefinition
            Header.methodsCount = Reader.ReadInt32();
            Header.parameterDefaultValuesOffset = Reader.ReadUInt32(); // Il2CppParameterDefaultValue
            Header.parameterDefaultValuesCount = Reader.ReadInt32();
            Header.fieldDefaultValuesOffset = Reader.ReadUInt32(); // Il2CppFieldDefaultValue
            Header.fieldDefaultValuesCount = Reader.ReadInt32();
            Header.fieldAndParameterDefaultValueDataOffset = Reader.ReadUInt32(); // uint8_t
            Header.fieldAndParameterDefaultValueDataCount = Reader.ReadInt32();
            Header.fieldMarshaledSizesOffset = Reader.ReadInt32(); // Il2CppFieldMarshaledSize
            Header.fieldMarshaledSizesCount = Reader.ReadInt32();
            Header.parametersOffset = Reader.ReadUInt32(); // Il2CppParameterDefinition
            Header.parametersCount = Reader.ReadInt32();
            Header.fieldsOffset = Reader.ReadUInt32(); // Il2CppFieldDefinition
            Header.fieldsCount = Reader.ReadInt32();
            Header.genericParametersOffset = Reader.ReadUInt32(); // Il2CppGenericParameter
            Header.genericParametersCount = Reader.ReadInt32();
            Header.genericParameterConstraintsOffset = Reader.ReadUInt32(); // TypeIndex
            Header.genericParameterConstraintsCount = Reader.ReadInt32();
            Header.genericContainersOffset = Reader.ReadUInt32(); // Il2CppGenericContainer
            Header.genericContainersCount = Reader.ReadInt32();
            Header.nestedTypesOffset = Reader.ReadUInt32(); // TypeDefinitionIndex
            Header.nestedTypesCount = Reader.ReadInt32();
            Header.interfacesOffset = Reader.ReadUInt32(); // TypeIndex
            Header.interfacesCount = Reader.ReadInt32();
            Header.vtableMethodsOffset = Reader.ReadUInt32(); // EncodedMethodIndex
            Header.vtableMethodsCount = Reader.ReadInt32();
            Header.interfaceOffsetsOffset = Reader.ReadUInt32(); // Il2CppInterfaceOffsetPair
            Header.interfaceOffsetsCount = Reader.ReadInt32();
            Header.typeDefinitionsOffset = Reader.ReadUInt32(); // Il2CppTypeDefinition
            Header.typeDefinitionsCount = Reader.ReadInt32();
            /*
            Header.rgctxEntriesOffset = Reader.ReadUInt32();
            Header.rgctxEntriesCount = Reader.ReadInt32();
            */
            Header.imagesOffset = Reader.ReadUInt32(); // Il2CppImageDefinition
            Header.imagesCount = Reader.ReadInt32();
            Header.assembliesOffset = Reader.ReadUInt32(); // Il2CppAssemblyDefinition
            Header.assembliesCount = Reader.ReadInt32();
            Header.metadataUsageListsOffset = Reader.ReadUInt32(); // Il2CppMetadataUsageList
            Header.metadataUsageListsCount = Reader.ReadInt32();
            Header.metadataUsagePairsOffset = Reader.ReadUInt32(); // Il2CppMetadataUsagePair
            Header.metadataUsagePairsCount = Reader.ReadInt32();
            Header.fieldRefsOffset = Reader.ReadUInt32(); // Il2CppFieldRef
            Header.fieldRefsCount = Reader.ReadInt32();
            Header.referencedAssembliesOffset = Reader.ReadInt32(); // int32
            Header.referencedAssembliesCount = Reader.ReadInt32();
            Header.attributesInfoOffset = Reader.ReadUInt32(); // Il2CppCustomAttributeTypeRange
            Header.attributesInfoCount = Reader.ReadInt32();
            Header.attributeTypesOffset = Reader.ReadUInt32(); // TypeIndex
            Header.attributeTypesCount = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterTypesOffset = Reader.ReadInt32(); // TypeIndex
            Header.unresolvedVirtualCallParameterTypesCount = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterRangesOffset = Reader.ReadInt32(); // Il2CppRange
            Header.unresolvedVirtualCallParameterRangesCount = Reader.ReadInt32();
            Header.windowsRuntimeTypeNamesOffset = Reader.ReadInt32(); // Il2CppWindowsRuntimeTypeNamePair
            Header.windowsRuntimeTypeNamesSize = Reader.ReadInt32();
            Header.exportedTypeDefinitionsOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.exportedTypeDefinitionsCount = Reader.ReadInt32();

            metadatastream.Position = 0;
        }
    }
    public class Metadata : BinaryStream
    {
        public object header;

        public IL2CPP_Version InVersion;
        
        public int version;
        public uint stringOffset;
        public int stringCount;
        public uint stringLiteralOffset;
        public int stringLiteralCount;
        public uint stringLiteralDataOffset;
        public int stringLiteralDataCount;
        public uint imagesOffset;
        public int imagesCount;
        public uint assembliesOffset;
        public int assembliesCount;
        public int windowsRuntimeTypeNamesOffset;
        public int windowsRuntimeTypeNamesSize;
        public uint typeDefinitionsOffset;
        public int typeDefinitionsCount;

        public StringLiteral[] stringLiterals;
        public ImageDefinition[] imageDefinitions;
        public Il2CppAssemblyDefinition[] assemblyDefs;
        public Il2CppWindowsRuntimeTypeNamePair[] il2CppWindowsRuntimeTypeNamePairs;
        public Il2CppTypeDefinition[] il2CppTypeDefinitions;
        Stream metadatastream;
        public Metadata(Stream i_metadatastream,Type HeaderType,object Header, IL2CPP_Version Version) : base(i_metadatastream)
        {
            InVersion = Version;
            metadatastream = i_metadatastream;

            header = Header;
            version = (int)HeaderType.GetField("version").GetValue(Header);
            stringOffset = (uint)HeaderType.GetField("stringOffset").GetValue(Header);
            stringCount = (int)HeaderType.GetField("stringCount").GetValue(Header);
            stringLiteralOffset = (uint)HeaderType.GetField("stringLiteralOffset").GetValue(Header);
            stringLiteralCount = (int)HeaderType.GetField("stringLiteralCount").GetValue(Header);
            imagesOffset = (uint)HeaderType.GetField("imagesOffset").GetValue(Header);
            imagesCount = (int)HeaderType.GetField("imagesCount").GetValue(Header);
            stringLiteralDataOffset = (uint)HeaderType.GetField("stringLiteralDataOffset").GetValue(Header);
            stringLiteralDataCount = (int)HeaderType.GetField("stringLiteralDataCount").GetValue(Header);
            assembliesOffset = (uint)HeaderType.GetField("assembliesOffset").GetValue(Header);
            assembliesCount = (int)HeaderType.GetField("assembliesCount").GetValue(Header);
            windowsRuntimeTypeNamesOffset = (int)HeaderType.GetField("windowsRuntimeTypeNamesOffset").GetValue(Header);
            windowsRuntimeTypeNamesSize = (int)HeaderType.GetField("windowsRuntimeTypeNamesSize").GetValue(Header);
            typeDefinitionsOffset = (uint)HeaderType.GetField("typeDefinitionsOffset").GetValue(Header);
            typeDefinitionsCount = (int)HeaderType.GetField("typeDefinitionsCount").GetValue(Header);

            stringLiterals = GetLiterals();
            imageDefinitions = GetImageDefinitions();
            assemblyDefs = GetIl2CppAssemblyDefinitions();
            il2CppWindowsRuntimeTypeNamePairs = GetCppWindowsRuntimeTypeNamePairs();
            il2CppTypeDefinitions = GetIl2CppTypeDefinitions();
        }
        StringLiteral[] GetLiterals()
        {
            List<StringLiteral> stringLiterals = new List<StringLiteral>();
            BinaryReader reader = new BinaryReader(metadatastream);

            reader.BaseStream.Position = stringLiteralOffset;
            for (int i = 0; i < stringLiteralCount / SizeOf(typeof(StringLiteral)); i++) // 8 = sizeof(Il2CppStringLiteral)
            {
                stringLiterals.Add(new StringLiteral
                {
                    Length = reader.ReadUInt32(),
                    Offset = reader.ReadUInt32()
                });
            }
            return stringLiterals.ToArray();
        }
        ImageDefinition[] GetImageDefinitions()
        {
            List<ImageDefinition> ImageDefinitions = new List<ImageDefinition>();
            BinaryReader reader = new BinaryReader(metadatastream);
            reader.BaseStream.Position = imagesOffset;
            for (int i = 0; i < imagesCount / SizeOf(typeof(ImageDefinition)); i++) // 40 = sizeof(ImageDefinition)
            {
                ImageDefinitions.Add(new ImageDefinition
                {
                    nameIndex = reader.ReadUInt32(),
                    assemblyIndex = reader.ReadInt32(),
                    typeStart = reader.ReadInt32(),
                    typeCount = reader.ReadUInt32(),
                    exportedTypeStart = reader.ReadInt32(),
                    exportedTypeCount = reader.ReadUInt32(),
                    entryPointIndex = reader.ReadInt32(),
                    token = reader.ReadUInt32(),
                    customAttributeStart = reader.ReadInt32(),
                    customAttributeCount = reader.ReadUInt32(),
                });
            }
            return ImageDefinitions.ToArray();
        }
        Il2CppAssemblyDefinition[] GetIl2CppAssemblyDefinitions()
        {
            return ReadMetadataClassArray<Il2CppAssemblyDefinition>(assembliesOffset, assembliesCount);
        }
        Il2CppWindowsRuntimeTypeNamePair[] GetCppWindowsRuntimeTypeNamePairs()
        {
            return ReadMetadataClassArray<Il2CppWindowsRuntimeTypeNamePair>((uint)windowsRuntimeTypeNamesOffset, windowsRuntimeTypeNamesSize);
        }
        Il2CppTypeDefinition[] GetIl2CppTypeDefinitions()
        {
            return ReadMetadataClassArray<Il2CppTypeDefinition>(typeDefinitionsOffset, typeDefinitionsCount);
        }

        public List<byte[]> GetBytesFromStringLiteral(StringLiteral[] stringLiterals)
        {
            List<byte[]> strBytes = new List<byte[]>();
            BinaryReader reader = new BinaryReader(metadatastream);
            for (int i = 0; i < stringLiterals.Length; i++)
            {
                reader.BaseStream.Position = stringLiteralDataOffset + stringLiterals[i].Offset;
                strBytes.Add(reader.ReadBytes((int)stringLiterals[i].Length));
            }
            return strBytes;
        }
        public Stream SetCryptedStreamToMetadata(List<byte[]> CryptedStringLiteralBytes, byte[] allString)
        {
            Stream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            object o_Header;
            if (InVersion == IL2CPP_Version.V24_5)
                o_Header = new CryptedHeader_2019_4_32_f1((MetadataHeader_v24_5)header);
            else
                o_Header = new CryptedHeader_2019_4_32_f1((MetadataHeader_v24_5)header);
            metadatastream.Position = 0;
            metadatastream.CopyTo(writer.BaseStream);
            writer.BaseStream.Position = 0;
            CryptHeader cryptHeader = new CryptHeader(header,IL2CPP_Version.V24_5, metadatastream.Length);
            writer.Write(cryptHeader.frontHeader.sign); // 24
            //Console.WriteLine("sign: " + Encoding.UTF8.GetString(cryptHeader.frontHeader.sign));
            writer.Write(cryptHeader.frontHeader.offset); // 8
            //Console.WriteLine("offset: " + cryptHeader.frontHeader.offset);
            writer.Write(cryptHeader.frontHeader.length); // 4
            //Console.WriteLine("length: " + cryptHeader.frontHeader.length);
            writer.Write(cryptHeader.frontHeader.key); // 32             all:68
            //Console.WriteLine("key: " + BitConverter.ToString(cryptHeader.frontHeader.key));
            writer.Write(RandomNumberGenerator.GetBytes(cryptHeader.frontHeader.OriginLegnth - 68)); //使用随机数填充Header直至原始大小
            //byte[]? bytes = o_Header.GetType().GetMethod("cryptedHeader").Invoke(o_Header, null) as byte[];
            //writer.Write(bytes);//混淆Header
            for (int i = 0; i < stringLiterals.Length; i++) //加密StringLiteral
            {
                writer.BaseStream.Position = stringLiteralDataOffset + stringLiterals[i].Offset;
                writer.Write(CryptedStringLiteralBytes[i]);
            }
            writer.BaseStream.Position = stringOffset;
            writer.Write(allString); //加密String
            writer.BaseStream.Position = writer.BaseStream.Length;
            writer.Write(cryptHeader.Crypted_Header); //将混淆后的Header写入文件末尾
            stream.Position = 0;
            return stream;
        }
        public List<byte[]> GetImageStringsFromImageDefinitions(ImageDefinition[] imageDefinitions)
        {
            List<byte[]> strings = new List<byte[]>();
            BinaryReader reader = new BinaryReader(metadatastream);
            for (int i = 0; i < imageDefinitions.Length; i++)
            {
                strings.Add(GetStringFromIndex(imageDefinitions[i].nameIndex));
            }
            return strings;
        }
        public List<AssemblyString> GetAssemblyStringsFromIl2CppAssemblyDefinition(Il2CppAssemblyDefinition[] il2CppAssemblyDefinitions)
        {
            List<byte[]> strings = new List<byte[]>();
            List<AssemblyString> assemblyStrings = new List<AssemblyString>();
            BinaryReader reader = new BinaryReader(metadatastream);
            for (int i = 0; i < il2CppAssemblyDefinitions.Length; i++)
            {
                assemblyStrings.Add(new AssemblyString
                {
                    name = GetStringFromIndex((ulong)il2CppAssemblyDefinitions[i].aname.nameIndex),
                    culture = GetStringFromIndex((ulong)il2CppAssemblyDefinitions[i].aname.cultureIndex),
                    public_key = GetStringFromIndex((ulong)il2CppAssemblyDefinitions[i].aname.publicKeyIndex)
                });
            }
            return assemblyStrings;
        }
        public List<TypeString> GetTypeStringFromIl2CppTypeDefinition(Il2CppTypeDefinition[] il2CppTypeDefinitions)
        {
            List<TypeString> strings = new List<TypeString>();
            BinaryReader reader = new BinaryReader(metadatastream);
            for (int i = 0; i < il2CppTypeDefinitions.Length; i++)
            {
                strings.Add(new TypeString
                {
                    name = GetStringFromIndex((ulong)il2CppTypeDefinitions[i].nameIndex),
                    namespaze = GetStringFromIndex((ulong)il2CppTypeDefinitions[i].namespaceIndex)
                });
            }
            return strings;
        }
        public byte[] GetStringFromIndex(ulong index)
        {
            return (ReadStringToNull(stringOffset + index));
        }
        byte[] ReadStringToNull(ulong addr)
        {
            BinaryReader reader = new BinaryReader(metadatastream);
            reader.BaseStream.Position = (long)addr;
            var bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0)
                bytes.Add(b);
            return bytes.ToArray();
        }
        public int SizeOf(Type type)
        {
            var size = 0;
            foreach (var i in type.GetFields())
            {
                var attr = (VersionAttribute)Attribute.GetCustomAttribute(i, typeof(VersionAttribute));
                if (attr != null)
                {
                    if (version < attr.Min || version > attr.Max)
                        continue;
                }
                var fieldType = i.FieldType;
                if (fieldType.IsPrimitive)
                {
                    size += GetPrimitiveTypeSize(fieldType.Name);
                }
                else if (fieldType.IsEnum)
                {
                    var e = fieldType.GetField("value__").FieldType;
                    size += GetPrimitiveTypeSize(e.Name);
                }
                else if (fieldType.IsArray)
                {
                    var arrayLengthAttribute = i.GetCustomAttribute<ArrayLengthAttribute>();
                    size += arrayLengthAttribute.Length;
                }
                else
                {
                    size += SizeOf(fieldType);
                }
            }
            return size;

            int GetPrimitiveTypeSize(string name)
            {
                switch (name)
                {
                    case "Int32":
                    case "UInt32":
                        return 4;
                    case "Int16":
                    case "UInt16":
                        return 2;
                    default:
                        return 0;
                }
            }
        }
        private T[] ReadMetadataClassArray<T>(uint addr, int count) where T : new()
        {
            return ReadClassArray<T>(addr, count / SizeOf(typeof(T)));
        }
        public byte[] GetAllStringFromMeta()
        {
            BinaryReader reader = new BinaryReader(metadatastream);
            reader.BaseStream.Position = stringOffset;
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < stringCount; i++)
            {
                bytes.Add(reader.ReadByte());
            }
            return bytes.ToArray();
        }
    }
    public class BinaryStream : IDisposable
    {
        public double Version;
        public bool Is32Bit;
        public ulong ImageBase;
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private MethodInfo readClass;
        private MethodInfo readClassArray;
        private Dictionary<Type, MethodInfo> genericMethodCache = new Dictionary<Type, MethodInfo>();
        private Dictionary<FieldInfo, VersionAttribute[]> attributeCache = new Dictionary<FieldInfo, VersionAttribute[]>();

        public BinaryStream(Stream input)
        {
            stream = input;
            reader = new BinaryReader(stream, Encoding.UTF8, true);
            writer = new BinaryWriter(stream, Encoding.UTF8, true);
            readClass = GetType().GetMethod("ReadClass", Type.EmptyTypes);
            readClassArray = GetType().GetMethod("ReadClassArray", new[] { typeof(long) });
        }

        public bool ReadBoolean() => reader.ReadBoolean();

        public byte ReadByte() => reader.ReadByte();

        public byte[] ReadBytes(int count) => reader.ReadBytes(count);

        public sbyte ReadSByte() => reader.ReadSByte();

        public short ReadInt16() => reader.ReadInt16();

        public ushort ReadUInt16() => reader.ReadUInt16();

        public int ReadInt32() => reader.ReadInt32();

        public uint ReadUInt32() => reader.ReadUInt32();

        public long ReadInt64() => reader.ReadInt64();

        public ulong ReadUInt64() => reader.ReadUInt64();

        public float ReadSingle() => reader.ReadSingle();

        public double ReadDouble() => reader.ReadDouble();

        public uint ReadCompressedUInt32() => reader.ReadCompressedUInt32();

        public int ReadCompressedInt32() => reader.ReadCompressedInt32();

        public uint ReadULeb128() => reader.ReadULeb128();

        public void Write(bool value) => writer.Write(value);

        public void Write(byte value) => writer.Write(value);

        public void Write(sbyte value) => writer.Write(value);

        public void Write(short value) => writer.Write(value);

        public void Write(ushort value) => writer.Write(value);

        public void Write(int value) => writer.Write(value);

        public void Write(uint value) => writer.Write(value);

        public void Write(long value) => writer.Write(value);

        public void Write(ulong value) => writer.Write(value);

        public void Write(float value) => writer.Write(value);

        public void Write(double value) => writer.Write(value);

        public ulong Position
        {
            get => (ulong)stream.Position;
            set => stream.Position = (long)value;
        }

        public ulong Length => (ulong)stream.Length;

        private object ReadPrimitive(Type type)
        {
            var typename = type.Name;
            switch (typename)
            {
                case "Int32":
                    return ReadInt32();
                case "UInt32":
                    return ReadUInt32();
                case "Int16":
                    return ReadInt16();
                case "UInt16":
                    return ReadUInt16();
                case "Byte":
                    return ReadByte();
                case "Int64" when Is32Bit:
                    return (long)ReadInt32();
                case "Int64":
                    return ReadInt64();
                case "UInt64" when Is32Bit:
                    return (ulong)ReadUInt32();
                case "UInt64":
                    return ReadUInt64();
                default:
                    throw new NotSupportedException();
            }
        }

        public T ReadClass<T>(ulong addr) where T : new()
        {
            Position = addr;
            return ReadClass<T>();
        }

        public T ReadClass<T>() where T : new()
        {
            var type = typeof(T);
            if (type.IsPrimitive)
            {
                return (T)ReadPrimitive(type);
            }
            else
            {
                var t = new T();
                foreach (var i in t.GetType().GetFields())
                {
                    if (!attributeCache.TryGetValue(i, out var versionAttributes))
                    {
                        if (Attribute.IsDefined(i, typeof(VersionAttribute)))
                        {
                            versionAttributes = i.GetCustomAttributes<VersionAttribute>().ToArray();
                            attributeCache.Add(i, versionAttributes);
                        }
                    }
                    if (versionAttributes?.Length > 0)
                    {
                        var read = false;
                        foreach (var versionAttribute in versionAttributes)
                        {
                            if (Version >= versionAttribute.Min && Version <= versionAttribute.Max)
                            {
                                read = true;
                                break;
                            }
                        }
                        if (!read)
                        {
                            continue;
                        }
                    }
                    var fieldType = i.FieldType;
                    if (fieldType.IsPrimitive)
                    {
                        i.SetValue(t, ReadPrimitive(fieldType));
                    }
                    else if (fieldType.IsEnum)
                    {
                        var e = fieldType.GetField("value__").FieldType;
                        i.SetValue(t, ReadPrimitive(e));
                    }
                    else if (fieldType.IsArray)
                    {
                        var arrayLengthAttribute = i.GetCustomAttribute<ArrayLengthAttribute>();
                        if (!genericMethodCache.TryGetValue(fieldType, out var methodInfo))
                        {
                            methodInfo = readClassArray.MakeGenericMethod(fieldType.GetElementType());
                            genericMethodCache.Add(fieldType, methodInfo);
                        }
                        i.SetValue(t, methodInfo.Invoke(this, new object[] { arrayLengthAttribute.Length }));
                    }
                    else
                    {
                        if (!genericMethodCache.TryGetValue(fieldType, out var methodInfo))
                        {
                            methodInfo = readClass.MakeGenericMethod(fieldType);
                            genericMethodCache.Add(fieldType, methodInfo);
                        }
                        i.SetValue(t, methodInfo.Invoke(this, null));
                    }
                }
                return t;
            }
        }

        public T[] ReadClassArray<T>(long count) where T : new()
        {
            var t = new T[count];
            for (var i = 0; i < count; i++)
            {
                t[i] = ReadClass<T>();
            }
            return t;
        }

        public T[] ReadClassArray<T>(ulong addr, long count) where T : new()
        {
            Position = addr;
            return ReadClassArray<T>(count);
        }

        public string ReadStringToNull(ulong addr)
        {
            Position = addr;
            var bytes = new List<byte>();
            byte b;
            while ((b = ReadByte()) != 0)
                bytes.Add(b);
            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        public long ReadIntPtr()
        {
            return Is32Bit ? ReadInt32() : ReadInt64();
        }

        public ulong ReadUIntPtr()
        {
            return Is32Bit ? ReadUInt32() : ReadUInt64();
        }

        public ulong PointerSize
        {
            get => Is32Bit ? 4ul : 8ul;
        }

        public BinaryReader Reader => reader;

        public BinaryWriter Writer => writer;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                reader.Close();
                writer.Close();
                stream.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
    public static class BinaryReaderExtensions
    {
        public static string ReadString(this BinaryReader reader, int numChars)
        {
            var start = reader.BaseStream.Position;
            // UTF8 takes up to 4 bytes per character
            var str = Encoding.UTF8.GetString(reader.ReadBytes(numChars * 4)).Substring(0, numChars);
            // make our position what it would have been if we'd known the exact number of bytes needed.
            reader.BaseStream.Position = start;
            reader.ReadBytes(Encoding.UTF8.GetByteCount(str));
            return str;
        }

        public static uint ReadULeb128(this BinaryReader reader)
        {
            uint value = reader.ReadByte();
            if (value >= 0x80)
            {
                var bitshift = 0;
                value &= 0x7f;
                while (true)
                {
                    var b = reader.ReadByte();
                    bitshift += 7;
                    value |= (uint)((b & 0x7f) << bitshift);
                    if (b < 0x80)
                        break;
                }
            }
            return value;
        }

        public static uint ReadCompressedUInt32(this BinaryReader reader)
        {
            uint val;
            var read = reader.ReadByte();

            if ((read & 0x80) == 0)
            {
                // 1 byte written
                val = read;
            }
            else if ((read & 0xC0) == 0x80)
            {
                // 2 bytes written
                val = (read & ~0x80u) << 8;
                val |= reader.ReadByte();
            }
            else if ((read & 0xE0) == 0xC0)
            {
                // 4 bytes written
                val = (read & ~0xC0u) << 24;
                val |= ((uint)reader.ReadByte() << 16);
                val |= ((uint)reader.ReadByte() << 8);
                val |= reader.ReadByte();
            }
            else if (read == 0xF0)
            {
                // 5 bytes written, we had a really large int32!
                val = reader.ReadUInt32();
            }
            else if (read == 0xFE)
            {
                // Special encoding for Int32.MaxValue
                val = uint.MaxValue - 1;
            }
            else if (read == 0xFF)
            {
                // Yes we treat UInt32.MaxValue (and Int32.MinValue, see ReadCompressedInt32) specially
                val = uint.MaxValue;
            }
            else
            {
                throw new Exception("Invalid compressed integer format");
            }

            return val;
        }

        public static int ReadCompressedInt32(this BinaryReader reader)
        {
            var encoded = reader.ReadCompressedUInt32();

            // -UINT32_MAX can't be represted safely in an int32_t, so we treat it specially
            if (encoded == uint.MaxValue)
                return int.MinValue;

            bool isNegative = (encoded & 1) != 0;
            encoded >>= 1;
            if (isNegative)
                return -(int)(encoded + 1);
            return (int)encoded;
        }
    }
}