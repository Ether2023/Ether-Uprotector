using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xxtea;

namespace O_Z_IL2CPP_Security
{
    public class crypted_Header
    {
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
            public int windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
            public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex

        }
        public o_Header o_header;
        public crypted_Header(MetadataHeader metadataHeader)
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
            return StreamToBytes(stream);
        }
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
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
