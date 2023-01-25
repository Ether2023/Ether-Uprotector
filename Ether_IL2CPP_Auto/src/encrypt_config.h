#pragma once
#include <iostream>

struct encrypt_config {
    // basic config
    std::string logfile = "";
    std::string unity_version = "2018.4.23";

    // custom options
    std::string encrypt_key = "REPLACE_THIS_FOR_A_CUSTOM_KEY";
    bool enable_check_sum = true;
    // not supported below
    bool enable_strings_encrypt = false;
    bool enable_api_obfuscate = false;
    bool enable_proxy_check = false;
    bool enable_console_for_win = false;
};
