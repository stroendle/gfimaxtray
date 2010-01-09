using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheKunde
    {
        public string name = "";
        public string clientid = "";

        public cacheKunde(string tmpname, string tmpclientid)
        {
            name = tmpname;
            clientid = tmpclientid;
        }
    }
}
