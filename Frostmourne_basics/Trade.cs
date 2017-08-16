using Frostmourne_basics.Dbs;
using System;
using xAPI.Codes;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Trade
    {
        public int Id { get; set; }
        
        public long Xtb_order_id_1 { get; set; }

        public long Xtb_order_id_2 { get; set; }


        public Symbol Symbol { get; set; }

        public TRADE_OPERATION_CODE Cmd { get; set; }

        public int Trade_type { get; set; }


        public double Volume { get; set; }

        public double Stop_loss { get; set; }

        public double Profit { get; set; }

        public long Digits { get; set; }


        public DateTime Opened_at { get; set; }

        public DateTime Closed_at { get; set; }

        public double Opened_price { get; set; }

        public double Closed_price { get; set; }

        public string Opened_reason { get; set; }

        public string Closed_reason { get; set; }
        
        public Trade() { }

        public Trade(int _id, long _xtb_order_id_1, long _xtb_order_id_2, Symbol _symbol, int _trade_type, 
            double _volume, double _stop_loss, double _profit, DateTime _opened_at, DateTime _closed_at,
            double _opened_price, double _closed_price, string _opened_reason, string _closed_reason)
        {
            this.Id = _id;
            this.Xtb_order_id_1 = _xtb_order_id_1;
            this.Xtb_order_id_2 = _xtb_order_id_2;
            this.Symbol = _symbol;
            this.Trade_type = _trade_type;
            this.Volume = _volume;
            this.Stop_loss = _stop_loss;
            this.Profit = _profit;
            this.Opened_at = _opened_at;
            this.Closed_at = _closed_at;
            this.Opened_price = _opened_price;
            this.Closed_price = _closed_price;
            this.Opened_reason = _opened_reason;
            this.Closed_reason = _closed_reason;
        }

        public Error Open_Trade(SyncAPIConnector _api_connector, ref Configuration configuration, ref Mysql MyDB, ref Trade _trade)
        {
            Error err;

            err = MyDB.New_trade(ref _trade);
            if (err.IsAnError)
                return err;
            
            return new Error(false, "Trade opened !");
        }




    }
}
