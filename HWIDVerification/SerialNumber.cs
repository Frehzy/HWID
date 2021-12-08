using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace HWIDVerification
{
    public class SerialNumber
    {
        public string HWID { get; private set; }
        public string HWIDHash { get; private set; }

        public SerialNumber()
        {
            HWID = GetHWID();
            HWIDHash = HashClass.GetHash(HWID);
        }

        private string GetHWID()
        {
            Func<string>[] methods = new Func<string>[] { CpuID, BiosID, BaseBoardID, DiskDriveID, VideoControllerID };
            return string.Concat(Task.WhenAll(methods.Select(Task.Run)).Result);
        }

        #region Original Device ID Getting Code
        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            ManagementObjectCollection moc = new ManagementClass(wmiClass).GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }
        private static string identifier(string wmiClass, List<string> wmiPropertyList)
        {
            string result = "";
            ManagementClass mc = new ManagementClass(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                try
                {
                    foreach (string wmiProperty in wmiPropertyList)
                        result += $"{wmiProperty}: " + mo[wmiProperty].ToString() + "\n";
                    break;
                }
                catch
                { }
            }
            return result;
        }

        private static string CpuID()
        {
            List<string> wmiPropertyList = new List<string>()
            {
                "ProcessorId",
                "Name",
                "Manufacturer",
                "MaxClockSpeed"
            };
            string result = identifier("Win32_Processor", wmiPropertyList);
            return "CPU >> " + result;
        }
        private static string BiosID()
        {
            List<string> wmiPropertyList = new List<string>()
            {
                "SerialNumber",
                "SMBIOSBIOSVersion",
                "Manufacturer",
                "ReleaseDate",
                "Version"
            };
            string result = identifier("Win32_BIOS", wmiPropertyList);
            return "BIOS >> " + result;
        }
        private static string BaseBoardID()
        {
            List<string> wmiPropertyList = new List<string>()
            {
                "SerialNumber",
                "Name",
                "Manufacturer"
            };
            string result = identifier("Win32_BaseBoard", wmiPropertyList);
            return "BaseBoard >> " + result;
        }
        private static string DiskDriveID()
        {
            List<string> wmiPropertyList = new List<string>()
            {
                "SerialNumber",
                "TotalHeads",
                "Signature",
                "Manufacturer",
                "Model"
            };
            string result = identifier("Win32_DiskDrive", wmiPropertyList);
            return "DiskDrive >> " + result;
        }
        private static string VideoControllerID()
        {
            List<string> wmiPropertyList = new List<string>()
            {
                "PNPDeviceID",
                "Name",
                "AdapterRAM"
            };
            string result = identifier("Win32_VideoController", wmiPropertyList);
            return "VideoController >> " + result;
        }
        private static string MAC_ID()
        {
            try { return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled"); }
            catch { return null; }
        }
        #endregion
    }
}
