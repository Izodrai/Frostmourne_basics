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

        public static Error Get_stock_values_from_xtb(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, string _s_to_retrieve, ref List<Bid> _bids, DateTime _from)
        {
            Error err;

            err = Tool.InitXtb(ref configuration, ref Xtb_api_connector);
            if (err.IsAnError)
                return err;

            DateTime tNow = DateTime.Now;

            Tool.Get_xtb_server_time(ref Xtb_api_connector, ref tNow);

            err = Xtb.Retrieve_bids_of_symbol_from_xtb(ref Xtb_api_connector, ref configuration, _s_to_retrieve, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, _from, ref _bids);
            if (err.IsAnError)
                return err;

            err = Tool.CloseXtb(ref Xtb_api_connector);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }
        /*
        public static Error Get_from_xtb_stock_values_from_last_insert_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_update, ref List<Bid> _bids)
        {
            Error err;
            List<Symbol> not_inactiv_symbols = new List<Symbol>();
            Tool.InitMyDb(ref configuration, ref MyDB);

            err = MyDB.Load_not_inactive_symbols(ref not_inactiv_symbols);
            if (err.IsAnError)
                return err;

            foreach (Symbol not_inactiv_s in not_inactiv_symbols)
            {
                if (_s_to_update.Name == not_inactiv_s.Name || _s_to_update.Id == not_inactiv_s.Id)
                {
                    _s_to_update = not_inactiv_s;
                    break;
                }
            }

            if (_s_to_update.Id == 0)
                return new Error(true, "this symbol id are not inactive or doesn't exist");
            
            Bid last_bid = new Bid();
            MyDB.Load_last_value_for_symbol(ref last_bid, _s_to_update);
            
            if (last_bid.Bid_at == null || last_bid.Bid_at == new DateTime())
                last_bid.Bid_at = DateTime.Now.AddDays(-90);

            DateTime tNow = DateTime.Now;
            
            err = Tool.InitXtb(ref configuration, ref Xtb_api_connector);
            if (err.IsAnError)
                return err;

            Tool.Get_xtb_server_time(ref Xtb_api_connector, ref tNow);
            
            Log.Info("Time Now -> " + tNow.ToString("yyyy-MM-dd HH:mm:ss") + " ||| last data -> " + last_bid.Bid_at.ToString("yyyy-MM-dd HH:mm:ss") + " ||| retrieve data from -> " + last_bid.Bid_at.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            Log.JumpLine();

            last_bid.Bid_at = last_bid.Bid_at.AddDays(-1);

            List<Bid> mysql_bids = new List<Bid>();
            List<Bid> xtb_bids = new List<Bid>();

            try
            {
                MyDB.Load_bids_values_for_symbol_between_to_date(ref mysql_bids, last_bid.Bid_at, tNow, _s_to_update);
            }
            catch (Exception e)
            {
                return new Error(true, "Error during Load_bids_values_symbol : " + e.Message);
            }

            err = Xtb.Retrieve_bids_of_symbol_from_xtb(ref Xtb_api_connector, ref configuration, _s_to_update, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, last_bid.Bid_at, ref xtb_bids);
            if (err.IsAnError)
                return err;
            
            err = Tool.CloseXtb(ref Xtb_api_connector);
            if (err.IsAnError)
                return err;

            List<Bid> bids_to_insert_or_update = new List<Bid>();

            Log.JumpLine();
            Log.Info("mysql bid : " + mysql_bids.Count().ToString());
            Log.Info("xtb bid : " + xtb_bids.Count().ToString());

            foreach (Bid xtb_bid in xtb_bids)
            {
                bool exist = false;
                
                foreach (Bid mysql_bid in mysql_bids)
                {
                    if (xtb_bid.Symbol.Id != mysql_bid.Symbol.Id)
                        continue;
                    if (xtb_bid.Bid_at != mysql_bid.Bid_at)
                        continue;
                    if (xtb_bid.Last_bid != mysql_bid.Last_bid)
                        bids_to_insert_or_update.Add(xtb_bid);

                    exist = true;
                }
                if (!exist)
                {
                    bids_to_insert_or_update.Add(xtb_bid);
                }
            }

            err = MyDB.Insert_or_update_bids_values(bids_to_insert_or_update);
            if (err.IsAnError)
                return err;

            Log.JumpLine();
            Log.Info("Update Database");
            
            err = MyDB.Load_bids_values_for_symbol_between_to_date(ref _bids, last_bid.Bid_at.AddDays(-1), tNow, _s_to_update);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }*/
    }
}
