using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Z_IL2CPP_Security
{
    public class CPP
    {
        public string retsrc;
        public CPP(string src,IL2CPP_Version ver,int key,int skip)
        {
            retsrc = src.Replace("*|*|*#skip#*|*|*", skip.ToString());
            retsrc = retsrc.Replace("*|*|*#key#*|*|*", key.ToString());
        }
    }
}
