using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Xxtea;

namespace O_Z_IL2CPP_Security
{
    public struct FrontHeader //68
    {
        public byte[] sign; //24
        public long offset; //8
        public int length; //4
        public byte[] key; //32
        public int OriginLegnth; //原始Header大小
    }
    public class CryptHeader
    {
        public FrontHeader frontHeader; 
        object cryptHeader; //加密Header类
        byte[] o_Header; //混淆后Header
        public byte[] Crypted_Header; //加密后Header
        public CryptHeader(object Header, IL2CPP_Version ver, long offset)
        {
            if (ver == IL2CPP_Version.V24_4)
                cryptHeader = new CryptedHeader_2019_4_32_f1((MetadataHeader_v24_5)Header);
            else if (ver == IL2CPP_Version.V28)
                cryptHeader = new CryptedHeader_2021_3_6_f1((MetadataHeader_v28)Header);
            else
                throw new Exception("Error!");
            o_Header = cryptHeader.GetType().GetMethod("cryptedHeader").Invoke(cryptHeader, null) as byte[];
            frontHeader.key = GetKey(o_Header);
            Crypted_Header = XXTEA.Encrypt(o_Header, frontHeader.key);

            frontHeader.sign = new byte[] { 0x4F, 0x26, 0x5A, 0x5F, 0x49, 0x4C, 0x32, 0x43, 0x50, 0x50, 0x5F, 0x53, 0x65, 0x63, 0x75, 0x72, 0x69, 0x74, 0x79, 0x00, 0x00, 0x00, 0x00, 0x00 };
            frontHeader.length = Crypted_Header.Length;
            frontHeader.offset = offset;
            frontHeader.OriginLegnth = (int)cryptHeader.GetType().GetField("Length").GetValue(cryptHeader);
        }
        byte[] GetKey(byte[] bytes)
        {
            SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(bytes);
        }
    }
    public class CryptedHeader_2019_4_32_f1
    {
        public readonly int Length = 264;
        public struct o_Header
        {
            public uint sanity;

            public int stringLiteralCount;
            public int stringLiteralDataCount;
            public int stringCount;
            public int eventsCount;
            public int propertiesCount;
            public int methodsCount;
            public int parameterDefaultValuesCount;
            public int fieldDefaultValuesCount;
            public int fieldAndParameterDefaultValueDataCount;
            public int fieldMarshaledSizesCount;
            public int parametersCount;
            public int fieldsCount;
            public int genericParametersCount;
            public int genericParameterConstraintsCount;
            public int genericContainersCount;
            public int nestedTypesCount;
            public int interfacesCount;
            public int vtableMethodsCount;
            public int interfaceOffsetsCount;
            public int typeDefinitionsCount;
            public int imagesCount;
            public int assembliesCount;
            public int metadataUsageListsCount;
            public int metadataUsagePairsCount;
            public int fieldRefsCount;
            public int referencedAssembliesCount;
            public int attributesInfoCount;
            public int attributeTypesCount;
            public int unresolvedVirtualCallParameterTypesCount;
            public int unresolvedVirtualCallParameterRangesCount;
            public int windowsRuntimeTypeNamesSize;
            public int exportedTypeDefinitionsCount;

            public int version;

            public uint stringLiteralOffset; // string data for managed code
            public uint stringLiteralDataOffset;
            public uint stringOffset; // string data for metadata
            public uint eventsOffset; // Il2CppEventDefinition
            public uint propertiesOffset; // Il2CppPropertyDefinition
            public uint methodsOffset; // Il2CppMethodDefinition
            public uint parameterDefaultValuesOffset; // Il2CppParameterDefaultValue

