/********************Start OZ Il2cpp Encryption********************/
    char* pStr = (char*)s_GlobalMetadata + s_GlobalMetadataHeader->stringLiteralDataOffset + stringLiteral->dataIndex;
    for (int i = 0; i < stringLiteral->length; i++) {
        *pStr ^= i*12;
        pStr += 1;
    }
/********************End OZ Il2cpp Encryption********************/