#pragma once
#include <iostream>
#include <vector>

struct encrypt_file {
    std::string file_path;
    std::string file_type;
};

struct encrypt_file_config {
    //files
    std::string libunity32;
    std::string libunity64;
    std::string libil2cpp32;
    std::string libil2cpp64;
    std::string globalmetadata;
};