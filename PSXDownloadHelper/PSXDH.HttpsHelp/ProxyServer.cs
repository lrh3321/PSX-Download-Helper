using PSXDH.BLL;
using PSXDH.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PSXDH.HttpsHelp
{
    public partial class ProxyServer
    {
        public AppConfig Config { get; private set; }
        private IPAddress _address;

        public IPAddress Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public int Port
        {
            get
            {
                return Config.Port;
            }
        }

        public event UpdataUrlEventHandler UpdataUrl;

        public void OnUpdataUrlLog(UrlInfo urlinfo)
        {
            UrlInfo temp = DataHistoryOperate.GetInfo(urlinfo.PsnUrl);
            if (temp != null)
            {
                if (!string.IsNullOrEmpty(temp.ReplacePath) && !File.Exists(temp.ReplacePath))
                    temp.ReplacePath = "";
                urlinfo = temp;
            }

            this.UpdataUrl?.Invoke(this, new UpdataUrlEventArgs(urlinfo));
        }

        public bool IsRunning
        {
            get; private set;
        }

        public ExceptionHandler ExceptionHandler { get; set; }

        private HttpListenerHelp _listener;

        private readonly Hashtable _ctrls = new Hashtable();

        public ProxyServer(AppConfig config)
        {
            this.Config = config;
            // IPAddress.TryParse(Config.Ip, out IPAddress reval);
            _address = PickIPAddress(Config.Ip);
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IPAddress> GetHostIp()
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName());
            var iplist = ip.AddressList
                .Where(p => p.AddressFamily == AddressFamily.InterNetwork);
            return iplist;
        }

        public static IPAddress PickIPAddress(string iP = null)
        {
            IPAddress addr = null;
            IPAddress.TryParse(iP, out addr);
            if (addr != null)
            {
                addr = GetHostIp().FirstOrDefault(a => a.ToString() == iP);
            }
            return addr ?? GetHostIp().FirstOrDefault();

        }

        public void StartServer()
        {
            if (_listener == null)
            {
                if (!MonitorLog.BuildRules(this.Config.Rule))
                {
                    throw new NoCaptureRuleException();
                }

                if (this.Address == null)
                {
                    throw new IPAddressNotBindException();
                }

                _listener = new HttpListenerHelp(this.Address, this.Port, this.OnUpdataUrlLog)
                {
                    ExceptionHandler = this.ExceptionHandler,
                };
                _listener.Start();

                Config.Ip = this.Address.ToString();
                this.IsRunning = true;
            }
        }

        public void StopServer()
        {
            _listener.Dispose();
            _listener = null;
            this.IsRunning = false;
        }

        public ProxyServerInfo ServerInfo()
        {
            return new ProxyServerInfo()
            {
                Status = this.IsRunning ? 1 : 0,
                Host = this.Address?.ToString(),
                Port = this.Port,
                AvaliableAddresses = GetHostIp().Select(ip => ip.ToString()).ToArray(),
            };
        }
    }
}
