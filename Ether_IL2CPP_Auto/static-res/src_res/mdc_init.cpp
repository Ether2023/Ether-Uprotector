    // dec fronthead
	char* gbmdFileData = (char*)s_GlobalMetadata;
	
	// keys
    const char* key = $CUSTOM_KEY;
    // const char* header_key = $CUSTOM_HDR_KEY;

    char* frontHrdBytes = (char*)malloc(sizeof(FrontHeader));
    memcpy(frontHrdBytes, gbmdFileData, sizeof(FrontHeader));

    encryption::oz_encryption(frontHrdBytes, (int)sizeof(FrontHeader), key, (int)strlen(key));
    frontHeader = (FrontHeader*)frontHrdBytes;
    if (frontHeader->sanity != 8102084797857888879) {
        encryption::kill_process();
    }
	
    char* hdrBytes = (char*)malloc(frontHeader->length);
    size_t hdrLen = frontHeader->length;
    memcpy(hdrBytes, (char*)s_GlobalMetadata + frontHeader->offset, frontHeader->length);
    encryption::oz_encryption(hdrBytes, (int)frontHeader->length, key, (int)strlen(key));
	s_GlobalMetadataHeader = (Il2CppGlobalMetadataHeader*)hdrBytes;
	
    // dec datas
    char* gbmdBytes = (char*)malloc(frontHeader->offset);
	memcpy(gbmdBytes, s_GlobalMetadata, frontHeader->offset);
    encryption::oz_encryption(gbmdBytes + sizeof(FrontHeader), (int)(frontHeader->offset - sizeof(FrontHeader)), key, (int)strlen(key));
	s_GlobalMetadata = (void*)gbmdBytes;
#if _WIN32
    if($ENABLE_WIN_CHECKSUM == 1){
        encryption::check_sum_gameassembly(frontHeader->crc_x32 ,frontHeader->crc_x64);
    }
#endif
#if _ANDROID_
    if($ENABLE_WIN_CHECKSUM == 1){
        encryption::check_sum_libil2cpp(frontHeader->crc_x32 ,frontHeader->crc_x64);
    }
#endif