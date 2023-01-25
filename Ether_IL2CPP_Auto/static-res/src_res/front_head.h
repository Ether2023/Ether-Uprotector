//FrontHeader
#pragma pack(push, p1, 4)
typedef struct FrontHeader
{
    int64_t sanity;// it must be 8102084797857888879
    int64_t offset;
    int32_t length;
    uint32_t crc_x32;
    uint32_t crc_x64;
    char key[128];// it can't be more
} FrontHeader;
#pragma pack(pop, p1)