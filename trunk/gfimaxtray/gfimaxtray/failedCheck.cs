using System;
using System.Collections.Generic;
using System.Text;

namespace gfimaxtray
{
    class failedCheck
    {
        private string clientName = "";
        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        private string siteName = "";
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string deviceName = "";
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        private bool isOffline = false;
        public bool IsOffline
        {
            get { return isOffline; }
            set { isOffline = value; }
        }

        private bool isWorkstation = false;
        public bool IsWorkstation 
        {
            get { return isWorkstation; }
            set { isWorkstation = value; }
        }

        private string checkName = "";
        public string CheckName
        {
            get { return checkName; }
            set { checkName = value; }
        }

        private string checkOutput = "";
        public string CheckOutput
        {
            get { return checkOutput; }
            set { checkOutput = value; }
        }

        private DateTime failedSince = DateTime.MinValue;
        public DateTime FailedSince
        {
            get { return failedSince; }
            set { failedSince = value; }
        }

        public failedCheck(string _clientName, string _siteName, string _deviceName, bool _isWorkstation, bool _isOffline, string _checkName, string _checkOutput, DateTime _failedSince)
        {
            clientName = _clientName;
            siteName = _siteName;
            deviceName = _deviceName;
            isOffline = _isOffline;
            checkName = _checkName;
            checkOutput = _checkOutput;
            failedSince = _failedSince;
        }
        

    }
}
