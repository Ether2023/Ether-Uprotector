//FrontHeader
#pragma pack(push, p1, 4)
typedef struct FrontHeader
{
    char sign[24];
    int64_t offset;
    int32_t legnth;
    unsigned char key[32];
} FrontHeader;
#pragma pack(pop, p1)