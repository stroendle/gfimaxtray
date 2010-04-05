using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Configuration;

namespace gfimaxtray
{
    class apiCommunication
    {
        public static void GetFailedClients(ref ArrayList FailedCheckList)
        {
            string tmpClientName = "";
            string tmpSiteName = "";
            
            XmlNodeList nlclientlist = getXmlNLFromHttp(clsRegistry.getRegistryValue("dbserver") + "/api?apikey=" + clsRegistry.getRegistryValue("apikey") + "&service=list_failing_checks", "client");
            //Iterate through clients
            foreach (XmlNode nodeclient in nlclientlist)
            {
                tmpClientName = getAttribute(nodeclient, "name", false);

                //Iterate through client's sites
                XmlNodeList nlsitelist = getXmlDocFromString(nodeclient.OuterXml).GetElementsByTagName("site");
                foreach (XmlNode nodesite in nlsitelist) 
                {
                    tmpSiteName=getAttribute(nodesite,"name", false);

                    //Iterate through site's workstations
                    XmlNodeList nlworkstationlist=getXmlDocFromString(nodesite.OuterXml).GetElementsByTagName("workstation");
                    foreach (XmlNode nodeworkstation in nlworkstationlist)
                    {
                        bool tmpIsOffline=hasAttribute(nodeworkstation,"offline");
                        string tmpDeviceName=getAttribute(nodeworkstation,"name", false);

                        //Iterate through the failed checks, if there are any (could be that a workstation is "offline" only
                        bool hasfailedchecks = false;
                        XmlNodeList nlfailedcheckslist = getXmlDocFromString(getAttribute(nodeworkstation,"failed_checks", true)).GetElementsByTagName("check");
                        foreach (XmlNode nodefailedcheck in nlfailedcheckslist)
                        {
                            hasfailedchecks = true;
                            FailedCheckList.Add(new failedCheck(tmpClientName, tmpSiteName, tmpDeviceName, true, tmpIsOffline, getAttribute(nodefailedcheck,"description", false), getAttribute(nodefailedcheck, "formatted_output", false), DateTime.MinValue));
                        }

                        if (hasfailedchecks == false) { FailedCheckList.Add(new failedCheck(tmpClientName, tmpSiteName, tmpDeviceName, true, tmpIsOffline, "", "", DateTime.MinValue)); }
                    }



                    //Iterate through site's servers
                    XmlNodeList nlserverlist = getXmlDocFromString(nodesite.OuterXml).GetElementsByTagName("server");
                    foreach (XmlNode nodeserver in nlserverlist)
                    {
                        bool tmpIsOffline = hasAttribute(nodeserver, "offline");
                        string tmpDeviceName = getAttribute(nodeserver, "name", false);

                        //Iterate through the failed checks, if there are any (could be that a workstation is "offline" only
                        bool hasfailedchecks = false;
                        XmlNodeList nlfailedcheckslist = getXmlDocFromString(getAttribute(nodeserver, "failed_checks", true)).GetElementsByTagName("check");
                        foreach (XmlNode nodefailedcheck in nlfailedcheckslist)
                        {
                            hasfailedchecks = true;
                            FailedCheckList.Add(new failedCheck(tmpClientName, tmpSiteName, tmpDeviceName, false, tmpIsOffline, getAttribute(nodefailedcheck, "description", false), getAttribute(nodefailedcheck, "formatted_output", false), DateTime.MinValue));
                        }

                        if (hasfailedchecks == false) { FailedCheckList.Add(new failedCheck(tmpClientName, tmpSiteName, tmpDeviceName, false, tmpIsOffline, "", "", DateTime.MinValue)); }
                    }

                }
            }

        }


        public static XmlDocument getXmlDocFromString(string Xml)
        {
            XmlDocument doc = new XmlDocument();
            if (Xml != "") { doc.LoadXml(Xml); }
            return doc;
        }

        public static bool hasAttribute(XmlNode node, string attributename)
        {
            bool retval=false;
            foreach (XmlNode childnode in node.ChildNodes)
            {
                if (childnode.Name == attributename)
                {
                    retval = true;
                }
            }

            return retval;
        }



        public static string getAttribute(XmlNode node, string attributename, bool getOuterXml)
        {
            string retval = "";
            foreach (XmlNode childnode in node.ChildNodes)
            {
                if (childnode.Name == attributename)
                {
                    if (getOuterXml) { retval = childnode.OuterXml; } else { retval = childnode.InnerXml; }
                }
            }

            return retval;
        }



        public static XmlNodeList getXmlNLFromHttp(string url, string tagname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            XmlNodeList nl = doc.GetElementsByTagName(tagname);
            return nl;
        }


    }
}
