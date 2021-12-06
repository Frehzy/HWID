using System;
using HWIDVerification;

namespace Verification
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Protect.Encrypt("92.248.231.140"));


            long ellapledTicks0 = DateTime.Now.Ticks;
            new CerialNumber();
            Console.WriteLine("Потрачено SYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks0));

            long ellapledTicks02 = DateTime.Now.Ticks;
            new CerialNumber();
            Console.WriteLine("Потрачено SYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks02));

            long ellapledTicks03 = DateTime.Now.Ticks;
            new CerialNumber();
            Console.WriteLine("Потрачено SYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks03));

            long ellapledTicks04 = DateTime.Now.Ticks;
            new CerialNumber();
            Console.WriteLine("Потрачено SYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks04));

            long ellapledTicks05 = DateTime.Now.Ticks;
            new CerialNumber();
            Console.WriteLine("Потрачено SYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks05));


            long ellapledTicks2 = DateTime.Now.Ticks;
            var q = new CerialNumberAsync();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks2));
            Console.WriteLine(q.HWIDHash);


            long ellapledTicks3 = DateTime.Now.Ticks;
            var q2 = new CerialNumberAsync();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks3));
            Console.WriteLine(q2.HWIDHash);


            long ellapledTicks4 = DateTime.Now.Ticks;
            var q3 = new CerialNumberAsync();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks4));
            Console.WriteLine(q3.HWIDHash);


            long ellapledTicks5 = DateTime.Now.Ticks;
            var q4 = new CerialNumberAsync();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks5));
            Console.WriteLine(q4.HWIDHash);


            long ellapledTicks6 = DateTime.Now.Ticks;
            var q5 = new CerialNumberAsync();
            Console.WriteLine("Потрачено ASYNC тактов на выполнение: " + (DateTime.Now.Ticks - ellapledTicks6));
            Console.WriteLine(q5.HWIDHash);

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
