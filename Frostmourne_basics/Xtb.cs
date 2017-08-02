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
                bids.Add(new Bid(new Symbol(_symbol.Id, _symbol.Name, ""), Tool.LongUnixTimeStampToDateTime(v.Ctm), Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close), ""));

            return new Error(false, "Data symbol retrieved !");
        }
    }
}
