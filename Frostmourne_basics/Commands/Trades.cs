using Frostmourne_basics.Dbs;
using xAPI.Sync;

namespace Frostmourne_basics.Commands
{
    public partial class Commands
    {
        public static Error Open_trade_xtb(SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _symbol, ref Trade _trade)
        {
            return Xtb.Open_trade_xtb(_api_connector, ref configuration, ref MyDB, _symbol, ref _trade);
        }
    }
}
