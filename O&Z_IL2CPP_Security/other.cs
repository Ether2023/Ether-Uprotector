using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Z_IL2CPP_Security
{
    public class Il2CppGlobalMetadataHeader
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
        public int fieldMarshaledSizesOffset; // Il2CppFieldMarshaledSize
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
        public int interfaceOffsetsOffset; // Il2CppInterfaceOffsetPair
        public int interfaceOffsetsSize;
        public uint typeDefinitionsOffset; // Il2CppTypeDefinition
        public int typeDefinitionsSize;
        [Version(Max = 24.1)]
        public uint rgctxEntriesOffset; // Il2CppRGCTXDefinition
        [Version(Max = 24.1)]
        public int rgctxEntriesCount;
        public uint imagesOffset; // Il2CppImageDefinition
        public int imagesSize;
        public uint assembliesOffset; // Il2CppAssemblyDefinition
        public int assembliesSize;
        [Version(Min = 19, Max = 24.5)]
        public uint metadataUsageListsOffset; // Il2CppMetadataUsageList
        [Version(Min = 19, Max = 24.5)]
        public int metadataUsageListsCount;
        [Version(Min = 19, Max = 24.5)]
        public uint metadataUsagePairsOffset; // Il2CppMetadataUsagePair
        [Version(Min = 19, Max = 24.5)]
        public int metadataUsagePairsCount;
        [Version(Min = 19)]
        public uint fieldRefsOffset; // Il2CppFieldRef
        [Version(Min = 19)]
        public int fieldRefsSize;
        [Version(Min = 20)]
        public int referencedAssembliesOffset; // int32_t
        [Version(Min = 20)]
        public int referencedAssembliesSize;
        [Version(Min = 21, Max = 27.2)]
        public uint attributesInfoOffset; // Il2CppCustomAttributeTypeRange
        [Version(Min = 21, Max = 27.2)]
        public int attributesInfoCount;
        [Version(Min = 21, Max = 27.2)]
        public uint attributeTypesOffset; // TypeIndex
        [Version(Min = 21, Max = 27.2)]
        public int attributeTypesCount;
        [Version(Min = 29)]
        public uint attributeDataOffset;
        [Version(Min = 29)]
        public int attributeDataSize;
        [Version(Min = 29)]
        public uint attributeDataRangeOffset;
        [Version(Min = 29)]
        public int attributeDataRangeSize;
        [Version(Min = 22)]
        public int unresolvedVirtualCallParameterTypesOffset; // TypeIndex
        [Version(Min = 22)]
        public int unresolvedVirtualCallParameterTypesSize;
        [Version(Min = 22)]
        public int unresolvedVirtualCallParameterRangesOffset; // Il2CppRange
        [Version(Min = 22)]
        public int unresolvedVirtualCallParameterRangesSize;
        [Version(Min = 23)]
        public int windowsRuntimeTypeNamesOffset; // Il2CppWindowsRuntimeTypeNamePair
        [Version(Min = 23)]
        public int windowsRuntimeTypeNamesSize;
        [Version(Min = 27)]
        public int windowsRuntimeStringsOffset; // const char*
        [Version(Min = 27)]
        public int windowsRuntimeStringsSize;
        [Version(Min = 24)]
        public int exportedTypeDefinitionsOffset; // TypeDefinitionIndex
        [Version(Min = 24)]
        public int exportedTypeDefinitionsSize;
    }
    public class Il2CppImageDefinition
    {
        public uint nameIndex;
        public int assemblyIndex;

        public int typeStart;
        public uint typeCount;

        [Version(Min = 24)]
        public int exportedTypeStart;
        [Version(Min = 24)]
        public uint exportedTypeCount;

        public int entryPointIndex;
        [Version(Min = 19)]
        public uint token;

        [Version(Min = 24.1)]
        public int customAttributeStart;
        [Version(Min = 24.1)]
        public uint customAttributeCount;
    }
    public class Il2CppAssemblyDef
    {
        public int imageIndex;
        [Version(Min = 24.1)]
        public uint token;
        [Version(Max = 24)]
        public int customAttributeIndex;
        [Version(Min = 20)]
        public int referencedAssemblyStart;
        [Version(Min = 20)]
        public int referencedAssemblyCount;
        public Il2CppAssemblyNameDefinition aname;
    }
}
