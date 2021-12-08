using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HWIDVerification
{
    public class TestClass
    {
        private class wmiClass
        {
            public string WMI { get; set; }
            public string Property { get; set; }

            public wmiClass(string WMI, string Property)
            {
                this.WMI = WMI;
                this.Property = Property;
            }
        }

        public string HWID { get; private set; }
        public string HWIDHash { get; private set; }

        public TestClass()
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
        private static Dictionary<string, string> identifier(List<wmiClass> list)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>(); //property, result
            foreach (var item in list)
                dic.Add(item.Property, null);

            ManagementClass mc = new ManagementClass(list.First().WMI);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                try
                {
                    Parallel.ForEach(list, (value, state) =>
                    {
                        dic[value.Property] = mo[value.Property].ToString();
                    });
                    break;
                }
                catch
                { }
            }
            return dic;
        }

        private static string GetString(Dictionary<string, string> dic)
        {
            string result = "";
            for (int i = 0; i < dic.Count; i++)
                result += $"{dic.ElementAt(i).Key}: " + dic.ElementAt(i).Value + "\n";
            return result;
        }

        private static string CpuID()
        {
            List<wmiClass> wmiPropertyList = new List<wmiClass>()
            { 
                new wmiClass("Win32_Processor", "ProcessorId"),
                new wmiClass("Win32_Processor", "Name"),
                new wmiClass("Win32_Processor", "Manufacturer"),
                new wmiClass("Win32_Processor", "MaxClockSpeed")
            };

            var dic = identifier(wmiPropertyList);
            return "CPU >> " + GetString(dic);
        }
        private static string BiosID()
        {
            List<wmiClass> wmiPropertyList = new List<wmiClass>()
            {
                new wmiClass("Win32_BIOS", "SerialNumber"),
                new wmiClass("Win32_BIOS", "SMBIOSBIOSVersion"),
                new wmiClass("Win32_BIOS", "Manufacturer"),
                new wmiClass("Win32_BIOS", "ReleaseDate"),
                new wmiClass("Win32_BIOS", "Version")
            };

            var dic = identifier(wmiPropertyList);
            return "BIOS >> " + GetString(dic);
        }
        private static string BaseBoardID()
        {
            List<wmiClass> wmiPropertyList = new List<wmiClass>()
            {
                new wmiClass("Win32_BaseBoard", "SerialNumber"),
                new wmiClass("Win32_BaseBoard", "Name"),
                new wmiClass("Win32_BaseBoard", "Manufacturer")
            };

            var dic = identifier(wmiPropertyList);
            return "BaseBoard >> " + GetString(dic);
        }
        private static string DiskDriveID()
        {
            List<wmiClass> wmiPropertyList = new List<wmiClass>()
            {
                new wmiClass("Win32_DiskDrive", "SerialNumber"),
                new wmiClass("Win32_DiskDrive", "TotalHeads"),
                new wmiClass("Win32_DiskDrive", "Signature"),
                new wmiClass("Win32_DiskDrive", "Manufacturer"),
                new wmiClass("Win32_DiskDrive", "Model")
            };

            var dic = identifier(wmiPropertyList);
            return "DiskDrive >> " + GetString(dic);
        }
        private static string VideoControllerID()
        {
            List<wmiClass> wmiPropertyList = new List<wmiClass>()
            {
                new wmiClass("Win32_VideoController", "PNPDeviceID"),
                new wmiClass("Win32_VideoController", "Name"),
                new wmiClass("Win32_VideoController", "AdapterRAM")
            };

            var dic = identifier(wmiPropertyList);
            return "VideoController >> " + GetString(dic);
        }
        private static string MAC_ID()
        {
            try { return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled"); }
            catch { return null; }
        }
        #endregion
    }
}
