using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Z_IL2CPP_Security
{
    public enum IL2CPP_Version
    {
        V24_1,
        V24_4,
        V28
    };
    public struct MetadataHeader_v24_4
    {
        public int sanity;
        public int version;
        public int stringLiteralOffset; // string data for managed code
        public int stringLiteralCount;
        public int stringLiteralDataOffset;
        public int stringLiteralDataCount;
        public int stringOffset; // string data for metadata
        public int stringCount;
        public int eventsOffset; // Il2CppEventDefinition
        public int eventsCount;
        public int propertiesOffset; // Il2CppPropertyDefinition
        public int propertiesCount;
        public int methodsOffset; // Il2CppMethodDefinition
        public int methodsCount;
        public int parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
        public int parameterDefaultValuesCount;
        public int fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
        public int fieldDefaultValuesCount;
        public int fieldAndParameterDefaultValueDataOffset; // uint8_t
        public int fieldAndParameterDefaultValueDataCount;
        public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
        public int fieldMarshaledSizesCount;
        public int parametersOffset; // Il2CppParameterDefinition
        public int parametersCount;
        public int fieldsOffset; // Il2CppFieldDefinition
        public int fieldsCount;
        public int genericParametersOffset; // Il2CppGenericParameter
        public int genericParametersCount;
        public int genericParameterConstraintsOffset; // TypeIndex
        public int genericParameterConstraintsCount;
        public int genericContainersOffset; // Il2CppGenericContainer
        public int genericContainersCount;
        public int nestedTypesOffset; // TypeDefinitionIndex
        public int nestedTypesCount;
        public int interfacesOffset; // TypeIndex
        public int interfacesCount;
        public int vtableMethodsOffset; // EncodedMethodIndex
        public int vtableMethodsCount;
        public int interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsCount;
        public int typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsCount;
        public int rgctxEntriesOffset; // *
        public int rgctxEntriesCount; // *
        public int imagesOffset; // Il2CppImageDefinition
        public int imagesCount;
        public int assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesCount;
        public int metadataUsageListsOffset; // Il2CppMetadataUsageList
        public int metadataUsageListsCount;
        public int metadataUsagePairsOffset; // Il2CppMetadataUsagePair
        public int metadataUsagePairsCount;
        public int fieldRefsOffset; // Il2CppFieldRef
        public int fieldRefsCount;
        public int referencedAssembliesOffset; // public UInt32
        public int referencedAssembliesCount;
        public int attributesInfoOffset; // Il2CppCustomAttributeTypeRange
        public int attributesInfoCount;
        public int attributeTypesOffset; // TypeIndex
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
    public struct MetadataHeader_v28
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
        public int fieldAndParameterDefaultValueDataOffset; // int8_t
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
    public struct MetadataHeader_v24_1
    {
        public int sanity;
        public int version;
        public int stringLiteralOffset; // string data for managed code
        public int stringLiteralCount;
        public int stringLiteralDataOffset;
        public int stringLiteralDataCount;
        public int stringOffset; // string data for metadata
        public int stringCount;
        public int eventsOffset; // Il2CppEventDefinition
        public int eventsCount;
        public int propertiesOffset; // Il2CppPropertyDefinition
        public int propertiesCount;
        public int methodsOffset; // Il2CppMethodDefinition
        public int methodsCount;
        public int parameterDefaultValuesOffset; // Il2CppParameterDefaultValue
        public int parameterDefaultValuesCount;
        public int fieldDefaultValuesOffset; // Il2CppFieldDefaultValue
        public int fieldDefaultValuesCount;
        public int fieldAndParameterDefaultValueDataOffset; // int8_t
        public int fieldAndParameterDefaultValueDataCount;
        public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
        public int fieldMarshaledSizesCount;
        public int parametersOffset; // Il2CppParameterDefinition
        public int parametersCount;
        public int fieldsOffset; // Il2CppFieldDefinition
        public int fieldsCount;
        public int genericParametersOffset; // Il2CppGenericParameter
        public int genericParametersCount;
        public int genericParameterConstraintsOffset; // TypeIndex
        public int genericParameterConstraintsCount;
        public int genericContainersOffset; // Il2CppGenericContainer
        public int genericContainersCount;
        public int nestedTypesOffset; // TypeDefinitionIndex
        public int nestedTypesCount;
        public int interfacesOffset; // TypeIndex
        public int interfacesCount;
        public int vtableMethodsOffset; // EncodedMethodIndex
        public int vtableMethodsCount;
        public int interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsCount;
        public int typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsCount;
        public int rgctxEntriesOffset; // Il2CppRGCTXDefinition
        public int rgctxEntriesCount;
        public int imagesOffset; // Il2CppImageDefinition
        public int imagesCount;
        public int assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesCount;
        public int metadataUsageListsOffset; // Il2CppMetadataUsageList
        public int metadataUsageListsCount;
        public int metadataUsagePairsOffset; // Il2CppMetadataUsagePair
        public int metadataUsagePairsCount;
        public int fieldRefsOffset; // Il2CppFieldRef
        public int fieldRefsCount;
        public int referencedAssembliesOffset; // int32_t
        public int referencedAssembliesCount;
        public int attributesInfoOffset; // Il2CppCustomAttributeTypeRange
        public int attributesInfoCount;
        public int attributeTypesOffset; // TypeIndex
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
    public class LoadMetadata_v24_4
    {
        public Stream metadatastream;
        public MetadataHeader_v24_4 Header;
        public LoadMetadata_v24_4(Stream i_metadatastream)
        {
            metadatastream = i_metadatastream;
            BinaryReader Reader = new BinaryReader(metadatastream);
            Header.sanity = Reader.ReadInt32();
            Header.version = Reader.ReadInt32();
            Header.stringLiteralOffset = Reader.ReadInt32(); // string data for managed code
            Header.stringLiteralCount = Reader.ReadInt32();
            Header.stringLiteralDataOffset = Reader.ReadInt32();
            Header.stringLiteralDataCount = Reader.ReadInt32();
            Header.stringOffset = Reader.ReadInt32(); // string data for metadata
            Header.stringCount = Reader.ReadInt32();
            Header.eventsOffset = Reader.ReadInt32(); // Il2CppEventDefinition
            Header.eventsCount = Reader.ReadInt32();
            Header.propertiesOffset = Reader.ReadInt32(); // Il2CppPropertyDefinition
            Header.propertiesCount = Reader.ReadInt32();
            Header.methodsOffset = Reader.ReadInt32(); // Il2CppMethodDefinition
            Header.methodsCount = Reader.ReadInt32();
            Header.parameterDefaultValuesOffset = Reader.ReadInt32(); // Il2CppParameterDefaultValue
            Header.parameterDefaultValuesCount = Reader.ReadInt32();
            Header.fieldDefaultValuesOffset = Reader.ReadInt32(); // Il2CppFieldDefaultValue
            Header.fieldDefaultValuesCount = Reader.ReadInt32();
            Header.fieldAndParameterDefaultValueDataOffset = Reader.ReadInt32(); // uint8_t
            Header.fieldAndParameterDefaultValueDataCount = Reader.ReadInt32();
            Header.fieldMarshaledSizesOffset = Reader.ReadInt32(); // Il2CppFieldMarshaledSize
            Header.fieldMarshaledSizesCount = Reader.ReadInt32();
            Header.parametersOffset = Reader.ReadInt32(); // Il2CppParameterDefinition
            Header.parametersCount = Reader.ReadInt32();
            Header.fieldsOffset = Reader.ReadInt32(); // Il2CppFieldDefinition
            Header.fieldsCount = Reader.ReadInt32();
            Header.genericParametersOffset = Reader.ReadInt32(); // Il2CppGenericParameter
            Header.genericParametersCount = Reader.ReadInt32();
            Header.genericParameterConstraintsOffset = Reader.ReadInt32(); // TypeIndex
            Header.genericParameterConstraintsCount = Reader.ReadInt32();
            Header.genericContainersOffset = Reader.ReadInt32(); // Il2CppGenericContainer
            Header.genericContainersCount = Reader.ReadInt32();
            Header.nestedTypesOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.nestedTypesCount = Reader.ReadInt32();
            Header.interfacesOffset = Reader.ReadInt32(); // TypeIndex
            Header.interfacesCount = Reader.ReadInt32();
            Header.vtableMethodsOffset = Reader.ReadInt32(); // EncodedMethodIndex
            Header.vtableMethodsCount = Reader.ReadInt32();
            Header.interfaceOffsetsOffset = Reader.ReadInt32(); // Il2CppInterfaceOffsetPair
            Header.interfaceOffsetsCount = Reader.ReadInt32();
            Header.typeDefinitionsOffset = Reader.ReadInt32(); // Il2CppTypeDefinition
            Header.typeDefinitionsCount = Reader.ReadInt32();
            Header.imagesOffset = Reader.ReadInt32(); // Il2CppImageDefinition
            Header.imagesCount = Reader.ReadInt32();
            Header.assembliesOffset = Reader.ReadInt32(); // Il2CppAssemblyDefinition
            Header.assembliesCount = Reader.ReadInt32();
            Header.metadataUsageListsOffset = Reader.ReadInt32(); // Il2CppMetadataUsageList
            Header.metadataUsageListsCount = Reader.ReadInt32();
            Header.metadataUsagePairsOffset = Reader.ReadInt32(); // Il2CppMetadataUsagePair
            Header.metadataUsagePairsCount = Reader.ReadInt32();
            Header.fieldRefsOffset = Reader.ReadInt32(); // Il2CppFieldRef
            Header.fieldRefsCount = Reader.ReadInt32();
            Header.referencedAssembliesOffset = Reader.ReadInt32(); // int32
            Header.referencedAssembliesCount = Reader.ReadInt32();
            Header.attributesInfoOffset = Reader.ReadInt32(); // Il2CppCustomAttributeTypeRange
            Header.attributesInfoCount = Reader.ReadInt32();
            Header.attributeTypesOffset = Reader.ReadInt32(); // TypeIndex
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
    public class LoadMetadata_v28
    {
        public Stream metadatastream;
        public MetadataHeader_v28 Header;
        public LoadMetadata_v28(Stream i_metadatastream)
        {
            metadatastream = i_metadatastream;
            BinaryReader Reader = new BinaryReader(metadatastream);
            
            Header.sanity = Reader.ReadInt32();
            Header.version = Reader.ReadInt32();
            Header.stringLiteralOffset = Reader.ReadInt32(); // string data for managed code
            Header.stringLiteralSize = Reader.ReadInt32();
            Header.stringLiteralDataOffset = Reader.ReadInt32();
            Header.stringLiteralDataSize = Reader.ReadInt32();
            Header.stringOffset = Reader.ReadInt32(); // string data for metadata
            Header.stringSize = Reader.ReadInt32();
            Header.eventsOffset = Reader.ReadInt32(); // Il2CppEventDefinition
            Header.eventsSize = Reader.ReadInt32();
            Header.propertiesOffset = Reader.ReadInt32(); // Il2CppPropertyDefinition
            Header.propertiesSize = Reader.ReadInt32();
            Header.methodsOffset = Reader.ReadInt32(); // Il2CppMethodDefinition
            Header.methodsSize = Reader.ReadInt32();
            Header.parameterDefaultValuesOffset = Reader.ReadInt32(); // Il2CppParameterDefaultValue
            Header.parameterDefaultValuesSize = Reader.ReadInt32();
            Header.fieldDefaultValuesOffset = Reader.ReadInt32(); // Il2CppFieldDefaultValue
            Header.fieldDefaultValuesSize = Reader.ReadInt32();
            Header.fieldAndParameterDefaultValueDataOffset = Reader.ReadInt32(); // uint8_t
            Header.fieldAndParameterDefaultValueDataSize = Reader.ReadInt32();
            Header.fieldMarshaledSizesOffset = Reader.ReadInt32(); // Il2CppFieldMarshaledSize
            Header.fieldMarshaledSizesSize = Reader.ReadInt32();
            Header.parametersOffset = Reader.ReadInt32(); // Il2CppParameterDefinition
            Header.parametersSize = Reader.ReadInt32();
            Header.fieldsOffset = Reader.ReadInt32(); // Il2CppFieldDefinition
            Header.fieldsSize = Reader.ReadInt32();
            Header.genericParametersOffset = Reader.ReadInt32(); // Il2CppGenericParameter
            Header.genericParametersSize = Reader.ReadInt32();
            Header.genericParameterConstraintsOffset = Reader.ReadInt32(); // TypeIndex
            Header.genericParameterConstraintsSize = Reader.ReadInt32();
            Header.genericContainersOffset = Reader.ReadInt32(); // Il2CppGenericContainer
            Header.genericContainersSize = Reader.ReadInt32();
            Header.nestedTypesOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.nestedTypesSize = Reader.ReadInt32();
            Header.interfacesOffset = Reader.ReadInt32(); // TypeIndex
            Header.interfacesSize = Reader.ReadInt32();
            Header.vtableMethodsOffset = Reader.ReadInt32(); // EncodedMethodIndex
            Header.vtableMethodsSize = Reader.ReadInt32();
            Header.interfaceOffsetsOffset = Reader.ReadInt32(); // Il2CppInterfaceOffsetPair
            Header.interfaceOffsetsSize = Reader.ReadInt32();
            Header.typeDefinitionsOffset = Reader.ReadInt32(); // Il2CppTypeDefinition
            Header.typeDefinitionsSize = Reader.ReadInt32();
            Header.imagesOffset = Reader.ReadInt32(); // Il2CppImageDefinition
            Header.imagesSize = Reader.ReadInt32();
            Header.assembliesOffset = Reader.ReadInt32(); // Il2CppAssemblyDefinition
            Header.assembliesSize = Reader.ReadInt32();
            Header.fieldRefsOffset = Reader.ReadInt32(); // Il2CppFieldRef
            Header.fieldRefsSize = Reader.ReadInt32();
            Header.referencedAssembliesOffset = Reader.ReadInt32(); // int32_t
            Header.referencedAssembliesSize = Reader.ReadInt32();
            Header.attributeDataOffset = Reader.ReadInt32();
            Header.attributeDataSize = Reader.ReadInt32();
            Header.attributeDataRangeOffset = Reader.ReadInt32();
            Header.attributeDataRangeSize = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterTypesOffset = Reader.ReadInt32(); // TypeIndex
            Header.unresolvedVirtualCallParameterTypesSize = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterRangesOffset = Reader.ReadInt32(); // Il2CppMetadataRange
            Header.unresolvedVirtualCallParameterRangesSize = Reader.ReadInt32();
            Header.windowsRuntimeTypeNamesOffset = Reader.ReadInt32(); // Il2CppWindowsRuntimeTypeNamePair
            Header.windowsRuntimeTypeNamesSize = Reader.ReadInt32();
            Header.windowsRuntimeStringsOffset = Reader.ReadInt32(); // const char*
            Header.windowsRuntimeStringsSize = Reader.ReadInt32();
            Header.exportedTypeDefinitionsOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.exportedTypeDefinitionsSize = Reader.ReadInt32();
        }
    }
    public class LoadMetadata_v24_1
    {
        public Stream metadatastream;
        public MetadataHeader_v24_1 Header;
        public LoadMetadata_v24_1(Stream i_metadatastream)
        {
            metadatastream = i_metadatastream;
            BinaryReader Reader = new BinaryReader(metadatastream);
            Header.sanity = Reader.ReadInt32();
            Header.version = Reader.ReadInt32();
            Header.stringLiteralOffset = Reader.ReadInt32(); // string data for managed code
            Header.stringLiteralCount = Reader.ReadInt32();
            Header.stringLiteralDataOffset = Reader.ReadInt32();
            Header.stringLiteralDataCount = Reader.ReadInt32();
            Header.stringOffset = Reader.ReadInt32(); // string data for metadata
            Header.stringCount = Reader.ReadInt32();
            Header.eventsOffset = Reader.ReadInt32(); // Il2CppEventDefinition
            Header.eventsCount = Reader.ReadInt32();
            Header.propertiesOffset = Reader.ReadInt32(); // Il2CppPropertyDefinition
            Header.propertiesCount = Reader.ReadInt32();
            Header.methodsOffset = Reader.ReadInt32(); // Il2CppMethodDefinition
            Header.methodsCount = Reader.ReadInt32();
            Header.parameterDefaultValuesOffset = Reader.ReadInt32(); // Il2CppParameterDefaultValue
            Header.parameterDefaultValuesCount = Reader.ReadInt32();
            Header.fieldDefaultValuesOffset = Reader.ReadInt32(); // Il2CppFieldDefaultValue
            Header.fieldDefaultValuesCount = Reader.ReadInt32();
            Header.fieldAndParameterDefaultValueDataOffset = Reader.ReadInt32(); // uint8_t
            Header.fieldAndParameterDefaultValueDataCount = Reader.ReadInt32();
            Header.fieldMarshaledSizesOffset = Reader.ReadInt32(); // Il2CppFieldMarshaledSize
            Header.fieldMarshaledSizesCount = Reader.ReadInt32();
            Header.parametersOffset = Reader.ReadInt32(); // Il2CppParameterDefinition
            Header.parametersCount = Reader.ReadInt32();
            Header.fieldsOffset = Reader.ReadInt32(); // Il2CppFieldDefinition
            Header.fieldsCount = Reader.ReadInt32();
            Header.genericParametersOffset = Reader.ReadInt32(); // Il2CppGenericParameter
            Header.genericParametersCount = Reader.ReadInt32();
            Header.genericParameterConstraintsOffset = Reader.ReadInt32(); // TypeIndex
            Header.genericParameterConstraintsCount = Reader.ReadInt32();
            Header.genericContainersOffset = Reader.ReadInt32(); // Il2CppGenericContainer
            Header.genericContainersCount = Reader.ReadInt32();
            Header.nestedTypesOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.nestedTypesCount = Reader.ReadInt32();
            Header.interfacesOffset = Reader.ReadInt32(); // TypeIndex
            Header.interfacesCount = Reader.ReadInt32();
            Header.vtableMethodsOffset = Reader.ReadInt32(); // EncodedMethodIndex
            Header.vtableMethodsCount = Reader.ReadInt32();
            Header.interfaceOffsetsOffset = Reader.ReadInt32(); // Il2CppInterfaceOffsetPair
            Header.interfaceOffsetsCount = Reader.ReadInt32();
            Header.typeDefinitionsOffset = Reader.ReadInt32(); // Il2CppTypeDefinition
            Header.typeDefinitionsCount = Reader.ReadInt32();
            Header.rgctxEntriesOffset = Reader.ReadInt32(); // Il2CppRGCTXDefinition
            Header.rgctxEntriesCount = Reader.ReadInt32();
            Header.imagesOffset = Reader.ReadInt32(); // Il2CppImageDefinition
            Header.imagesCount = Reader.ReadInt32();
            Header.assembliesOffset = Reader.ReadInt32(); // Il2CppAssemblyDefinition
            Header.assembliesCount = Reader.ReadInt32();
            Header.metadataUsageListsOffset = Reader.ReadInt32(); // Il2CppMetadataUsageList
            Header.metadataUsageListsCount = Reader.ReadInt32();
            Header.metadataUsagePairsOffset = Reader.ReadInt32(); // Il2CppMetadataUsagePair
            Header.metadataUsagePairsCount = Reader.ReadInt32();
            Header.fieldRefsOffset = Reader.ReadInt32(); // Il2CppFieldRef
            Header.fieldRefsCount = Reader.ReadInt32();
            Header.referencedAssembliesOffset = Reader.ReadInt32(); // int32_t
            Header.referencedAssembliesCount = Reader.ReadInt32();
            Header.attributesInfoOffset = Reader.ReadInt32(); // Il2CppCustomAttributeTypeRange
            Header.attributesInfoCount = Reader.ReadInt32();
            Header.attributeTypesOffset = Reader.ReadInt32(); // TypeIndex
            Header.attributeTypesCount = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterTypesOffset = Reader.ReadInt32(); // TypeIndex
            Header.unresolvedVirtualCallParameterTypesCount = Reader.ReadInt32();
            Header.unresolvedVirtualCallParameterRangesOffset = Reader.ReadInt32(); // Il2CppRange
            Header.unresolvedVirtualCallParameterRangesCount = Reader.ReadInt32();
            Header.windowsRuntimeTypeNamesOffset = Reader.ReadInt32(); // Il2CppWindowsRuntimeTypeNamePair
            Header.windowsRuntimeTypeNamesSize = Reader.ReadInt32();
            Header.exportedTypeDefinitionsOffset = Reader.ReadInt32(); // TypeDefinitionIndex
            Header.exportedTypeDefinitionsCount = Reader.ReadInt32();
        }
    }
}
