using System.Collections.Generic;
using xAPI.Sync;

namespace Frostmourne_basics.Commands
{
    public partial class Commands
    {
        public static Error Open_trade_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, Symbol _symbol, ref Trade _trade)
        {
            Error err;

            err = Tool.InitXtb(ref configuration, ref _api_connector);
            if (err.IsAnError)
                return err;

            /*err = Xtb.Open_trade_xtb(ref _api_connector, ref configuration, _symbol, ref _trade);
            if (err.IsAnError)
                return err;*/

            err = Tool.CloseXtb(ref _api_connector);
            return err;
        }

        public static Error Get_open_trades_from_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, ref List<Trade> _trades)
        {
            Error err;

            err = Tool.InitXtb(ref configuration, ref _api_connector);
            if (err.IsAnError)
                return err;

            err = Xtb.Get_open_trades_from_xtb(ref _api_connector, ref _trades);
            if (err.IsAnError)
                return err;

            err = Tool.CloseXtb(ref _api_connector);
            return err;
        }
        /*
        public static Error Close_trade_xtb(ref SyncAPIConnector _api_connector, ref Configuration configuration, ref Trade _trade)
        {
            Error err;

            err = Tool.InitXtb(ref configuration, ref _api_connector);
            if (err.IsAnError)
                return err;

            err = Xtb.Close_trade_xtb(ref _api_connector, ref configuration, ref _trade);
            if (err.IsAnError)
                return err;

            err = Tool.CloseXtb(ref _api_connector);
            return err;
        }*/
    }
}
