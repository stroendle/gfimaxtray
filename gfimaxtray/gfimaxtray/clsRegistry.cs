using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text;

namespace gfimaxtray
{
    class clsRegistry
    {

        public static string getRegistryValue(string RegKey)
        {
            string value="";

            if (SubKeyExist("SOFTWARE\\gfimaxtray\\AppSettings"))
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\gfimaxtray\\AppSettings");
                value = (string)myKey.GetValue(RegKey);
            }
            else
            {


            }

            return value;
        }

        public static void setRegistryValue(string RegKey, string Value)
        {
            RegistryKey newKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\gfimaxtray\\AppSettings");

            // Write Values to the Subkey
            newKey.SetValue(RegKey, Value);
        }
	
        private static bool SubKeyExist(string Subkey)
        {
	        // Check if a Subkey exist
	        RegistryKey myKey = Registry.CurrentUser.OpenSubKey(Subkey);
	        if (myKey == null)
		        return false;
	        else
		        return true;
        }

    }
}