            public uint fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
            public uint fieldAndParameterDefaultValueDataOffset; // uint8_t
            public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
            public uint parametersOffset; // Il2CppParameterDefinition
            public uint fieldsOffset; // Il2CppFieldDefinition
            public uint genericParametersOffset; // Il2CppGenericParameter
            public uint genericParameterConstraintsOffset; // TypeIndex
            public uint genericContainersOffset; // Il2CppGenericContainer
            public uint nestedTypesOffset; // TypeDefinitionIndex
            public uint interfacesOffset; // TypeIndex
            public uint vtableMethodsOffset; // EncodedMethodIndex
            public uint interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
            public uint typeDefinitionsOffset; // Il2CppTypeDefinition
            public uint imagesOffset; // Il2CppImageDefinition
            public uint assembliesOffset; // Il2CppAssemblyDefinition
            public uint metadataUsageListsOffset; // Il2CppMetadataUsageList
            public uint metadataUsagePairsOffset; // Il2CppMetadataUsagePair
            public uint fieldRefsOffset; // Il2CppFieldRef
            public int referencedAssembliesOffset; // public UInt32
            public uint attributesInfoOffset; // Il2CppCustomAttributeTypeRange
            public uint attributeTypesOffset; // TypeIndex
            public int unresolvedVirtualCallParameterTypesOffset; // TypeIndex
            public int unresolvedVirtualCallParameterRangesOffset; // Il2CppRange
            public uint windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
            public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex

        }
        public o_Header o_header;
        public CryptedHeader_2019_4_32_f1(MetadataHeader_v24_5 metadataHeader)
        {
            o_header.sanity = 0x5A264F;
            o_header.version = 0x00;
            o_header.stringLiteralOffset = metadataHeader.stringLiteralOffset;
            o_header.stringLiteralCount = metadataHeader.stringLiteralCount;
            o_header.stringLiteralDataOffset = metadataHeader.stringLiteralDataOffset;
            o_header.stringLiteralDataCount = metadataHeader.stringLiteralDataCount;
            o_header.stringOffset = metadataHeader.stringOffset;
            o_header.stringCount = metadataHeader.stringCount;
            o_header.eventsOffset = metadataHeader.eventsOffset;
            o_header.eventsCount = metadataHeader.eventsCount;
            o_header.propertiesOffset = metadataHeader.propertiesOffset;
            o_header.propertiesCount = metadataHeader.propertiesCount;
            o_header.methodsOffset = metadataHeader.methodsOffset;
            o_header.methodsCount = metadataHeader.methodsCount;
            o_header.parameterDefaultValuesOffset = metadataHeader.parameterDefaultValuesOffset;
            o_header.parameterDefaultValuesCount = metadataHeader.parameterDefaultValuesCount;
            o_header.fieldDefaultValuesOffset = metadataHeader.fieldDefaultValuesOffset;
            o_header.fieldDefaultValuesCount = metadataHeader.fieldDefaultValuesCount;
            o_header.fieldAndParameterDefaultValueDataOffset = metadataHeader.fieldAndParameterDefaultValueDataOffset;
            o_header.fieldAndParameterDefaultValueDataCount = metadataHeader.fieldAndParameterDefaultValueDataCount;
            o_header.fieldMarshaledSizesOffset = metadataHeader.fieldMarshaledSizesOffset;
            o_header.fieldMarshaledSizesCount = metadataHeader.fieldMarshaledSizesCount;
            o_header.parametersOffset = metadataHeader.parametersOffset;
            o_header.parametersCount = metadataHeader.parametersCount;
            o_header.fieldsOffset = metadataHeader.fieldsOffset;
            o_header.fieldsCount = metadataHeader.fieldsCount;
            o_header.genericParametersOffset = metadataHeader.genericParametersOffset;
            o_header.genericParametersCount = metadataHeader.genericParametersCount;
            o_header.genericParameterConstraintsOffset = metadataHeader.genericParameterConstraintsOffset;
            o_header.genericParameterConstraintsCount = metadataHeader.genericParameterConstraintsCount;
            o_header.genericContainersOffset = metadataHeader.genericContainersOffset;
            o_header.genericContainersCount = metadataHeader.genericContainersCount;
            o_header.nestedTypesOffset = metadataHeader.nestedTypesOffset;
            o_header.nestedTypesCount = metadataHeader.nestedTypesCount;
            o_header.interfacesOffset = metadataHeader.interfacesOffset;
            o_header.interfacesCount = metadataHeader.interfacesCount;
            o_header.vtableMethodsOffset = metadataHeader.vtableMethodsOffset;
            o_header.vtableMethodsCount = metadataHeader.vtableMethodsCount;
            o_header.interfaceOffsetsOffset = metadataHeader.interfaceOffsetsOffset;
            o_header.interfaceOffsetsCount = metadataHeader.interfaceOffsetsCount;
            o_header.typeDefinitionsOffset = metadataHeader.typeDefinitionsOffset;
            o_header.typeDefinitionsCount = metadataHeader.typeDefinitionsCount;

            o_header.imagesOffset = metadataHeader.imagesOffset;
            o_header.imagesCount = metadataHeader.imagesCount;
            o_header.assembliesOffset = metadataHeader.assembliesOffset;
            o_header.assembliesCount = metadataHeader.assembliesCount;
            o_header.metadataUsageListsOffset = metadataHeader.metadataUsageListsOffset;
            o_header.metadataUsageListsCount = metadataHeader.metadataUsageListsCount;
            o_header.metadataUsagePairsOffset = metadataHeader.metadataUsagePairsOffset;
            o_header.metadataUsagePairsCount = metadataHeader.metadataUsagePairsCount;
            o_header.fieldRefsOffset = metadataHeader.fieldRefsOffset;
            o_header.fieldRefsCount = metadataHeader.fieldRefsCount;
            o_header.referencedAssembliesOffset = metadataHeader.referencedAssembliesOffset;
            o_header.referencedAssembliesCount = metadataHeader.referencedAssembliesCount;
            o_header.attributesInfoOffset = metadataHeader.attributesInfoOffset;
            o_header.attributesInfoCount = metadataHeader.attributesInfoCount;
            o_header.attributeTypesOffset = metadataHeader.attributeTypesOffset;
            o_header.attributeTypesCount = metadataHeader.attributeTypesCount;
            o_header.unresolvedVirtualCallParameterTypesOffset = metadataHeader.unresolvedVirtualCallParameterTypesOffset;
            o_header.unresolvedVirtualCallParameterTypesCount = metadataHeader.unresolvedVirtualCallParameterTypesCount;
            o_header.unresolvedVirtualCallParameterRangesOffset = metadataHeader.unresolvedVirtualCallParameterRangesOffset;
            o_header.unresolvedVirtualCallParameterRangesCount = metadataHeader.unresolvedVirtualCallParameterRangesCount;
            o_header.windowsRuntimeTypeNamesOffset = metadataHeader.windowsRuntimeTypeNamesOffset;
            o_header.windowsRuntimeTypeNamesSize = metadataHeader.windowsRuntimeTypeNamesSize;
            o_header.exportedTypeDefinitionsOffset = metadataHeader.exportedTypeDefinitionsOffset;
            o_header.exportedTypeDefinitionsCount = metadataHeader.exportedTypeDefinitionsCount;
        }
        public byte[] cryptedHeader()
        {
            Stream stream = new MemoryStream();
            stream.Position = 0;
            BinaryWriter writer = new BinaryWriter(stream);
            writer.BaseStream.Position = 0;
            writer.Write(o_header.sanity);
            writer.Write(o_header.stringLiteralCount);
            writer.Write(o_header.stringLiteralDataCount);
            writer.Write(o_header.stringCount);
            writer.Write(o_header.eventsCount);
            writer.Write(o_header.propertiesCount);
            writer.Write(o_header.methodsCount);
            writer.Write(o_header.parameterDefaultValuesCount);
            writer.Write(o_header.fieldDefaultValuesCount);
            writer.Write(o_header.fieldAndParameterDefaultValueDataCount);
            writer.Write(o_header.fieldMarshaledSizesCount);
            writer.Write(o_header.parametersCount);
            writer.Write(o_header.fieldsCount);
            writer.Write(o_header.genericParametersCount);
            writer.Write(o_header.genericParameterConstraintsCount);
            writer.Write(o_header.genericContainersCount);
            writer.Write(o_header.nestedTypesCount);
            writer.Write(o_header.interfacesCount);
            writer.Write(o_header.vtableMethodsCount);
            writer.Write(o_header.interfaceOffsetsCount);
            writer.Write(o_header.typeDefinitionsCount);
            writer.Write(o_header.imagesCount);
            writer.Write(o_header.assembliesCount);
            writer.Write(o_header.metadataUsageListsCount);
            writer.Write(o_header.metadataUsagePairsCount);
            writer.Write(o_header.fieldRefsCount);
            writer.Write(o_header.referencedAssembliesCount);
            writer.Write(o_header.attributesInfoCount);
            writer.Write(o_header.attributeTypesCount);
            writer.Write(o_header.unresolvedVirtualCallParameterTypesCount);
            writer.Write(o_header.unresolvedVirtualCallParameterRangesCount);
            writer.Write(o_header.windowsRuntimeTypeNamesSize);
            writer.Write(o_header.exportedTypeDefinitionsCount);

            writer.Write(o_header.version);

            writer.Write(o_header.stringLiteralOffset); // string data for managed code
            writer.Write(o_header.stringLiteralDataOffset);
            writer.Write(o_header.stringOffset); // string data for metadata
            writer.Write(o_header.eventsOffset); // Il2CppEventDefinition
            writer.Write(o_header.propertiesOffset); // Il2CppPropertyDefinition
            writer.Write(o_header.methodsOffset); // Il2CppMethodDefinition
            writer.Write(o_header.parameterDefaultValuesOffset); // Il2CppParameterDefaultValue
            writer.Write(o_header.fieldDefaultValuesOffset); // Il2CppFieldDefaultValue
            writer.Write(o_header.fieldAndParameterDefaultValueDataOffset); // uint8_t
            writer.Write(o_header.fieldMarshaledSizesOffset); // Il2CppFieldMarshaledSize
            writer.Write(o_header.parametersOffset); // Il2CppParameterDefinition
            writer.Write(o_header.fieldsOffset); // Il2CppFieldDefinition
            writer.Write(o_header.genericParametersOffset); // Il2CppGenericParameter
            writer.Write(o_header.genericParameterConstraintsOffset); // TypeIndex
            writer.Write(o_header.genericContainersOffset); // Il2CppGenericContainer
            writer.Write(o_header.nestedTypesOffset); // TypeDefinitionIndex
            writer.Write(o_header.interfacesOffset); // TypeIndex
            writer.Write(o_header.vtableMethodsOffset); // EncodedMethodIndex
            writer.Write(o_header.interfaceOffsetsOffset); // Il2CppInterfaceOffsetPair
            writer.Write(o_header.typeDefinitionsOffset); // Il2CppTypeDefinition
            writer.Write(o_header.imagesOffset); // Il2CppImageDefinition
            writer.Write(o_header.assembliesOffset); // Il2CppAssemblyDefinition
            writer.Write(o_header.metadataUsageListsOffset); // Il2CppMetadataUsageList
            writer.Write(o_header.metadataUsagePairsOffset); // Il2CppMetadataUsagePair
            writer.Write(o_header.fieldRefsOffset); // Il2CppFieldRef
            writer.Write(o_header.referencedAssembliesOffset); // public UInt32
            writer.Write(o_header.attributesInfoOffset); // Il2CppCustomAttributeTypeRange
            writer.Write(o_header.attributeTypesOffset); // TypeIndex
            writer.Write(o_header.unresolvedVirtualCallParameterTypesOffset); // TypeIndex
            writer.Write(o_header.unresolvedVirtualCallParameterRangesOffset); // Il2CppRange
            writer.Write(o_header.windowsRuntimeTypeNamesOffset); // Il2CppWindowsRuntimeTypeNamePair
            writer.Write(o_header.exportedTypeDefinitionsOffset); // TypeDefinitionIndex
            stream.Position = 0;
            return Tools.StreamToBytes(stream);
        }
    }
    public class CryptedHeader_2021_3_6_f1
    {
        public readonly int Length = 256;
        public struct o_Header
        {
            public uint sanity;

