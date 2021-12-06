using System.Collections.Generic;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace HWIDVerification
{
    public class CerialNumberAsync
    {
        private static class HWIDClass
        {
            public static string CPU { get; set; }
            public static string BIOS { get; set; }
            public static string BaseBoard { get; set; }
            public static string DiskDrive { get; set; }
            public static string VideoController { get; set; }
        }

        public string HWID { get; private set; }
        public string HWIDHash { get; private set; }

        public CerialNumberAsync()
        {
            Value();
        }

        private void Value()
        {
            if (string.IsNullOrEmpty(string.Empty))
            {
                Task task1 = Task.Run(CPU_ID);
                Task task2 = Task.Run(BIOS_ID);
                Task task3 = Task.Run(BaseBoardID);
                Task task4 = Task.Run(DiskDriveID);
                Task task5 = Task.Run(VideoControllerID);
                Task.WaitAll(task1, task2, task3, task4, task5);

                HWID = HWIDClass.CPU + HWIDClass.BIOS + HWIDClass.BaseBoard + HWIDClass.DiskDrive + HWIDClass.VideoController;

                HWIDHash = HashClass.GetHash(HWID);
            }
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

        private static void CPU_ID()
        {
            string result = TryGetString("Win32_Processor", "ProcessorId");
            result += TryGetString("Win32_Processor", "Name");
            result += TryGetString("Win32_Processor", "Manufacturer");
            result += TryGetString("Win32_Processor", "MaxClockSpeed");
            HWIDClass.CPU = "CPU >> " + result;
        }
        private static void BIOS_ID()
        {
            string result = TryGetString("Win32_BIOS", "SerialNumber");
            result += TryGetString("Win32_BIOS", "SMBIOSBIOSVersion");
            result += TryGetString("Win32_BIOS", "Manufacturer");
            result += TryGetString("Win32_BIOS", "ReleaseDate");
            result += TryGetString("Win32_BIOS", "Version");
            HWIDClass.BIOS = "BIOS >> " + result;
        }
        private static void BaseBoardID()
        {
            string result = TryGetString("Win32_BaseBoard", "SerialNumber");
            result += TryGetString("Win32_BaseBoard", "Name");
            result += TryGetString("Win32_BaseBoard", "Manufacturer");
            HWIDClass.BaseBoard = "BaseBoard >> " + result;
        }
        private static void DiskDriveID()
        {
            string result = TryGetString("Win32_DiskDrive", "SerialNumber");
            result += TryGetString("Win32_DiskDrive", "TotalHeads");
            result += TryGetString("Win32_DiskDrive", "Signature");
            result += TryGetString("Win32_DiskDrive", "Manufacturer");
            result += TryGetString("Win32_DiskDrive", "Model");
            HWIDClass.DiskDrive = "DiskDrive >> " + result;
        }
        private static void VideoControllerID()
        {
            string result = TryGetString("Win32_VideoController", "PNPDeviceID");
            result += TryGetString("Win32_VideoController", "Name");
            result += TryGetString("Win32_VideoController", "AdapterRAM");
            HWIDClass.VideoController = "VideoController >> " + result;
        }
        private static string MAC_ID()
        {
            try { return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled"); }
            catch { return null; }
        }
        #endregion
    }
}
