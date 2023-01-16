    
	frontHeader = (FrontHeader *)s_GlobalMetadata;
    char *headerdata = (char *)malloc(frontHeader->legnth);
    size_t headerlen;
    memcpy(headerdata, (char*)s_GlobalMetadata + frontHeader->offset, frontHeader->legnth);
    char *header = headerdata;
	int keylen = strlen(header_key);
	for(int i=0;i<frontHeader->legnth;i++){
		header[i]^=header_key[i%keylen];
	}
    s_GlobalMetadataHeader = (const Il2CppGlobalMetadataHeader*)header;