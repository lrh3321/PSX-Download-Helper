using PSXDH.Model;
using System;
using System.Data;

namespace PSXDH.DAL
{
    public static class DataStoreHelper
    {

        public static UrlInfo ToUrlInfo(this DataRow row)
        {
            return new UrlInfo(
                (string)row["RemoteUrl"],
                (string)row["LocalUrl"],
                (string)row["Names"],
                (bool)row["IsLixian"],
                (string)row["LixianUrl"]);
        }

        public static void CopyToDataRow(this UrlInfo urlinfo, DataRow row)
        {
            lock (row)
            {
                row.BeginEdit();

                row["Names"] = urlinfo.MarkTxt;
                row["RemoteUrl"] = urlinfo.PsnUrl;
                row["LocalUrl"] = urlinfo.ReplacePath;
                row["LixianUrl"] = urlinfo.LixianUrl;
                row["IsLixian"] = urlinfo.IsLixian;
                row["UpdateDateTime"] = DateTime.Now;

                row.EndEdit();
            }
        }


        public static void CopyToAccessDataRow(this UrlInfo urlinfo, DataRow row)
        {
            lock (row)
            {
                row.BeginEdit();
                
                row["RemoteUrl"] = urlinfo.PsnUrl;
                row["UpdateDateTime"] = DateTime.Now;

                row.EndEdit();
            }
        }

    }
}
