using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace PSXDH.Model
{
    public class HttpHeaderCollection : NameValueCollection
    {
        #region Const Http Methods
        public const string HTTP_METHOD_GET = "GET";
        public const string HTTP_METHOD_POST = "POST";
        public const string HTTP_METHOD_HEAD = "HEAD";
        public const string HTTP_METHOD_PUT = "PUT";
        public const string HTTP_METHOD_DELETE = "DELETE";
        public const string HTTP_METHOD_OPTIONS = "OPTIONS";
        public const string HTTP_METHOD_TRACE = "TRACE";
        public const string HTTP_METHOD_CONNECT = "CONNECT";
        #endregion

        public const string HTTP_VERSION_1_1 = "HTTP/1.1";

        public string Method { get; set; }

        public string URL { get; set; }

        public string HttpVersion { get; set; }

        public static bool TryParse(string httpRequest, out HttpHeaderCollection headers)
        {
            headers = null;
            if (string.IsNullOrWhiteSpace(httpRequest))
            {
                return false;
            }
            headers = new HttpHeaderCollection();
            StringReader reader = new StringReader(httpRequest);
            string line = reader.ReadLine();

            var arr = line.Split(' ');
            if (arr.Length != 3)
            {
                return false;
            }
            headers.Method = arr[0].ToUpper();
            headers.URL = arr[1];
            headers.HttpVersion = arr[2];

            int index;
            string key, value;
            while (reader.Peek() > 0)
            {
                line = reader.ReadLine();
                index = line.IndexOf(':');
                if (index < 0)
                {
                    break;
                }
                key = line.Substring(0, index);
                value = line.Substring(index + 2);

                headers.Add(key, value);
            }
            return true;
        }
    }
}
