using System;

namespace PSXDH.DAL
{

    public partial class PowerDataHistory
    {
        [Flags]
        public enum HistoryClearOptions
        {
            History = 0x0001,
            Access = 0x0002,
            All = History | Access,
        }
    }
}
