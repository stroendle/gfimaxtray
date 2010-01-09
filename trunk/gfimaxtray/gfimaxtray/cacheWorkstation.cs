using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheWorkstation
    {
        public string workstationid = "";
        public string workstationname = "";
        public cacheSite workstationsite;

        public cacheWorkstation(string tmpworkstationname, string tmpworkstationid, cacheSite tmpsite)
        {
            workstationid = tmpworkstationid;
            workstationname = tmpworkstationname;
            workstationsite = tmpsite;
        }
    }
}
