using System;
using System.Collections.Generic;
using System.Text;
using PSXDH.Model;
using Xunit;

namespace PSXDH.UnitTest
{
    [Collection("Parser")]
    public class HttpHeaderCollectionTest
    {
        [Fact]
        [Trait("Class", "HttpHeaderCollection")]
        public void TestHttpsProxy()
        {
            var requestBody = string.Join("\r\n",
                new[]
                {
                    "CONNECT adservice.google.com:443 HTTP/1.1",
                    "Host: adservice.google.com:443",
                    "Proxy-Connection: keep-alive",
                    "User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 QIHU 360EE",
                    "",
                });

            if (HttpHeaderCollection.TryParse(requestBody, out var headers))
            {
                Assert.Equal(HttpHeaderCollection.HTTP_METHOD_CONNECT, headers.Method);
                Assert.Equal("adservice.google.com:443", headers.URL);
                Assert.Equal(HttpHeaderCollection.HTTP_VERSION_1_1, headers.HttpVersion);

                Assert.Equal("adservice.google.com:443", headers.Get("Host"));
                Assert.Equal(3, headers.Count);
            }
            else
            {
                Assert.True(false, "Parse Failed");
            }
        }

        [Theory]
        [InlineData("http://www.cnblogs.com/deerchao/archive/2007/08/09/849361.html")]
        [InlineData("http://www.cnblogs.com/jerehedu/p/7800273.html")]
        [InlineData("http://www.cnblogs.com/%E9%98%BF%E9%87%8C/")]
        [Trait("Class", "HttpHeaderCollection")]
        public void TestHttpGet(string url)
        {
            var requestBody = string.Join("\r\n",
                new[]
                {
                    $"GET {url} HTTP/1.1",
                    "Host: www.cnblogs.com",
                    "Proxy-Connection: keep-alive",
                    "Cache-Control: max-age=0",
                    "Upgrade-Insecure-Requests: 1",
                    "User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                    "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                    "Accept-Encoding: gzip, deflate, sdch",
                    "Accept-Language: zh-CN,zh;q=0.8,ja;q=0.6,zh-TW;q=0.4,en;q=0.2",
                    "Cookie: __gads=ID=8b40342391f10a0f:T=1509847429:S=ALNI_MaQoh7nMk63P0VQaToTRcfgP95E1w; _ga=GA1.2.221866378.1509758983; _gid=GA1.2.1801548824.1509965208",
                    "",
                });

            if (HttpHeaderCollection.TryParse(requestBody, out var headers))
            {
                Assert.Equal(HttpHeaderCollection.HTTP_METHOD_GET, headers.Method);
                Assert.Equal(url, headers.URL);
                Assert.Equal(HttpHeaderCollection.HTTP_VERSION_1_1, headers.HttpVersion);

                Assert.Equal("www.cnblogs.com", headers.Get("Host"));
                Assert.Equal(9, headers.Count);
            }
            else
            {
                Assert.True(false, "Parse Failed");
            }

        }
    }
}
