using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;


namespace gfimaxtray
{
    static class Program
    {
        private static NotifyIcon  notico;

        private static ArrayList arrayclients = new ArrayList();
        private static ArrayList arraysites = new ArrayList();
        private static ArrayList arrayservers = new ArrayList();
        private static ArrayList arrayworkstations = new ArrayList();
        private static bool cacheinitialized = false;


        public static void Main (string [] astrArg)
        {
            ContextMenu cm;
            MenuItem    miCurr;
            int         iIndex = 0;

            // Kontextmenü erzeugen
            cm = new ContextMenu ();

            // Kontextmenüeinträge erzeugen
            miCurr = new MenuItem ();
            miCurr.Index = iIndex++;
            miCurr.Text = "Reload Clients+Sites";
            miCurr.Click += new System.EventHandler (ActionReloadClick);
            cm.MenuItems.Add (miCurr);

            miCurr = new MenuItem();
            miCurr.Index = iIndex++;
            miCurr.Text = "Update Status";
            miCurr.Click += new System.EventHandler(timerRefresh_Tick);
            cm.MenuItems.Add(miCurr);

            // Kontextmenüeinträge erzeugen
            miCurr = new MenuItem ();
            miCurr.Index = iIndex++;
            miCurr.Text = "&Beenden";
            miCurr.Click += new System.EventHandler (ExitClick);
            cm.MenuItems.Add (miCurr);

            // NotifyIcon selbst erzeugen
            notico = new NotifyIcon ();
            notico.Icon = new Icon("icons/maxicon_red.ico"); // Eigenes Icon einsetzen
            notico.Text = "Doppelklick mich!";   // Eigenen Text einsetzen
            notico.Visible = true;
            notico.ContextMenu = cm;
            notico.DoubleClick += new EventHandler (NotifyIconDoubleClick);


            Timer timerRefresh = new Timer();
            timerRefresh.Enabled=false;
            timerRefresh.Interval = 5000;
            timerRefresh.Tick += new EventHandler(timerRefresh_Tick);


            //Cache aktualisieren
            updateCache();


            // Ohne Appplication.Run geht es nicht
            Application.Run ();
        }

        static void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (cacheinitialized)
            {
                foreach (cacheServer tmpserver in arrayservers)
                {
                    XmlNodeList nlchecklist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_checks&deviceid="+tmpserver.serverid, "check");
                    foreach (XmlNode nodecheck in nlchecklist)
                    {
                        string tmpstatusid=getAttribute(nodecheck,"statusid");

                        if (tmpstatusid == "1")
                        {
                            MessageBox.Show(tmpserver.serversite.siteCustomer.name + " - " + tmpserver.serversite.sitename + " - " + tmpserver.servername+ ": " + getAttribute(nodecheck, "description"));
                        }

                    }


                }

                foreach (cacheWorkstation tmpwks in arrayworkstations)
                {
                    XmlNodeList nlchecklist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_checks&deviceid=" + tmpwks.workstationid, "check");
                    foreach (XmlNode nodecheck in nlchecklist)
                    {
                        string tmpstatusid = getAttribute(nodecheck, "statusid");

                        if (tmpstatusid == "1")
                        {
                            MessageBox.Show(tmpwks.workstationsite.siteCustomer.name+" - "+tmpwks.workstationsite.sitename+" - "+tmpwks.workstationname + ": " + getAttribute(nodecheck,"description"));
                        }

                    }


                }

            }

            // Get list of hosts

            // Get list of workstations

            // Get list of failed checks
        }



        static void updateCache()
        {
            cacheinitialized = false;
            notico.Text = "Reloading Clients+Sites-List";
            arrayclients.Clear();
            arraysites.Clear();
            arrayservers.Clear();
            arrayworkstations.Clear();


            // Get list of clients
            XmlNodeList nlclientlist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_clients", "client");
            foreach (XmlNode nodeclient in nlclientlist)
            {
                string tmpclientid=getAttribute(nodeclient, "clientid");
                cacheKunde tmpKunde=new cacheKunde(getAttribute(nodeclient, "name"), tmpclientid);
                arrayclients.Add(tmpKunde);

                // Get recursive list of sites
                XmlNodeList nlsitelist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_sites&clientid=" + tmpclientid , "site");
                foreach (XmlNode nodesite in nlsitelist)
                {
                    string tmpsiteid=getAttribute(nodesite,"siteid");
                    cacheSite tmpSite = new cacheSite(getAttribute(nodesite, "name"), tmpsiteid, tmpKunde);
                    arraysites.Add(tmpSite);


                    // Get recursive list of servers
                    XmlNodeList nlserverslist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_servers&siteid=" + tmpsiteid, "server");
                    foreach (XmlNode nodeserver in nlserverslist)
                    {
                        string tmpserverid = getAttribute(nodeserver, "serverid");
                        arrayservers.Add(new cacheServer(getAttribute(nodeserver, "name"), tmpserverid, tmpSite));
                    }

                    // Get recursive list of workstations
                    XmlNodeList nlwkslist = getXmlNLFromHttp(System.Configuration.ConfigurationSettings.AppSettings["dbserver"] + "/api?apikey=" + ConfigurationSettings.AppSettings["apikey"] + "&service=list_workstations&siteid=" + tmpsiteid, "workstation");
                    foreach (XmlNode nodewks in nlwkslist)
                    {
                        string tmpwksid = getAttribute(nodewks, "workstationid");
                        arrayworkstations.Add(new cacheWorkstation(getAttribute(nodewks, "name"), tmpwksid, tmpSite));
                    }

                }
            }



            
            
            notico.Text = arrayclients.Count.ToString() + " clients, " + arraysites.Count.ToString() + " sites, " +arrayservers.Count.ToString() + " servers, "+arrayworkstations.Count.ToString()+" wks (Cache date: "+System.DateTime.Now.ToLongTimeString()+")";
            cacheinitialized = true;
        }

        static string getAttribute(XmlNode node, string attributename)
        {
            string retval="";
            foreach (XmlNode childnode in node.ChildNodes)
            {
                if (childnode.Name == attributename)
                {
                    retval = childnode.InnerXml;
                }
            }

            return retval;
        }

        static XmlNodeList getXmlNLFromHttp(string url, string tagname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            XmlNodeList nl = doc.GetElementsByTagName(tagname);
            return nl;
        }

        //==========================================================================
        private static void ExitClick (Object sender, EventArgs e)
        {
            notico.Dispose ();
            Application.Exit ();
        }

        //==========================================================================
        private static void ActionReloadClick (Object sender, EventArgs e)
        {
            updateCache();
        }

        //==========================================================================
        private static void NotifyIconDoubleClick (Object sender, EventArgs e)
        {
            // Was immer du willst
        }
    }

}
