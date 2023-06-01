using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CPPMHelper
{
    internal class GlobalVPNHelper
    {
        private const string _binaryPath = @"C:\Program Files\SonicWall\Global VPN Client\SWGVC.exe";
        private const string _serviceName = "SWGVCSvc";
        private const string _wmiClassName = "Win32_Service";
        private const string _wmiQuery = $"{_wmiClassName}.Name='{_serviceName}'"; 

        internal void DisableClient()
        {
            StopClient();
            DisableService();
        }

        private void StopClient()
        {
            Process GlobalVPN = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = _binaryPath,
                    Arguments = "/Q"
                }
            };
            GlobalVPN.Start();
            GlobalVPN.WaitForExit();
        }
        private void DisableService()
        {
            using(ManagementObject obj = new ManagementObject(_wmiQuery))
            {
                //Stop the service
                obj.InvokeMethod("StopService", null);

                //Set State to Deactivated
                object[] parmeter = new object[] { "Disabled" };
                obj.InvokeMethod("ChangeStartMode", parmeter);
            }
        }
    }
}