            public uint stringLiteralOffset; // string data for managed code
            public uint interfacesOffset; // TypeIndex
            public uint stringLiteralDataOffset;
            public uint stringOffset; // string data for metadata
            public uint vtableMethodsOffset; // EncodedMethodIndex
            public uint eventsOffset; // Il2CppEventDefinition
            public uint interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
            public uint propertiesOffset; // Il2CppPropertyDefinition
            public uint assembliesOffset; // Il2CppAssemblyDefinition
            public uint methodsOffset; // Il2CppMethodDefinition
            public uint fieldRefsOffset; // Il2CppFieldRef
            public uint parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
            public uint typeDefinitionsOffset; // Il2CppTypeDefinition
            public uint fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
            public uint imagesOffset; // Il2CppImageDefinition
            public uint fieldAndParameterDefaultValueDataOffset; // uint8_t
            public uint referencedAssembliesOffset; // int32_t
            public uint fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
            public uint unresolvedVirtualCallParameterTypesOffset; // TypeIndex
            public uint parametersOffset; // Il2CppParameterDefinition
            public uint unresolvedVirtualCallParameterRangesOffset; // Il2CppMetadataRange
            public uint fieldsOffset; // Il2CppFieldDefinition
            public uint windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
            public uint genericParametersOffset; // Il2CppGenericParameter
            public uint windowsRuntimeStringsOffset; // const char*
            public uint genericParameterConstraintsOffset; // TypeIndex
            public uint exportedTypeDefinitionsOffset; // TypeDefinitionIndex
            public uint genericContainersOffset; // Il2CppGenericContainer
            public uint nestedTypesOffset; // TypeDefinitionIndex

