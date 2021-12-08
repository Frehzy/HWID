using System;
using System.Collections.Generic;
using System.Linq;
using HWIDVerification;

namespace Verification
{
    class Program
    {
        static List<long> defaultList = new List<long>();
        static List<long> testList = new List<long>();
        static void Main(string[] args)
        {
            DefaultMethod(false);
            TestMethod(false);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Default: " + DefaultMethod());
                Console.WriteLine("Test: " + TestMethod());
                Console.WriteLine("Test: " + TestMethod());
                Console.WriteLine("Default: " + DefaultMethod());
            }
            Console.WriteLine("Среднее значение Default: " + defaultList.Sum() / defaultList.Count);
            Console.WriteLine("Среднее значение Test: " + testList.Sum() / testList.Count);

            /*
            Console.WriteLine(Protect.Encrypt("92.248.231.140"));
            
            IP onlyIP = new IP();
            IP infoIP = new IP(onlyIP.PublicIP);
            Console.WriteLine(infoIP.LocalIP);
            Console.WriteLine($"{infoIP.Location[0]}/{infoIP.Location[1]}");
            Console.WriteLine(infoIP.PublicIP + "\n");
            
            Console.WriteLine(infoIP.Info.Hostname);
            Console.WriteLine(infoIP.Info.Ip);
            Console.WriteLine(infoIP.Info.Loc);
            Console.WriteLine(infoIP.Info.Org);
            Console.WriteLine(infoIP.Info.Postal);
            Console.WriteLine(infoIP.Info.Region);
            Console.WriteLine(infoIP.IPInfoHash);
            */
        }

        private static long DefaultMethod(bool AddToList = true, bool IsWriteOnConsole = false)
        {
            long ellapledTicks = DateTime.Now.Ticks;
            var temp = new SerialNumber();
            if (IsWriteOnConsole)
            {
                Console.WriteLine(temp.HWID);
                Console.WriteLine(temp.HWIDHash);
            }
            long time = DateTime.Now.Ticks - ellapledTicks;
            if (AddToList)
                defaultList.Add(time);
            return time;
        }
        private static long TestMethod(bool AddToList = true, bool IsWriteOnConsole = false)
        {
            long ellapledTicks = DateTime.Now.Ticks;
            var temp = new TestClass();
            if (IsWriteOnConsole)
            {
                Console.WriteLine(temp.HWID);
                Console.WriteLine(temp.HWIDHash);
            }
            long time = DateTime.Now.Ticks - ellapledTicks;
            if (AddToList)
                testList.Add(time);
            return time;
        }
    }
}
