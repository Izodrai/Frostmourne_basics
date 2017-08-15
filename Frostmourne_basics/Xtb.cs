using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Xtb
    {
        public static Error Retrieve_bids_of_symbol_from_xtb(SyncAPIConnector _api_connector, Symbol _symbol, xAPI.Codes.PERIOD_CODE _period, DateTime tNow, DateTime tFrom, ref List<Bid> bids)
        {
            Log.Info("Retrieve XTB data for -> " + _symbol.Name);
            
            ////////////////
            // Récupération des données de xtb sur la période
            ////////////////

            long? timeTStart = Tool.DateTimeToLongUnixTimeStamp(tFrom);

            ChartLastResponse resp;

            try
            {
                resp = APICommandFactory.ExecuteChartLastCommand(_api_connector, _symbol.Name, _period, timeTStart);
            }
            catch (Exception e)
            {
                return new Error(true, "Error during ExecuteChartLastCommand : " + e.Message);
            }

            RateInfoRecord[] infos = new RateInfoRecord[resp.RateInfos.Count];

            resp.RateInfos.CopyTo(infos, 0);

            if (infos.Length == 0)
                return new Error(true, "No data to retrieve in this range");

            foreach (RateInfoRecord v in infos)
            {
                bids.Add(new Bid(new Symbol(_symbol.Id, _symbol.Name, ""), Tool.LongUnixTimeStampToDateTime(v.Ctm), Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close), ""));
            }

            return new Error(false, "Data symbol retrieved !");
        }

        public static Error Open_trade_xtb(SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, Symbol _symbol, int _cmd, double _volume)
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
                if (_symbol.Name == not_inactiv_s.Name)
                {
                    _symbol = not_inactiv_s;
                    break;
                }
            }

            if (_symbol.Id == 0)
                return new Error(true, "this symbols are not inactive or doesn't exist");

            Trade trade = new Trade();

            err = trade.Open_Trade(_api_connector, ref configuration, _symbol, _cmd, _volume);
            if (err.IsAnError)
                return err;
            
            return new Error(false, "Trade opened !");
        }


    }
}
