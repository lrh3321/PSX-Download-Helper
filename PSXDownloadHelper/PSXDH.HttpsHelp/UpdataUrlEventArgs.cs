using PSXDH.Model;
using System;

namespace PSXDH.HttpsHelp
{
    public class UpdataUrlEventArgs : EventArgs
    {
        public UrlInfo Info { get; set; }

        public bool Handled { get; set; }

        public UpdataUrlEventArgs() : this(null)
        {
        }

        public UpdataUrlEventArgs(UrlInfo info) : base()
        {
            this.Info = info;
            this.Handled = false;
        }
    }

    public delegate void UpdataUrlEventHandler(object sender, UpdataUrlEventArgs e);
}
