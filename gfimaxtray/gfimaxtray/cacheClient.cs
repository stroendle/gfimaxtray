using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheClient
    {
        public string name = "";
        public string clientid = "";

        public cacheClient(string tmpname, string tmpclientid)
        {
            name = tmpname;
            clientid = tmpclientid;
        }
    }
}