            public int version;
            public int genericContainersSize;
            public int stringLiteralSize;
            public int nestedTypesSize;
            public int interfacesSize;
            public int stringLiteralDataSize;
            public int vtableMethodsSize;
            public int stringSize;
            public int interfaceOffsetsSize;
            public int eventsSize;
            public int typeDefinitionsSize;
            public int parameterDefaultValuesSize;
            public int imagesSize;
            public int fieldDefaultValuesSize;
            public int assembliesSize;
            public int propertiesSize;
            public int fieldRefsSize;
            public int methodsSize;
            public int referencedAssembliesSize;
            public uint attributeDataOffset;
            public int attributeDataSize;
            public uint attributeDataRangeOffset;
            public int attributeDataRangeSize;
            public int fieldAndParameterDefaultValueDataSize;
            public int unresolvedVirtualCallParameterTypesSize;
            public int fieldMarshaledSizesSize;
            public int unresolvedVirtualCallParameterRangesSize;
            public int parametersSize;
            public int windowsRuntimeTypeNamesSize;
            public int fieldsSize;
            public int windowsRuntimeStringsSize;
            public int genericParametersSize;
            public int exportedTypeDefinitionsSize;
            public int genericParameterConstraintsSize;
        }
        public o_Header o_header;
        public CryptedHeader_2021_3_6_f1(MetadataHeader_v28 metadataHeader)
        {
            o_header.sanity = 0x5A264F;
            o_header.version = 0x00;
            o_header.stringLiteralOffset = metadataHeader.stringLiteralOffset; // string data for managed code
            o_header.stringLiteralSize = metadataHeader.stringLiteralSize;
            o_header.stringLiteralDataOffset = metadataHeader.stringLiteralDataOffset;
            o_header.stringLiteralDataSize = metadataHeader.stringLiteralDataSize;
            o_header.stringOffset = metadataHeader.stringOffset; // string data for metadata
            o_header.stringSize = metadataHeader.stringSize;
            o_header.eventsOffset = metadataHeader.eventsOffset; // Il2CppEventDefinition
            o_header.eventsSize = metadataHeader.eventsSize;
            o_header.propertiesOffset = metadataHeader.propertiesOffset; // Il2CppPropertyDefinition
            o_header.propertiesSize = metadataHeader.propertiesSize;
            o_header.methodsOffset = metadataHeader.methodsOffset; // Il2CppMethodDefinition
            o_header.methodsSize = metadataHeader.methodsSize;
            o_header.parameterDefaultValuesOffset = metadataHeader.parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
            o_header.parameterDefaultValuesSize = metadataHeader.parameterDefaultValuesSize;
            o_header.fieldDefaultValuesOffset = metadataHeader.fieldDefaultValuesOffset;// Il2CppFieldDefaultValue
            o_header.fieldDefaultValuesSize = metadataHeader.fieldDefaultValuesSize;
            o_header.fieldAndParameterDefaultValueDataOffset = metadataHeader.fieldAndParameterDefaultValueDataOffset; // uint8_t
            o_header.fieldAndParameterDefaultValueDataSize = metadataHeader.fieldAndParameterDefaultValueDataSize;
            o_header.fieldMarshaledSizesOffset = metadataHeader.fieldMarshaledSizesOffset;// Il2CppFieldMarshaledSize
            o_header.fieldMarshaledSizesSize = metadataHeader.fieldMarshaledSizesSize;
            o_header.parametersOffset = metadataHeader.parametersOffset; // Il2CppParameterDefinition
            o_header.parametersSize = metadataHeader.parametersSize;
            o_header.parametersSize = metadataHeader.parametersSize;
            o_header.fieldsOffset = metadataHeader.fieldsOffset; // Il2CppFieldDefinition
            o_header.fieldsSize = metadataHeader.fieldsSize;
            o_header.genericParametersOffset = metadataHeader.genericParametersOffset; // Il2CppGenericParameter
            o_header.genericParametersSize = metadataHeader.genericParametersSize;
            o_header.genericParameterConstraintsOffset = metadataHeader.genericParameterConstraintsOffset; // TypeIndex
            o_header.genericParameterConstraintsSize = metadataHeader.genericParameterConstraintsSize;
            o_header.genericContainersOffset = metadataHeader.genericContainersOffset; // Il2CppGenericContainer
            o_header.genericContainersSize = metadataHeader.genericContainersSize;
            o_header.nestedTypesOffset = metadataHeader.nestedTypesOffset; // TypeDefinitionIndex
            o_header.nestedTypesSize = metadataHeader.nestedTypesSize;
            o_header.interfacesOffset = metadataHeader.interfacesOffset;// TypeIndex
            o_header.interfacesSize = metadataHeader.interfacesSize;
            o_header.vtableMethodsOffset = metadataHeader.vtableMethodsOffset; // EncodedMethodIndex
            o_header.vtableMethodsSize = metadataHeader.vtableMethodsSize;
            o_header.interfaceOffsetsOffset = metadataHeader.interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
            o_header.interfaceOffsetsSize = metadataHeader.interfaceOffsetsSize;
            o_header.typeDefinitionsOffset = metadataHeader.typeDefinitionsOffset; // Il2CppTypeDefinition
            o_header.typeDefinitionsSize = metadataHeader.typeDefinitionsSize;
            o_header.imagesOffset = metadataHeader.imagesOffset; // Il2CppImageDefinition
            o_header.imagesSize = metadataHeader.imagesSize;
            o_header.assembliesOffset = metadataHeader.assembliesOffset; // Il2CppAssemblyDefinition
            o_header.assembliesSize = metadataHeader.assembliesSize;
            o_header.fieldRefsOffset = metadataHeader.fieldRefsOffset; // Il2CppFieldRef
            o_header.fieldRefsSize = metadataHeader.fieldRefsSize;
            o_header.referencedAssembliesOffset = metadataHeader.referencedAssembliesOffset; // int32_t
            o_header.referencedAssembliesSize = metadataHeader.referencedAssembliesSize;
            o_header.attributeDataOffset = metadataHeader.attributeDataOffset;
            o_header.attributeDataSize = metadataHeader.attributeDataSize;
            o_header.attributeDataRangeOffset = metadataHeader.attributeDataRangeOffset;
            o_header.attributeDataRangeSize = metadataHeader.attributeDataRangeSize;
            o_header.unresolvedVirtualCallParameterTypesOffset = metadataHeader.unresolvedVirtualCallParameterTypesOffset; // TypeIndex
            o_header.unresolvedVirtualCallParameterTypesSize = metadataHeader.unresolvedVirtualCallParameterTypesSize;
            o_header.unresolvedVirtualCallParameterRangesOffset = metadataHeader.unresolvedVirtualCallParameterRangesOffset; // Il2CppMetadataRange
            o_header.unresolvedVirtualCallParameterRangesSize = metadataHeader.unresolvedVirtualCallParameterRangesSize;
            o_header.windowsRuntimeTypeNamesOffset = metadataHeader.windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
            o_header.windowsRuntimeTypeNamesSize = metadataHeader.windowsRuntimeTypeNamesSize;
            o_header.windowsRuntimeStringsOffset = metadataHeader.windowsRuntimeStringsOffset; // const char*
            o_header.windowsRuntimeStringsSize = metadataHeader.windowsRuntimeStringsSize;
            o_header.exportedTypeDefinitionsOffset = metadataHeader.exportedTypeDefinitionsOffset; // TypeDefinitionIndex
            o_header.exportedTypeDefinitionsSize = metadataHeader.exportedTypeDefinitionsSize;
        }
        public byte[] cryptedHeader()
        {
            Stream stream = new MemoryStream();
            stream.Position = 0;
            BinaryWriter writer = new BinaryWriter(stream);
            writer.BaseStream.Position = 0;

            writer.Write(o_header.sanity);

            writer.Write(o_header.stringLiteralOffset); // string data for managed code
            writer.Write(o_header.interfacesOffset); // TypeIndex
            writer.Write(o_header.stringLiteralDataOffset);
            writer.Write(o_header.stringOffset); // string data for metadata
            writer.Write(o_header.vtableMethodsOffset); // EncodedMethodIndex
            writer.Write(o_header.eventsOffset); // Il2CppEventDefinition
            writer.Write(o_header.interfaceOffsetsOffset); // Il2CppInterfaceOffsetPair
            writer.Write(o_header.propertiesOffset); // Il2CppPropertyDefinition
            writer.Write(o_header.assembliesOffset); // Il2CppAssemblyDefinition
            writer.Write(o_header.methodsOffset); // Il2CppMethodDefinition
            writer.Write(o_header.fieldRefsOffset); // Il2CppFieldRef
            writer.Write(o_header.parameterDefaultValuesOffset); // Il2CppParameterDefaultValue
            writer.Write(o_header.typeDefinitionsOffset); // Il2CppTypeDefinition
            writer.Write(o_header.fieldDefaultValuesOffset); // Il2CppFieldDefaultValue
            writer.Write(o_header.imagesOffset); // Il2CppImageDefinition
            writer.Write(o_header.fieldAndParameterDefaultValueDataOffset); // uint8_t
            writer.Write(o_header.referencedAssembliesOffset); // int32_t
            writer.Write(o_header.fieldMarshaledSizesOffset); // Il2CppFieldMarshaledSize
            writer.Write(o_header.unresolvedVirtualCallParameterTypesOffset); // TypeIndex
            writer.Write(o_header.parametersOffset); // Il2CppParameterDefinition
            writer.Write(o_header.unresolvedVirtualCallParameterRangesOffset); // Il2CppMetadataRange
            writer.Write(o_header.fieldsOffset); // Il2CppFieldDefinition
            writer.Write(o_header.windowsRuntimeTypeNamesOffset); // Il2CppWindowsRuntimeTypeNamePair
            writer.Write(o_header.genericParametersOffset); // Il2CppGenericParameter
            writer.Write(o_header.windowsRuntimeStringsOffset); // const char*
            writer.Write(o_header.genericParameterConstraintsOffset); // TypeIndex
            writer.Write(o_header.exportedTypeDefinitionsOffset); // TypeDefinitionIndex
            writer.Write(o_header.genericContainersOffset); // Il2CppGenericContainer
            writer.Write(o_header.nestedTypesOffset); // TypeDefinitionIndex

            writer.Write(o_header.version);
            writer.Write(o_header.genericContainersSize);
            writer.Write(o_header.stringLiteralSize);
            writer.Write(o_header.nestedTypesSize);
            writer.Write(o_header.interfacesSize);
            writer.Write(o_header.stringLiteralDataSize);
            writer.Write(o_header.vtableMethodsSize);
            writer.Write(o_header.stringSize);
            writer.Write(o_header.interfaceOffsetsSize);
            writer.Write(o_header.eventsSize);
            writer.Write(o_header.typeDefinitionsSize);
            writer.Write(o_header.parameterDefaultValuesSize);
            writer.Write(o_header.imagesSize);
            writer.Write(o_header.fieldDefaultValuesSize);
            writer.Write(o_header.assembliesSize);
            writer.Write(o_header.propertiesSize);
            writer.Write(o_header.fieldRefsSize);
            writer.Write(o_header.methodsSize);
            writer.Write(o_header.referencedAssembliesSize);
            writer.Write(o_header.attributeDataOffset);
            writer.Write(o_header.attributeDataSize);
            writer.Write(o_header.attributeDataRangeOffset);
            writer.Write(o_header.attributeDataRangeSize);
            writer.Write(o_header.fieldAndParameterDefaultValueDataSize);
            writer.Write(o_header.unresolvedVirtualCallParameterTypesSize);
            writer.Write(o_header.fieldMarshaledSizesSize);
            writer.Write(o_header.unresolvedVirtualCallParameterRangesSize);
            writer.Write(o_header.parametersSize);
            writer.Write(o_header.windowsRuntimeTypeNamesSize);
            writer.Write(o_header.fieldsSize);
            writer.Write(o_header.windowsRuntimeStringsSize);
            writer.Write(o_header.genericParametersSize);
            writer.Write(o_header.exportedTypeDefinitionsSize);
            writer.Write(o_header.genericParameterConstraintsSize);
            stream.Position = 0;
            return Tools.StreamToBytes(stream);
        }
    }
        
