using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheServer
    {
        public string serverid = "";
        public string servername = "";
        public cacheSite serversite;

        public cacheServer(string tmpservername, string tmpserverid, cacheSite tmpsite)
        {
            serverid = tmpserverid;
            servername = tmpservername;
            serversite = tmpsite;
        }
    }
}
