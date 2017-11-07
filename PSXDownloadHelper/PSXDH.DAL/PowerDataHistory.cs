using PSXDH.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace PSXDH.DAL
{

    public partial class PowerDataHistory : IDataHistory
    {
        private static readonly string XmlPath
            = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"DataFiles\PowerDataHistory.xml");

        private DataSet dataSet;

        private DataTable historyTable;

        private DataTable accessTable;

        private static PowerDataHistory _current;

        public static PowerDataHistory Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        public bool FuzzyMatching { get; set; }

        public int LogCount { get => this.historyTable.Rows.Count; }

        public int MaxLogCount { get; set; }

        public string SavePath { get; set; }

        static PowerDataHistory()
        {
            _current = new PowerDataHistory(XmlPath);
            ReloadCurrent();
        }

        #region Static Methods

        private static void _InsertHistoryColumns(DataTable table)
        {
            table.Columns
                .AddRange(
                new DataColumn[] {
                    new DataColumn("Names", typeof(string), null, MappingType.Attribute),
                    new DataColumn("RemoteUrl", typeof(string), null, MappingType.Attribute),
                    new DataColumn("LocalUrl", typeof(string), null, MappingType.Attribute),
                    new DataColumn("LixianUrl", typeof(string), null, MappingType.Attribute),
                    new DataColumn("IsLixian", typeof(bool), null, MappingType.Attribute),
                    new DataColumn("UpdateDateTime", typeof(DateTime), null, MappingType.Attribute),
                });
        }

        private static void _InsertHttpsAccessColumns(DataTable table)
        {
            table.Columns
                .AddRange(
                new DataColumn[] {
                    new DataColumn("RemoteUrl", typeof(string), null, MappingType.Attribute),
                    new DataColumn("UpdateDateTime", typeof(DateTime), null, MappingType.Attribute),
                });
        }

        public static void ReloadCurrent()
        {
            if (File.Exists(_current.SavePath))
            {
                try
                {
                    _current.dataSet.ReadXml(_current.SavePath);
                    _current.historyTable = _current.dataSet.Tables["DataHistory"];
                    _current.accessTable = _current.dataSet.Tables["HA"];
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
        #endregion

        private PowerDataHistory() : this(string.Empty)
        {
        }

        public PowerDataHistory(string savepath)
        {
            this.SavePath = savepath;
            this.historyTable = new DataTable
            {
                TableName = "DataHistory",
                CaseSensitive = false,
            };

            this.accessTable = new DataTable
            {
                TableName = "HA",
                CaseSensitive = false,
            };

            this.dataSet = new DataSet("AccessHistory");
            this.dataSet.Tables.Add(this.historyTable);
            this.dataSet.Tables.Add(this.accessTable);

            _InsertHistoryColumns(historyTable);
            _InsertHttpsAccessColumns(accessTable);
        }

        private DataRow UrlInfoToDataRow(UrlInfo urlinfo)
        {
            var row = this.historyTable.NewRow();
            urlinfo.CopyToDataRow(row);
            return row;
        }

        private DataRow UrlInfoToAccessDataRow(UrlInfo urlinfo)
        {
            var row = this.accessTable.NewRow();
            urlinfo.CopyToAccessDataRow(row);
            return row;
        }

        public bool AddLog(UrlInfo urlinfo)
        {
            if (!this.UpdataLog(urlinfo))
            {
                if (urlinfo.PsnUrl.IndexOf('/') >= 0)
                {
                    var row = this.UrlInfoToDataRow(urlinfo);
                    this.historyTable.Rows.Add(row);
                }
                else
                {
                    var row = this.UrlInfoToAccessDataRow(urlinfo);
                    this.accessTable.Rows.Add(row);
                }

                return true;
            }
            return false;
        }

        public bool DelLog(UrlInfo urlinfo)
        {
            var rows = this.historyTable?.Select($"RemoteUrl='{urlinfo.PsnUrl}'");
            if (rows != null && rows.Length > 0)
            {
                this.historyTable.Rows.Remove(rows[0]);
            }
            return true;
        }

        public IEnumerable<DataRow> EnumerateRows()
        {
            return this.historyTable.Select(null, "UpdateDateTime desc");
        }

        public List<UrlInfo> GetAllUrl()
        {
            return this.EnumerateRows()
                .Select(DataStoreHelper.ToUrlInfo)
                .ToList();
        }

        public UrlInfo GetInfo(string psnurl)
        {

            DataRow row = null;
            string selectString;
            if (this.FuzzyMatching)
            {
                selectString = $"RemoteUrl LIKE '{psnurl.Replace('*', '%')}'";
            }
            else
            {
                selectString = $"RemoteUrl='{psnurl}'";
            }
            row = this.historyTable.Select(selectString).FirstOrDefault();
            if (row != null)
            {
                return row.ToUrlInfo();
            }
            return null;
        }

        public bool UpdataLog(UrlInfo urlinfo)
        {
            DataRow row;
            try
            {
                if (urlinfo.PsnUrl.IndexOf('/') >= 0)
                {
                    row = this.historyTable
                        .Select($"RemoteUrl='{urlinfo.PsnUrl}'")
                        .FirstOrDefault();
                    if (row != null)
                    {
                        urlinfo.CopyToDataRow(row);
                        return true;
                    }
                }
                else
                {
                    row = this.accessTable
                        .Select($"RemoteUrl='{urlinfo.PsnUrl}'")
                        .FirstOrDefault();
                    if (row != null)
                    {
                        urlinfo.CopyToAccessDataRow(row);
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public IEnumerable<UrlInfo> Query(int skip = 0, int count = 10)
        {
            return this.EnumerateRows()
                .Skip(skip)
                .Take(count)
                .Select(DataStoreHelper.ToUrlInfo);
        }

        public void ClearAll()
        {
            this.Clear(HistoryClearOptions.All);
        }

        public void Clear(HistoryClearOptions clearOptions)
        {
            this.historyTable.Clear();
            this.accessTable.Clear();
        }

        public void Save()
        {
            var fileInfo = new FileInfo(this.SavePath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            this.dataSet.WriteXml(fileInfo.Create(), XmlWriteMode.WriteSchema);
        }
    }
}
