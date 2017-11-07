using System.Diagnostics;
using System.Xml.Serialization;

namespace PSXDH.Model
{
    [DebuggerDisplay("Remote={PsnUrl}, Host={Host}")]
    [XmlType("PsnRecord")]
    public class UrlInfo
    {
        public UrlInfo() : this(string.Empty, string.Empty, string.Empty, false, string.Empty)
        {
        }

        public UrlInfo(string psnurl, string replacepath, string marktxt, bool isLixian = false, string lixianurl = null)
        {
            SetLixian = false;
            PsnUrl = psnurl;
            ReplacePath = replacepath;
            MarkTxt = marktxt;
            LixianUrl = lixianurl;
            IsLixian = isLixian;
            ThroughHttps = false;
        }

        /// <summary>
        ///     PSN连接
        /// </summary>
        [XmlAttribute("PsnUrl")]
        public string PsnUrl { get; set; }

        /// <summary>
        ///     替换地址
        /// </summary>
        [XmlAttribute("LocalUrl")]
        public string ReplacePath { get; set; }

        /// <summary>
        ///     当前连接备注信息
        /// </summary>
        [XmlAttribute("Names")]
        public string MarkTxt { get; set; }

        /// <summary>
        ///     离线地址
        /// </summary>
        [XmlAttribute("LixianUrl")]
        public string LixianUrl { get; set; }

        /// <summary>
        ///     是否是线
        /// </summary>
        [XmlAttribute("isLixian")]
        public bool IsLixian { get; set; }

        /// <summary>
        ///     增加为离线
        /// </summary>
        public bool SetLixian { get; set; }

        /// <summary>
        /// 是否为CDN地址
        /// </summary>
        public bool IsCdn { get; set; }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 指示请求是否为Https协议，true表示请求为Https协议。
        /// </summary>
        public bool ThroughHttps { get; set; }
    }
}