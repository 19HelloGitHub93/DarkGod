using System;
using System.Configuration;

namespace Server.Common
{
    public class Tools
    {
        public static string getGuidePath()
        {
            string path="";
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    path = ConfigurationManager.AppSettings.Get("MacPath");
                    break;
                case PlatformID.Win32NT:
                    path = ConfigurationManager.AppSettings.Get("WinPath");
                    break;
            }

            return path;
        }
    }
}