using System;
using System.Collections.Generic;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Xtb
    {

        
        public static Error Retrieve_bids_of_symbol_from_xtb(ref SyncAPIConnector _api_connector, string _symbol, xAPI.Codes.PERIOD_CODE _period, Int64 tFrom, ref List<Bid> bids)
        {

            Log.Info("Retrieve XTB data for -> " + _symbol);
            
            ChartLastResponse resp;
            
            try
            {
                resp = APICommandFactory.ExecuteChartLastCommand(_api_connector, _symbol, _period, tFrom*1000);
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
                bids.Add(new Bid(_symbol, Convert.ToInt64(v.Ctm/1000), Convert.ToDouble(v.Open) + Convert.ToDouble(v.Close)));

            return new Error(false, "Data symbol retrieved !");
        }
        /*
        public static Error Open_trade_xtb(ref SyncAPIConnector _api_connector, Symbol _symbol, ref Trade _trade)
        {
            Error err;
            List<Symbol> not_inactiv_symbols = new List<Symbol>();

            foreach (Symbol not_inactiv_s in not_inactiv_symbols)
            {
                if (_symbol.Name == not_inactiv_s.Name || _symbol.Id == not_inactiv_s.Id)
                {
                    _symbol = not_inactiv_s;
                    break;
                }
            }

            if (_symbol.Id == 0)
                return new Error(true, "this symbols are not inactive or doesn't exist");
            
            SymbolResponse symbolResponse = APICommandFactory.ExecuteSymbolCommand(_api_connector, _symbol.Name);

            double price = symbolResponse.Symbol.Ask.GetValueOrDefault();

            if (_trade.Trade_type == 0 || _trade.Trade_type == 1)
            {
                if (_trade.Trade_type == 0)
                    _trade.Cmd = TRADE_OPERATION_CODE.BUY;
                else
                    _trade.Cmd = TRADE_OPERATION_CODE.SELL;
            }
            else
            {
                return new Error(true, "Not a valid trade type ! -> " + _trade.Trade_type.ToString());
            } 

            TradeTransInfoRecord ttOpenInfoRecord = new TradeTransInfoRecord(
                _trade.Cmd,
                TRADE_TRANSACTION_TYPE.ORDER_OPEN,
                price, _trade.Stop_loss, 0, _symbol.Name, _trade.Volume, 0, _trade.Opened_reason, 0);

            TradeTransactionResponse tradeTransactionResponse = APICommandFactory.ExecuteTradeTransactionCommand(_api_connector, ttOpenInfoRecord);

            TradeTransactionStatusResponse ttsResponse = APICommandFactory.ExecuteTradeTransactionStatusCommand(_api_connector, tradeTransactionResponse.Order);

            if (ttsResponse.RequestStatus.Code != 3)
            {
                return new Error(true, "Error during TradeTransactionStatusCommand -> RequestStatus = " + ttsResponse.RequestStatus.Code + " | Message = " + ttsResponse.Message);
            }

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

            _trade.Digits = Convert.ToInt64(tradeRecord.Digits);

            double multiplier = Math.Pow(10, Convert.ToInt32(_trade.Digits));

            _trade.Symbol = _symbol;
            _trade.Xtb_order_id_1 = Convert.ToInt64(tradeRecord.Order);
            _trade.Xtb_order_id_2 = Convert.ToInt64(tradeRecord.Order2);
            _trade.Opened_price = Convert.ToDouble(tradeRecord.Open_price) * multiplier;
            _trade.Volume = Convert.ToDouble(tradeRecord.Volume);
            _trade.Profit = Convert.ToDouble(tradeRecord.Profit);
            _trade.Opened_at = Tool.LongUnixTimeStampToDateTime(tradeRecord.Open_time);
            
            //if (_trade.Trade_type == 0)
            //    _trade.Stop_loss = _trade.Opened_price - configuration.Stop_loss;
            //else
            //    _trade.Stop_loss = _trade.Opened_price + configuration.Stop_loss;

            //TODO Setup Stop_loss
            
            return _trade.Open_Trade(_api_connector, ref configuration, ref _trade);
        }*/

        public static Error Get_open_trades_from_xtb(ref SyncAPIConnector _api_connector, ref List<Trade> _trades)
        {
            TradesResponse tradesResponse = APICommandFactory.ExecuteTradesCommand(_api_connector, true);
            
            foreach (TradeRecord tr in tradesResponse.TradeRecords)
                _trades.Add(new Trade(Convert.ToInt64(tr.Order2), Convert.ToDouble(tr.Profit)));
            
            return new Error(false, "");
        }
        /*
        public static Error Close_trade_xtb(ref SyncAPIConnector _api_connector, ref Trade _trade_to_close)
        {
            Error err;
            List<Trade> opened_trades = new List<Trade>();
            
            err = Get_open_trades_from_xtb(ref _api_connector, ref opened_trades);
            if (err.IsAnError)
                return err;

            Trade trade_to_close = new Trade();

            foreach (Trade t in opened_trades)
            {
                if (t.Id == _trade_to_close.Id)
                {
                    trade_to_close = t;
                    break;
                }
            }

            if (trade_to_close.Id == 0)
                return (new Error(true, "This trade ID doesn't exist -> " + _trade_to_close.Id));

            err = MyDB.Get_trade_by_db_id(ref trade_to_close);
            if (err.IsAnError)
                return err;

            trade_to_close.Closed_reason = _trade_to_close.Closed_reason;
            _trade_to_close = trade_to_close;

            if (_trade_to_close.Trade_type == 0)
                _trade_to_close.Cmd = TRADE_OPERATION_CODE.BUY;
            else
                _trade_to_close.Cmd = TRADE_OPERATION_CODE.SELL;
            
            SymbolResponse symbolResponse = APICommandFactory.ExecuteSymbolCommand(_api_connector, _trade_to_close.Symbol.Name);
          
            TradeTransInfoRecord ttCloseInfoRecord = new TradeTransInfoRecord(
                _trade_to_close.Cmd,
                TRADE_TRANSACTION_TYPE.ORDER_CLOSE,
                symbolResponse.Symbol.Ask.GetValueOrDefault(), _trade_to_close.Stop_loss, 0.0,
            _trade_to_close.Symbol.Name, _trade_to_close.Volume, _trade_to_close.Xtb_order_id_1, _trade_to_close.Closed_reason, 0);

            TradeTransactionResponse closeTradeTransactionResponse = APICommandFactory.ExecuteTradeTransactionCommand(_api_connector, ttCloseInfoRecord, true);

            TradeTransactionStatusResponse ttsCloseResponse = APICommandFactory.ExecuteTradeTransactionStatusCommand(_api_connector, closeTradeTransactionResponse.Order);

            if (ttsCloseResponse.RequestStatus.Code != 3)
            {
                return new Error(true, "Error during TradeTransactionStatusCommand -> RequestStatus = " + ttsCloseResponse.RequestStatus.Code + " | Message = " + ttsCloseResponse.Message);
            }

            TradesHistoryResponse resp = APICommandFactory.ExecuteTradesHistoryCommand(_api_connector, 0, 0);

            TradeRecord tradeClosed = new TradeRecord();

            foreach (TradeRecord tr in resp.TradeRecords)
            {
                if (tr.Position == _trade_to_close.Xtb_order_id_1)
                {
                    tradeClosed = tr;
                    break;
                }
            }

            double multiplier = Math.Pow(10, Convert.ToInt32(_trade_to_close.Digits));
            
            _trade_to_close.Closed_price = Convert.ToDouble(tradeClosed.Close_price) * multiplier;
            _trade_to_close.Closed_at = Tool.LongUnixTimeStampToDateTime(tradeClosed.Close_time);
            
            return MyDB.Close_trade(_trade_to_close);
        }*/
    }
}
