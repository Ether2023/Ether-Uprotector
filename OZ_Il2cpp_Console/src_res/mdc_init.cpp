/********************Start OZ Il2cpp Encryption********************/
    frontHeader = (FrontHeader *)s_GlobalMetadata;
    char *Headerdata = (char *)malloc(frontHeader->legnth);
    size_t Headerlen;
    memcpy(Headerdata, (char*)s_GlobalMetadata + frontHeader->offset, frontHeader->legnth);
    char *Header = (char *)xxtea_decrypt(Headerdata, frontHeader->legnth, frontHeader->key, &Headerlen);
    s_GlobalMetadataHeader = (const Il2CppGlobalMetadataHeader*)Header;
/********************End OZ Il2cpp Encryption********************/