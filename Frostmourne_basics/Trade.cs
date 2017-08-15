using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Trade
    {
        public int Id { get; set; }

        public Symbol Symbol { get; set; }

        public TRADE_OPERATION_CODE Cmd { get; set; }

        public double Volume { get; set; }

        public double Price { get; set; }

        public double Stop_loss { get; set; }

        public double Opened_price { get; set; }

        public double Closed_price { get; set; }

        public double Opened_reason { get; set; }

        public double Closed_reason { get; set; }

        public double Opened_at { get; set; }

        public double Closed_at { get; set; }



        public Error Open_Trade(SyncAPIConnector _api_connector, ref Configuration configuration, Symbol _symbol, int _cmd, double _volume)
        {

            Error err;
            err = Tool.InitXtb(ref configuration, ref _api_connector);
            if (err.IsAnError)
                return err;

            SymbolResponse symbolResponse = APICommandFactory.ExecuteSymbolCommand(_api_connector, _symbol.Name);

            this.Price = symbolResponse.Symbol.Ask.GetValueOrDefault();
            
            //if (_cmd >= 0 && _cmd <= 7)
            if (_cmd == 0 || _cmd == 1)
            {
                if (_cmd == 0)
                {
                    this.Cmd = TRADE_OPERATION_CODE.BUY;
                } else
                {
                    this.Cmd = TRADE_OPERATION_CODE.SELL;
                }
            } else
            {
                return new Error(true, "Not a valid cmd ! -> " + _cmd.ToString());
            }
            
            
            if (_volume >= _symbol.Lot_min_size && _volume <= _symbol.Lot_max_size)
            {
                this.Volume = _volume;
            }
            else
            {
                return new Error(true, "Not a valid volume ! -> " + _cmd.ToString());
            }

            TradeTransInfoRecord ttOpenInfoRecord = new TradeTransInfoRecord(
                this.Cmd,
                TRADE_TRANSACTION_TYPE.ORDER_OPEN,
                this.Price, this.Stop_loss, 0, _symbol.Name, this.Volume, 0, "", 0);

            TradeTransactionResponse tradeTransactionResponse = APICommandFactory.ExecuteTradeTransactionCommand(_api_connector, ttOpenInfoRecord);

            TradeTransactionStatusResponse ttsResponse = APICommandFactory.ExecuteTradeTransactionStatusCommand(_api_connector, tradeTransactionResponse.Order);
            
            TradesResponse tradesResponse = APICommandFactory.ExecuteTradesCommand(_api_connector, true);

            TradeRecord tradeRecord = new TradeRecord();

            foreach (TradeRecord tr in tradesResponse.TradeRecords)
            {
                if (tr.Order2 == ttsResponse.Order)
                {
                    tradeRecord = tr;
                    break;
                }
            }

            Log.Info("#######");
            Log.JumpLine();
            Log.JumpLine();

            Log.Info("tradeRecord.Order -> " + tradeRecord.Order);
            Log.Info("tradeRecord.Order2 -> " + tradeRecord.Order2);
            Log.Info("tradeRecord.Digits -> " + tradeRecord.Digits);
            Log.Info("tradeRecord.Open_price -> " + tradeRecord.Open_price);
            Log.Info("tradeRecord.Open_time -> " + tradeRecord.Open_time);
            Log.Info("tradeRecord.Closed -> " + tradeRecord.Closed);
            Log.Info("tradeRecord.Cmd -> " + tradeRecord.Cmd);
            Log.Info("tradeRecord.Profit -> " + tradeRecord.Profit);
            Log.Info("tradeRecord.Storage -> " + tradeRecord.Storage);
            Log.Info("tradeRecord.Volume -> " + tradeRecord.Volume);

            Tool.CloseXtb(ref _api_connector);
            if (err.IsAnError)
                return err;



            return new Error(false, "Trade opened !");
        }
    }
}