    public static class Crypt
    {
        public static List<byte[]> Cryptstring(List<byte[]> bytes)
        {
            List<byte[]> result = new List<byte[]>();
            foreach (byte[] b in bytes)
            {
                result.Add(CryptByte(b));
            }
            return result;
        }
        public static byte[] CryptByte(byte[] b)
        {
            byte[] result = new byte[b.Length];
            for(int i = 0; i < b.Length; i++)
            {
                result[i] = (byte)(b[i]^114514);
            }
            return result;
        }
        public static byte[] CryptMetadataBody(byte[] data)
        {
            byte[] sign = Encoding.UTF8.GetBytes("O&Z_IL2CPP");
            return Tools.addBytes(sign, XXTEA.Encrypt(data, "114514"));
        }
        public static byte[] CryptHeader(byte[] data,IL2CPP_Version ver)
        {
            byte[] ret = XXTEA.Encrypt(data, "114514");
            Stream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(ret.Length);
            writer.Write(ret);
            return Tools.StreamToBytes(stream);
        }
    }
    public static class AssetBundleCrypt
    {
        public static byte[] CryptAssetBundle(byte[] data)
        {
            byte[] sign = Encoding.UTF8.GetBytes("O&Z_ASSET");
            return Tools.addBytes(sign, XXTEA.Encrypt(data, "114514"));
        }
    }
}
