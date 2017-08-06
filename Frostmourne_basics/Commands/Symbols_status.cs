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
        public static Error Load_not_inactive_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_not_inactive_symbols(ref _sl);
        }

        public static Error Load_active_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_active_symbols(ref _sl);
        }

        public static Error Load_inactive_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_inactive_symbols(ref _sl);
        }

        public static Error Load_simulation_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_simulation_symbols(ref _sl);
        }

        public static Error Load_standby_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_standby_symbols(ref _sl);
        }

        public static Error Load_all_symbols(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_all_symbols_status(ref _sl);
        }

        public static Error Load_all_symbols_status(ref Configuration configuration, ref Mysql MyDB, ref List<Symbol> _sl)
        {
            return MyDB.Load_all_symbols_status(ref _sl);
        }

        public static Error Load_symbol_status(ref Configuration configuration, ref Mysql MyDB, ref Symbol _s)
        {
            return MyDB.Load_symbol_state(ref _s);
        }

        public static Error Update_symbol_status(ref Configuration configuration, ref Mysql MyDB, Symbol _s_to_update)
        {

            switch (_s_to_update.State)
            {
                case "inactive":
                    break;
                case "standby":
                    break;
                case "simulation":
                    break;
                case "active":
                    break;
                default:
                    return new Error(true, "not supported status : " + _s_to_update.State);
            }

            List<Symbol> symbols_list = new List<Symbol>();
            Symbol symbol = new Symbol();

            Error err = new Error();

            err = Load_all_symbols(ref configuration, ref MyDB, ref symbols_list);
            if (err.IsAnError)
                return err;

            foreach (Symbol s in symbols_list)
            {
                if (s.Id == _s_to_update.Id)
                {
                    symbol.Id = s.Id;
                    break;
                }
            }

            if (symbol.Id == 0)
                return new Error(false, "This ID doesn't exist : " + _s_to_update.Id.ToString());

            return MyDB.Update_symbol_status(_s_to_update);
        }
    }
}
