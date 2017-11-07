using PSXDH.DAL;
using PSXDH.Model;
using System;
using Xunit;
using Xunit.Abstractions;

namespace PSXDH.UnitTest
{
    [Collection("Data SQL")]
    public class PowerDataHistoryTest
    {
        private readonly ITestOutputHelper m_output;

        public PowerDataHistoryTest(ITestOutputHelper output)
        {
            this.m_output = output;
            PowerDataHistory.Current.ClearAll();
        }

        [Theory]
        [InlineData("https://www.baidu.com/")]
        [InlineData("https://www.baidu.com")]
        [InlineData("http://www.baidu.com")]
        [InlineData("http://www.baidu.com/kw=123")]
        public void TestAddLog(string psnurl)
        {
            int logCount = PowerDataHistory.Current.LogCount;
            var urlInfo = new UrlInfo() { PsnUrl = psnurl };
            PowerDataHistory.Current.AddLog(urlInfo);

            Assert.NotEqual(PowerDataHistory.Current.LogCount, logCount);

            var otherUrlInfo = PowerDataHistory.Current.GetInfo(psnurl);
            Assert.NotNull(otherUrlInfo);
            Assert.Equal(otherUrlInfo.PsnUrl, psnurl);
        }

        [Fact]
        public void TestGetAllUrl()
        {
            PowerDataHistory.Current.GetAllUrl();
        }

        [Fact]
        public void TestSave()
        {
            var obj = PowerDataHistory.Current;
            Assert.NotNull(obj);
            try
            {
                obj.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
