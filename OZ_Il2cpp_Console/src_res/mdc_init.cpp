/********************Start OZ Il2cpp Encryption********************/
    frontHeader = (FrontHeader *)s_GlobalMetadata;
    char *headerdata = (char *)malloc(frontHeader->legnth);
    size_t headerlen;
    memcpy(headerdata, (char*)s_GlobalMetadata + frontHeader->offset, frontHeader->legnth);
    char *header = (char *)xxtea_decrypt(headerdata, frontHeader->legnth, frontHeader->key, &headerlen);
    s_GlobalMetadataHeader = (const Il2CppGlobalMetadataHeader*)header;
/********************End OZ Il2cpp Encryption********************/