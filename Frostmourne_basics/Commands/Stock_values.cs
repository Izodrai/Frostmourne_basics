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

        public static Error Get_stock_values_from_xtb(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, string _s_to_retrieve, ref List<Bid> _bids, Int64 _from)
        {
            Error err;

            err = Tool.InitXtb(ref configuration, ref Xtb_api_connector);
            if (err.IsAnError)
                return err;
            
            err = Xtb.Retrieve_bids_of_symbol_from_xtb(ref Xtb_api_connector, ref configuration, _s_to_retrieve, xAPI.Codes.PERIOD_CODE.PERIOD_M5, _from, ref _bids);
            if (err.IsAnError)
                return err;

            err = Tool.CloseXtb(ref Xtb_api_connector);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }
    }
}
