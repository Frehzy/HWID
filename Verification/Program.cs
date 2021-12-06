using System;
using HWIDVerification;

namespace Verification
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Protect.Encrypt("92.248.231.140"));

            long ellapledTicks6 = DateTime.Now.Ticks;
            var q5 = new SerialNumber();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks6));
            Console.WriteLine(q5.HWIDHash);
            Console.WriteLine(q5.HWID);

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
        }
    }
}
