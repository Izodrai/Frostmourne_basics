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
            
            MyDB.Load_last_value_for_symbol(ref _bid, _s_to_check);

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

            MyDB.Count_value_for_symbol(ref _ct, _s_to_check);
            
            return new Error(false, "");
        }
    }
}
