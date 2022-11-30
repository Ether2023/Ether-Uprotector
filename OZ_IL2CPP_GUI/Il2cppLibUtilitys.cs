using System;
using System.Collections.Generic;
using System.Text;

namespace OZ_IL2CPP_GUI
{
    class Il2cppLibUtilitys
    {
        public static string GetVersion(UnityVersion ver)
        {
            if(ver.IsLower(new UnityVersion("4.6")))
            {
                throw new Exception("Unity not support il2cpp");
            }
            if(ver.IsInRange(new UnityVersion("2017.0"), new UnityVersion("2018.3")))
            {
                return "24.0";
            }
            if (ver.IsInRange(new UnityVersion("2018.3"), new UnityVersion("2018.5")))
            {
                return "24.1";
            }
            if (ver.IsInRange(new UnityVersion("2019.0"), new UnityVersion("2019.3")))
            {
                return "24.2";
            }
            if (ver.IsInRange(new UnityVersion("2019.3"), new UnityVersion("2020.2")))
            {
                return "24.4";
            }
            if (ver.IsInRange(new UnityVersion("2020.2"), new UnityVersion("2020.4")))
            {
                return "27.1";
            }
            if (ver.IsInRange(new UnityVersion("2021.2"), new UnityVersion("2020.2")))
            {
                return "27.0";
            }
            if (ver.IsInRange(new UnityVersion("2020.1"), new UnityVersion("2020.3")))
            {
                return "27.2";
            }
            if (ver.IsInRange(new UnityVersion("2020.3"), new UnityVersion("2022.2")))
            {
                return "28";
            }
            return "Unknown";
        }

        public static bool HasOZSupport(string v)
        {
            if(v=="24.4"|| v == "28.0" || v == "28")
            {
                return true;
            }
            return false;
        }
    }
}
