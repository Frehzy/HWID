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
        private static string identifier (string wmiClass, string wmiProperty, string wmiMustBeTrue)
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
        private static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            ManagementClass mc = new ManagementClass(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
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
            return result;
        }

        private static string TryGetString(string wmiClass, string wmiProperty)
        {
            try 
            {
                string result = $"{wmiProperty}: " + identifier(wmiClass, wmiProperty);
                return result + "\n";
            }
            catch { return null; }
        }

        private static string CpuID()
        {
            string result = TryGetString("Win32_Processor", "ProcessorId");
            result += TryGetString("Win32_Processor", "Name");
            result += TryGetString("Win32_Processor", "Manufacturer");
            result += TryGetString("Win32_Processor", "MaxClockSpeed");
            return "CPU >> " + result;
        }
        private static string BiosID()
        {
            string result = TryGetString("Win32_BIOS", "SerialNumber");
            result += TryGetString("Win32_BIOS", "SMBIOSBIOSVersion");
            result += TryGetString("Win32_BIOS", "Manufacturer");
            result += TryGetString("Win32_BIOS", "ReleaseDate");
            result += TryGetString("Win32_BIOS", "Version");
            return "BIOS >> " + result;
        }
        private static string BaseBoardID()
        {
            string result = TryGetString("Win32_BaseBoard", "SerialNumber");
            result += TryGetString("Win32_BaseBoard", "Name");
            result += TryGetString("Win32_BaseBoard", "Manufacturer");
            return "BaseBoard >> " + result;
        }
        private static string DiskDriveID()
        {
            string result = TryGetString("Win32_DiskDrive", "SerialNumber");
            result += TryGetString("Win32_DiskDrive", "TotalHeads");
            result += TryGetString("Win32_DiskDrive", "Signature");
            result += TryGetString("Win32_DiskDrive", "Manufacturer");
            result += TryGetString("Win32_DiskDrive", "Model");
            return "DiskDrive >> " + result;
        }
        private static string VideoControllerID()
        {
            string result = TryGetString("Win32_VideoController", "PNPDeviceID");
            result += TryGetString("Win32_VideoController", "Name");
            result += TryGetString("Win32_VideoController", "AdapterRAM");
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
