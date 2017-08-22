using Frostmourne_basics.Dbs;
using System.Collections.Generic;
using xAPI.Sync;

namespace Frostmourne_basics.Commands
{
    public partial class Commands
    {
        public static Error Open_trade_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _symbol, ref Trade _trade)
        {
            return Xtb.Open_trade_xtb(ref _api_connector, ref configuration, ref MyDB, _symbol, ref _trade);
        }

        public static Error Get_open_trades_from_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, ref List<Trade> _trades)
        {
            return Xtb.Get_open_trades_from_xtb(ref _api_connector, ref configuration, ref MyDB, ref _trades);
        }

        public static Error Close_trade_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, ref Trade _trade)
        {
            return Xtb.Close_trade_xtb(ref _api_connector, ref configuration, ref MyDB, ref _trade);
        }
    }
}
