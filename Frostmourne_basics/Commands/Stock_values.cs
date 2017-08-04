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
        public static Error Get_from_db_last_insert_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_check, ref Bid _bid)
        {
            List<Symbol> symbols_list = new List<Symbol>();
            Symbol symbol = new Symbol();

            Error err = new Error();

            err = Load_all_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id == _s_to_check.Id)
                {
                    symbol = s;
                    break;
                }
            }

            if (symbol.Id == 0)
                return new Error(false, "This ID doesn't exist : " + _s_to_check.Id.ToString());

            _s_to_check = symbol;
            _s_to_check.Description = "";

            err = MyDB.Load_last_value_for_symbol(ref _bid, _s_to_check);
            if (err.IsAnError)
                return err;

            if (_bid.Id == 0)
                return new Error(true, "Something appening during select query (Load_last_value_for_symbol)");

            return new Error(false, "");
        }

        public static Error Get_from_db_nb_insert_by_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_check, ref int _ct)
        {
            List<Symbol> symbols_list = new List<Symbol>();
            Symbol symbol = new Symbol();

            Error err = new Error();

            err = Load_all_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id == _s_to_check.Id)
                {
                    symbol = s;
                    break;
                }
            }

            if (symbol.Id == 0)
                return new Error(false, "This ID doesn't exist : " + _s_to_check.Id.ToString());

            _s_to_check = symbol;
            _s_to_check.Description = "";

            err = MyDB.Count_value_for_symbol(ref _ct, _s_to_check);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }

        public static Error Get_from_db_nb_insert_by_day_between_two_date_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_check, DateTime _from, DateTime _to, ref List<Bid_by_date> bids_ct)
        {
            List<Symbol> symbols_list = new List<Symbol>();
            Symbol symbol = new Symbol();

            Error err = new Error();

            err = Load_all_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id == _s_to_check.Id)
                {
                    symbol = s;
                    break;
                }
            }

            if (symbol.Id == 0)
                return new Error(false, "This ID doesn't exist : " + _s_to_check.Id.ToString());

            _s_to_check = symbol;
            _s_to_check.Description = "";

            err = MyDB.Count_value_by_day_for_symbol(_s_to_check, _from, _to, ref bids_ct);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }

        public static Error Get_from_db_stock_values_between_two_date_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_check, DateTime _from, DateTime _to, ref List<Bid> _bids)
        {
            List<Symbol> symbols_list = new List<Symbol>();
            Symbol symbol = new Symbol();

            Error err = new Error();

            err = Load_all_symbols(ref Xtb_api_connector, ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id == _s_to_check.Id)
                {
                    symbol = s;
                    break;
                }
            }

            if (symbol.Id == 0)
                return new Error(false, "This ID doesn't exist : " + _s_to_check.Id.ToString());

            _s_to_check = symbol;
            _s_to_check.Description = "";

            err = MyDB.Load_bids_values_for_symbol_between_to_date(ref _bids, _from, _to, _s_to_check);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }

        public static Error Get_from_xtb_stock_values_from_last_insert_for_symbol(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_update, ref List<Bid> _bids)
        {
            Error err;
            List<Symbol> not_inactiv_symbols = new List<Symbol>();
            
            err = MyDB.Load_not_inactive_symbols(ref not_inactiv_symbols);
            if (err.IsAnError)
            {
                MyDB.Close();
                return err;
            }
            MyDB.Close();

            foreach (Symbol not_inactiv_s in not_inactiv_symbols)
            {
                if (_s_to_update.Name == not_inactiv_s.Name)
                {
                    _s_to_update = not_inactiv_s;
                    break;
                }
            }

            if (_s_to_update.Id == 0)
                return new Error(true, "this symbols are not inactive or doesn't exist");

            _s_to_update.Description = "";

            Bid last_bid = new Bid();
            MyDB.Load_last_value_for_symbol(ref last_bid, _s_to_update);

            DateTime tNow = DateTime.UtcNow;

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
                MyDB.Close();
                return new Error(true, "Error during Load_bids_values_symbol : " + e.Message);
            }

            err = Xtb.Retrieve_bids_of_symbol_from_xtb(Xtb_api_connector, _s_to_update, xAPI.Codes.PERIOD_CODE.PERIOD_M5, tNow, last_bid.Bid_at, ref xtb_bids);
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
            
            err = MyDB.Load_bids_values_for_symbol_between_to_date(ref _bids, last_bid.Bid_at, tNow, _s_to_update);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }

        public static Error Update_db_stock_values_calculation(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, ref List<Bid> _bids)
        {
            Error err;
            List<Bid> bids_to_update = new List<Bid>();

            foreach (Bid b in _bids)
            {
                if (b.Id != 0)
                {
                    bids_to_update.Add(b);
                }
            }

            if (bids_to_update.Count() == 0)
                return new Error(false, "");
            
            err = MyDB.Update_bid_calculations(bids_to_update);
            if (err.IsAnError)
                return err;

            return new Error(false, "");
        }
    }
}
