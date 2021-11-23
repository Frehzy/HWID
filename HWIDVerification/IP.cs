using Newtonsoft.Json;
using System;
using System.Device.Location;
using System.Net;
using System.Net.Sockets;

namespace HWIDVerification
{
    public class IP
    {
        public class IPInfo
        {
            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }

        public string LocalIP { get; private set; }
        public string PublicIP { get; private set; }
        public IPInfo Info { get; private set; }
        public double[] Location { get; private set; }
        public string IPInfoString { get; private set; }
        public string IPInfoHash { get; private set; }

        public IP()
        {
            LocalIP = GetLocalIPAddress();
            PublicIP = GetPublicIP();
        }
        public IP(string ip)
        {
            LocalIP = GetLocalIPAddress();
            PublicIP = ip;
            if (PublicIP != null)
            {
                Info = GetInfo(PublicIP);
                if (Info != null)
                {
                    Location = ConvertLoc(Info.Loc);

                    IPInfoString = "City >> " + Info.City +
                        "\nCountry >> " + Info.Country +
                        "\nHostname >> " + Info.Hostname +
                        "\nIP >> " + Info.Ip +
                        "\nRegion >> " + Info.Region +
                        "\nPostal >> " + Info.Postal;

                    IPInfoHash = HashClass.GetHash(IPInfoString);
                }
            }
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return null;
        }

        private string GetPublicIP()
        {
            try { return new WebClient().DownloadString("https://api.ipify.org"); }
            catch { return null; }
        }

        private IPInfo GetInfo(string ip)
        {
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                return JsonConvert.DeserializeObject<IPInfo>(info);
            }
            catch
            {
                return null;
            }
        }

        private double[] ConvertLoc(string Loc)
        {
            double X = Convert.ToDouble(Loc.Split(',')[0].Replace('.', ','));
            double Y = Convert.ToDouble(Loc.Split(',')[1].Replace('.', ','));
            return new double[2] { X, Y };
        }

        public double Distance(double X1, double Y1, double X2, double Y2)
        {
            GeoCoordinate c1 = new GeoCoordinate(X1, Y1);
            GeoCoordinate c2 = new GeoCoordinate(X2, Y2);

            return c1.GetDistanceTo(c2) / 1000;
        }

        public double[] GetPublicIPTOLocation(string IP)
        {
            var info = GetInfo(IP);
            return ConvertLoc(info.Loc);
        }
    }
}
