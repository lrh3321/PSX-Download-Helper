using System;
using System.Net;
using System.Net.Sockets;
using PSXDH.ProxyHelp;

namespace PSXDH.HttpsHelp
{
    public sealed class HttpListenerHelp : Listener
    {
        public UpdataUrlLog UpdataUrlLog;

        public ExceptionHandler ExceptionHandler { get; set; }

        public HttpListenerHelp(int port)
            : this(IPAddress.Any, port)
        {
        }

        public HttpListenerHelp(IPAddress address, int port)
            : base(port, address)
        {
        }

        public HttpListenerHelp(IPAddress address, int port, UpdataUrlLog updataurlLog)
            : base(port, address)
        {
            UpdataUrlLog = updataurlLog;
        }

        public override string ConstructString
        {
            get { return ("Host:" + Address + ";Port:" + Port); }
        }

        public override void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = ListenSocket.EndAccept(ar);
                if (clientSocket != null)
                {
                    var client = new HttpClient(clientSocket, RemoveClient, UpdataUrlLog);
                    AddClient(client);
                    client.StartHandshake();
                }
            }
            catch (Exception ex)
            {
                this.ExceptionHandler?.Invoke(ex);
            }
            try
            {
                ListenSocket.BeginAccept(OnAccept, ListenSocket);
            }
            catch (Exception ex)
            {
                this.ExceptionHandler?.Invoke(ex);
                Dispose();
            }
        }

        public override string ToString()
        {
            return ("HTTP service on " + Address + ":" + Port);
        }
    }
}