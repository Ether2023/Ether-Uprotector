    // dec fronthead
	char* char_gbmd = (char*)s_GlobalMetadata;
	
	// keys
    const char* key = "REPLACE_THIS_FOR_A_CUSTOM_KEY";
    const char* header_key = "REPLACE_THIS_FOR_A_CUSTOM_KEY";
	
    encryption::oz_encryption_new(&char_gbmd, sizeof(FrontHeader), key, (int)strlen(key));
    FrontHeader* frontHeader;
    frontHeader = (FrontHeader*)char_gbmd;
    if (frontHeader->sanity != 8102084797857888879) {
        exit(-1);
    }
	
    char* headerdata = (char*)malloc(frontHeader->length);
    size_t headerlen;
    memcpy(headerdata, (char*)s_GlobalMetadata + frontHeader->offset, frontHeader->length);
    char* header = headerdata;
    encryption::oz_encryption_new(&header, frontHeader->length, header_key, (int)strlen(header_key));
	s_GlobalMetadataHeader = (Il2CppGlobalMetadataHeader*)header;
	
    // dec datas
    char* final_gbmd = (char*)malloc(frontHeader->offset);
	memcpy(final_gbmd, s_GlobalMetadata, frontHeader->offset);
    encryption::oz_encryption(final_gbmd + sizeof(FrontHeader), frontHeader->offset - sizeof(FrontHeader), key, (int)strlen(key));
	s_GlobalMetadata = (void*)final_gbmd;
#if _WIN32
	encryption::check_sum_gameassembly(encryption::get_string_decrypt("F\\l\\>jldjYkt-[ic", -667221561), frontHeader->crc_userassembly);
#endif
#if _ANDROID_
#endif