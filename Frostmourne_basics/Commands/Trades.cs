using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Sync;

namespace Frostmourne_basics.Commands
{
    public partial class Commands
    {
        public static Error Open_trade_xtb(SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _symbol, int _cmd, double _volume)
        {
            return Xtb.Open_trade_xtb(_api_connector, ref configuration, ref MyDB, _symbol, _cmd, _volume);
        }
    }
}
