using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Z_IL2CPP_Security
{
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
        public uint windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
        public int windowsRuntimeTypeNamesSize;
        public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex
        public int exportedTypeDefinitionsCount;
    }
    public struct MetadataHeader_v29
    {
        public uint sanity;
        public int version;
        public uint stringLiteralOffset; // string data for managed code
        public int stringLiteralSize;
        public uint stringLiteralDataOffset;
        public int stringLiteralDataSize;
        public uint stringOffset; // string data for metadata
        public int stringSize;
        public uint eventsOffset; // Il2CppEventDefinition
        public int eventsSize;
        public uint propertiesOffset; // Il2CppPropertyDefinition
        public int propertiesSize;
        public uint methodsOffset; // Il2CppMethodDefinition
        public int methodsSize;
        public uint parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
        public int parameterDefaultValuesSize;
        public uint fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
        public int fieldDefaultValuesSize;
        public uint fieldAndParameterDefaultValueDataOffset; // uint8_t
        public int fieldAndParameterDefaultValueDataSize;
        public uint fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
        public int fieldMarshaledSizesSize;
        public uint parametersOffset; // Il2CppParameterDefinition
        public int parametersSize;
        public uint fieldsOffset; // Il2CppFieldDefinition
        public int fieldsSize;
        public uint genericParametersOffset; // Il2CppGenericParameter
        public int genericParametersSize;
        public uint genericParameterConstraintsOffset; // TypeIndex
        public int genericParameterConstraintsSize;
        public uint genericContainersOffset; // Il2CppGenericContainer
        public int genericContainersSize;
        public uint nestedTypesOffset; // TypeDefinitionIndex
        public int nestedTypesSize;
        public uint interfacesOffset; // TypeIndex
        public int interfacesSize;
        public uint vtableMethodsOffset; // EncodedMethodIndex
        public int vtableMethodsSize;
        public uint interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsSize;
        public uint typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsSize;
        public uint imagesOffset; // Il2CppImageDefinition
        public int imagesSize;
        public uint assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesSize;
        public uint fieldRefsOffset; // Il2CppFieldRef
        public int fieldRefsSize;
        public uint referencedAssembliesOffset; // int32_t
        public int referencedAssembliesSize;
        public uint attributeDataOffset;
        public int attributeDataSize;
        public uint attributeDataRangeOffset;
        public int attributeDataRangeSize;
        public uint unresolvedVirtualCallParameterTypesOffset; // TypeIndex
        public int unresolvedVirtualCallParameterTypesSize;
        public uint unresolvedVirtualCallParameterRangesOffset; // Il2CppMetadataRange
        public int unresolvedVirtualCallParameterRangesSize;
        public uint windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
        public int windowsRuntimeTypeNamesSize;
        public uint windowsRuntimeStringsOffset; // const char*
        public int windowsRuntimeStringsSize;
        public uint exportedTypeDefinitionsOffset; // TypeDefinitionIndex
        public int exportedTypeDefinitionsSize;
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
            Header.windowsRuntimeTypeNamesOffset = Reader.ReadUInt32(); // Il2CppWindowsRuntimeTypeNamePair
            Header.windowsRuntimeTypeNamesSize = Reader.ReadInt32();
            Header.exportedTypeDefinitionsOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.exportedTypeDefinitionsCount = Reader.ReadInt32();

            metadatastream.Position = 0;
        }
    }
    public class LoadMetadata_v29
    {
        public Stream metadatastream;
        public MetadataHeader_v29 Header;
        public LoadMetadata_v29(Stream i_metadatastream)
        {
            metadatastream = i_metadatastream;
            BinaryReader Reader = new BinaryReader(metadatastream);
            
            Header.sanity = Reader.ReadUInt32();
            Header.version = Reader.ReadInt32();
            Header.stringLiteralOffset = Reader.ReadUInt32(); // string data for managed code
            Header.stringLiteralSize = Reader.ReadInt32();
            Header.stringLiteralDataOffset = Reader.ReadUInt32();
            Header.stringLiteralDataSize = Reader.ReadInt32();
            Header.stringOffset = Reader.ReadUInt32(); // string data for metadata
            Header.stringSize = Reader.ReadInt32();
            Header.eventsOffset = Reader.ReadUInt32(); // Il2CppEventDefinition
            Header.eventsSize = Reader.ReadInt32();
            Header.propertiesOffset = Reader.ReadUInt32(); // Il2CppPropertyDefinition
            Header.propertiesSize = Reader.ReadInt32();
            Header.methodsOffset = Reader.ReadUInt32(); // Il2CppMethodDefinition
            Header.methodsSize = Reader.ReadInt32();
            Header.parameterDefaultValuesOffset = Reader.ReadUInt32(); // Il2CppParameterDefaultValue
            Header.parameterDefaultValuesSize = Reader.ReadInt32();
            Header.fieldDefaultValuesOffset = Reader.ReadUInt32(); // Il2CppFieldDefaultValue
            Header.fieldDefaultValuesSize = Reader.ReadInt32();
            Header.fieldAndParameterDefaultValueDataOffset = Reader.ReadUInt32(); // uint8_t
            Header.fieldAndParameterDefaultValueDataSize = Reader.ReadInt32();
            Header.fieldMarshaledSizesOffset = Reader.ReadUInt32(); // Il2CppFieldMarshaledSize
            Header.fieldMarshaledSizesSize = Reader.ReadInt32();
            Header.parametersOffset = Reader.ReadUInt32(); // Il2CppParameterDefinition
            Header.parametersSize = Reader.ReadInt32();
            Header.fieldsOffset = Reader.ReadUInt32(); // Il2CppFieldDefinition
            Header.fieldsSize = Reader.ReadInt32();
            Header.genericParametersOffset = Reader.ReadUInt32(); // Il2CppGenericParameter
            Header.genericParametersSize = Reader.ReadInt32();
            Header.genericParameterConstraintsOffset = Reader.ReadUInt32(); // TypeIndex
            Header.genericParameterConstraintsSize = Reader.ReadInt32();
            Header.genericContainersOffset = Reader.ReadUInt32(); // Il2CppGenericContainer
            Header.genericContainersSize = Reader.ReadInt32();
            Header.nestedTypesOffset = Reader.ReadUInt32(); // TypeDefinitionIndex
            Header.nestedTypesSize = Reader.ReadInt32();
            Header.interfacesOffset = Reader.ReadUInt32(); // TypeIndex
            Header.interfacesSize = Reader.ReadInt32();
            Header.vtableMethodsOffset = Reader.ReadUInt32(); // EncodedMethodIndex
            Header.vtableMethodsSize = Reader.ReadInt32();
            Header.interfaceOffsetsOffset = Reader.ReadUInt32(); // Il2CppInterfaceOffsetPair
            Header.interfaceOffsetsSize = Reader.ReadInt32();
            Header.typeDefinitionsOffset = Reader.ReadUInt32(); // Il2CppTypeDefinition
            Header.typeDefinitionsSize = Reader.ReadInt32();
            Header.imagesOffset = Reader.ReadUInt32(); // Il2CppImageDefinition
            Header.imagesSize = Reader.ReadInt32();
            Header.assembliesOffset = Reader.ReadUInt32(); // Il2CppAssemblyDefinition
            Header.assembliesSize = Reader.ReadInt32();
            Header.fieldRefsOffset = Reader.ReadUInt32(); // Il2CppFieldRef
            Header.fieldRefsSize = Reader.ReadInt32();
            Header.referencedAssembliesOffset = Reader.ReadUInt32(); // int32_t
            Header.referencedAssembliesSize = Reader.ReadInt32();
            Header.attributeDataOffset = Reader.ReadUInt32();
            Header.attributeDataSize = Reader.ReadInt32();
            Header.attributeDataRangeOffset = Reader.ReadUInt32();
            Header.attributeDataRangeSize = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterTypesOffset = Reader.ReadUInt32(); // TypeIndex
            Header.unresolvedVirtualCallParameterTypesSize = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterRangesOffset = Reader.ReadUInt32(); // Il2CppMetadataRange
            Header.unresolvedVirtualCallParameterRangesSize = Reader.ReadInt32();
            Header.windowsRuntimeTypeNamesOffset = Reader.ReadUInt32(); // Il2CppWindowsRuntimeTypeNamePair
            Header.windowsRuntimeTypeNamesSize = Reader.ReadInt32();
            Header.windowsRuntimeStringsOffset = Reader.ReadUInt32(); // const char*
            Header.windowsRuntimeStringsSize = Reader.ReadInt32();
            Header.exportedTypeDefinitionsOffset = Reader.ReadUInt32(); // TypeDefinitionIndex
            Header.exportedTypeDefinitionsSize = Reader.ReadInt32();
        }
    }
}
