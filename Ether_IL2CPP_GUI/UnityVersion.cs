using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_IL2CPP_GUI
{
    class UnityVersion
    {
        int f, s, t;

        public UnityVersion(string str)
        {
            string[] spl = str.Split('.');
            f = int.Parse(spl[0]);
            if (spl.Length >= 3)
            {
                s = int.Parse(spl[1]);
                t = int.Parse(spl[2]);
            }
            else if (spl.Length == 2)
            {
                s = int.Parse(spl[1]);
                t = int.MinValue;
            }
            else
            {
                s = int.MinValue;
                t = int.MinValue;
            }
        }

        public bool IsGreater(UnityVersion ver)
        {
            if (f > ver.f)
            {
                return true;
            }
            if(f== ver.f && s > ver.s)
            {
                return true;
            }
            if(f== ver.f && s == ver.s && t > ver.t)
            {
                return true;
            }
            return false;
        }

        public bool IsLower(UnityVersion ver)
        {
            if (f < ver.f)
            {
                return true;
            }
            if (f == ver.f && s < ver.s)
            {
                return true;
            }
            if (f == ver.f && s == ver.s && t < ver.t)
            {
                return true;
            }
            return false;
        }

        public bool IsInRange(UnityVersion ver1, UnityVersion ver2)
        {
            return (IsGreater(ver1) && IsLower(ver2)) || (IsGreater(ver2) && IsLower(ver1));
        }
    }
}
