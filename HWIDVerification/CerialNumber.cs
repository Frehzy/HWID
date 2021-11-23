using System.Management;

namespace HWIDVerification
{
    public class CerialNumber
    {
        private class HWIDClass
        {
            public string HWID { get; set; }
            public string HWIDHash { get; set; }
        }

        public string HWID { get; private set; }
        public string HWIDHash { get; private set; }

        public CerialNumber()
        {
            HWIDClass HWIDc = Value();
            HWID = HWIDc.HWID;
            HWIDHash = HWIDc.HWIDHash;
        }

        private HWIDClass Value()
        {
            HWIDClass HWIDc = new HWIDClass();
            if (string.IsNullOrEmpty(string.Empty))
            {
                HWIDc.HWID = "CPU >> " + CPU_ID()
                    + "BIOS >> " + BIOS_ID()
                    + "BaseBoard >> " + BaseBoardID()
                    + "DiskDrive >> " + DiskDriveID()
                    + "VideoController >> " + VideoControllerID();
                    //+ "\nMAC >> " + MAC_ID();

                HWIDc.HWIDHash = HashClass.GetHash(HWIDc.HWID);
            }
            return HWIDc;
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

        private static string CPU_ID()
        {
            string result = "";
            result += TryGetString("Win32_Processor", "ProcessorId");
            result += TryGetString("Win32_Processor", "Name");
            result += TryGetString("Win32_Processor", "Manufacturer");
            result += TryGetString("Win32_Processor", "MaxClockSpeed");
            return result;
        }
        private static string BIOS_ID()
        {
            string result = "";
            result += TryGetString("Win32_BIOS", "SerialNumber");
            result += TryGetString("Win32_BIOS", "SMBIOSBIOSVersion");
            result += TryGetString("Win32_BIOS", "Manufacturer");
            result += TryGetString("Win32_BIOS", "ReleaseDate");
            result += TryGetString("Win32_BIOS", "Version");
            return result;
        }
        private static string BaseBoardID()
        {
            string result = "";
            result += TryGetString("Win32_BaseBoard", "SerialNumber");
            result += TryGetString("Win32_BaseBoard", "Name");
            result += TryGetString("Win32_BaseBoard", "Manufacturer");
            return result;
        }
        private static string DiskDriveID()
        {
            string result = "";
            result += TryGetString("Win32_DiskDrive", "SerialNumber");
            result += TryGetString("Win32_DiskDrive", "TotalHeads");
            result += TryGetString("Win32_DiskDrive", "Signature");
            result += TryGetString("Win32_DiskDrive", "Manufacturer");
            result += TryGetString("Win32_DiskDrive", "Model");
            return result;
        }
        private static string VideoControllerID()
        {
            string result = "";
            result += TryGetString("Win32_VideoController", "PNPDeviceID");
            result += TryGetString("Win32_VideoController", "Name");
            result += TryGetString("Win32_VideoController", "AdapterRAM");
            return result;
        }
        private static string MAC_ID()
        {
            try { return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled"); }
            catch { return null; }
        }
        #endregion
    }
}
