using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class cacheSite
    {
        public string siteid = "";
        public string sitename = "";
        public cacheKunde siteCustomer;

        public cacheSite(string tmpsitename, string tmpsiteid, cacheKunde tmpKunde)
        {
            siteid = tmpsiteid;
            sitename = tmpsitename;
            siteCustomer = tmpKunde;
        }
    }
}
