namespace PSXDH.HttpsHelp
{
    public class ProxyServerInfo
    {
        public int Status { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public int ClientCount { get; set; }

        public string[] AvaliableAddresses { get; set; }
    }

}
