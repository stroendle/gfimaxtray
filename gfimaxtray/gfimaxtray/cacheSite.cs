using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheSite
    {
        public string siteid = "";
        public string sitename = "";
        public cacheClient siteCustomer;

        public cacheSite(string tmpsitename, string tmpsiteid, cacheClient tmpKunde)
        {
            siteid = tmpsiteid;
            sitename = tmpsitename;
            siteCustomer = tmpKunde;
        }
    }
}
