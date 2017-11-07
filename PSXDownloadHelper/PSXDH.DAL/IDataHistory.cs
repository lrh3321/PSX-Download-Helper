using System.Collections.Generic;
using PSXDH.Model;

namespace PSXDH.DAL
{
    public interface IDataHistory
    {
        bool AddLog(UrlInfo urlinfo);
        bool UpdataLog(UrlInfo urlinfo);
        UrlInfo GetInfo(string psnurl);
        List<UrlInfo> GetAllUrl();
        bool DelLog(UrlInfo urlinfo);
        void Save();
    }
}