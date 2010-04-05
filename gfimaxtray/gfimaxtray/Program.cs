using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Threading;


namespace gfimaxtray
{
    static class Program
    {
        private static NotifyIcon  notico;

        private static ArrayList arrayclients = new ArrayList();
        private static ArrayList arraysites = new ArrayList();
        private static ArrayList arrayservers = new ArrayList();
        private static ArrayList arrayworkstations = new ArrayList();
        private static ArrayList arrayFailedChecks = new ArrayList();

        
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
            notico.Icon = new Icon("icons/maxtrayicon_gray.ico"); // Eigenes Icon einsetzen
            notico.Text = "Doppelklick mich!";   // Eigenen Text einsetzen
            notico.Visible = true;
            notico.ContextMenu = cm;
            notico.DoubleClick += new EventHandler (NotifyIconDoubleClick);

            System.Windows.Forms.Timer timerRefresh = new System.Windows.Forms.Timer();
            timerRefresh.Enabled=false;
            timerRefresh.Interval = 5000;
            timerRefresh.Tick += new EventHandler(timerRefresh_Tick);


            if (clsRegistry.getRegistryValue("dbserver") == "")
            {
                frmKonfiguration tmpfrmkonfiguration = new frmKonfiguration();
                tmpfrmkonfiguration.ShowDialog();
            }

            //Cache aktualisieren
       //     updateCache();
            updateFailedClients();

            clsNotifyBox notifyBox = new clsNotifyBox();
            Thread notifyBoxThread = new Thread(new ParameterizedThreadStart(notifyBox.ShowBox));
            notifyBoxThread.Start((object)"Daten eingelesen !");


            // Ohne Appplication.Run geht es nicht
            Application.Run ();
        }

        static void timerRefresh_Tick(object sender, EventArgs e)
        {
            updateFailedClients();
        }


        static void updateFailedClients()
        {
            arrayFailedChecks.Clear();
            apiCommunication.GetFailedClients(ref arrayFailedChecks);
            int countFailedChecks = 0;
            int countDevicesOffline=0;
            string oldID="";
            foreach (failedCheck tmpCheck in arrayFailedChecks)
            {
                string newID=tmpCheck.ClientName+tmpCheck.SiteName+tmpCheck.DeviceName;
                if (oldID!=newID && tmpCheck.IsOffline==true)
                {
                    countDevicesOffline++;
                }

                if (tmpCheck.IsOffline == false)
                {
                    countFailedChecks++;
                }
                oldID=tmpCheck.ClientName+tmpCheck.SiteName+tmpCheck.DeviceName;
            }

            if (countFailedChecks > 0)
            {
                notico.Icon = new Icon("icons/maxtrayicon_red.ico");
            }
            else
            {
                notico.Icon = new Icon("icons/maxtrayicon_green.ico");
            }
            notico.Text = countFailedChecks.ToString() + " failed checks, " + countDevicesOffline.ToString() + " devices offline";
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
            updateFailedClients();
        }

        //==========================================================================
        private static void NotifyIconDoubleClick (Object sender, EventArgs e)
        {
            // frmCheckResults anzeigen
            frmCheckresults tmpfrm = new frmCheckresults();
            tmpfrm.init(arrayFailedChecks);
            tmpfrm.Show();
        }
    }

}
