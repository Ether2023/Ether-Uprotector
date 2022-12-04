/********************Start OZ Il2cpp Encryption********************/
    char* s = (char*)s_GlobalMetadata + s_GlobalMetadataHeader->stringLiteralDataOffset + stringLiteral->dataIndex;
    for(int i=0;i<stringLiteral->length;i++){
        s[i] ^= 123;
    }
/********************End OZ Il2cpp Encryption********************/