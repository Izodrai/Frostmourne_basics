using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        public Error Get_trade_by_order_id(ref Trade _trade)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;
            
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, symbol_id, trade_type_id, xtb_order_id_1, xtb_order_id_2, opened_at, closed_at, opened_value, closed_value, opened_reason, closed_reason, stop_loss_value, volume, profit, digits FROM trades WHERE xtb_order_id_2 = @xtb_order_id_2 ", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@xtb_order_id_2", _trade.Xtb_order_id_2);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        
                        int id = Convert.ToInt32(values[0]);

                        Symbol s = new Symbol();
                        s.Id = Convert.ToInt32(values[1]);
                        err = Load_symbol_state(ref s);
                        if (err.IsAnError)
                        {
                            this.Close();
                            return err;
                        }

                        int trade_type = Convert.ToInt32(values[2]);
                        long xtb_order_id_1 = Convert.ToInt64(values[3]);
                        long xtb_order_id_2 = Convert.ToInt64(values[4]);
                        double volume = Convert.ToDouble(values[12]);
                        double stop_loss = Convert.ToDouble(values[11]);
                        double profit = Convert.ToDouble(values[13]);

                        DateTime opened_at = DateTime.Parse(Convert.ToString(values[5]));

                        DateTime closed_at;
                        try
                        {
                            closed_at = DateTime.Parse(Convert.ToString(values[6]));
                        }
                        catch
                        {
                            closed_at = new DateTime();
                        }

                        double opened_price = Convert.ToDouble(values[7]);

                        double closed_price;
                        try
                        {
                            closed_price = Convert.ToDouble(values[8]);
                        }
                        catch
                        {
                            closed_price = 0.0;
                        }

                        string opened_reason = Convert.ToString(values[9]);
                        string closed_reason = Convert.ToString(values[10]);
                        int digits = Convert.ToInt32(values[14]);

                        _trade = new Trade(id, xtb_order_id_1, xtb_order_id_2, s, trade_type, volume, stop_loss, profit, opened_at, closed_at, opened_price, closed_price, opened_reason, closed_reason, digits);
                    }
                }
                
                this.Close();

                return new Error(false, "selected");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
        
        public Error Get_trades_by_order_id(ref List<Trade> _trades, bool _keep_xtb_profit)
        {
            Error err;

            List<Symbol> symbols = new List<Symbol>();

            err = this.Load_all_symbols_status(ref symbols);
            if (err.IsAnError)
            {
                this.Close();
                return err;
            }
            this.Close();
            
            err = this.Connect();
            if (err.IsAnError)
                return err;

            List<string> args = new List<string>();

            if (_trades.Count == 0)
                return err;

            foreach (Trade t in _trades)
            {
                args.Add(t.Xtb_order_id_2.ToString());
            }
            string req = "("+String.Join(",", args.ToArray())+")";

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, symbol_id, trade_type_id, xtb_order_id_1, xtb_order_id_2, opened_at, closed_at, opened_value, closed_value, opened_reason, closed_reason, stop_loss_value, volume, profit, digits FROM trades WHERE xtb_order_id_2 IN " + req, this.Mysql_connector);

                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Trade> db_trades = new List<Trade>();

                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        int id = Convert.ToInt32(values[0]);

                        Symbol s = new Symbol();
                        s.Id = Convert.ToInt32(values[1]);

                        foreach (Symbol symbol in symbols)
                        {
                            if (s.Id == symbol.Id)
                            {
                                s = symbol;
                                break;
                            }
                        }

                        int trade_type = Convert.ToInt32(values[2]);
                        long xtb_order_id_1 = Convert.ToInt64(values[3]);
                        long xtb_order_id_2 = Convert.ToInt64(values[4]);
                        double volume = Convert.ToDouble(values[12]);
                        double stop_loss = Convert.ToDouble(values[11]);

                        double profit = Convert.ToDouble(values[13]);

                        if (_keep_xtb_profit)
                        {
                            foreach (Trade t in _trades)
                            {
                                if (xtb_order_id_2 == t.Xtb_order_id_2)
                                {
                                    profit = t.Profit;
                                    break;
                                }
                            }
                        }
                           
                        DateTime opened_at = DateTime.Parse(Convert.ToString(values[5]));

                        DateTime closed_at;
                        try
                        {
                            closed_at = DateTime.Parse(Convert.ToString(values[6]));
                        }
                        catch
                        {
                            closed_at = new DateTime();
                        }

                        double opened_price = Convert.ToDouble(values[7]);

                        double closed_price;
                        try
                        {
                            closed_price = Convert.ToDouble(values[8]);
                        }
                        catch
                        {
                            closed_price = 0.0;
                        }

                        string opened_reason = Convert.ToString(values[9]);
                        string closed_reason = Convert.ToString(values[10]);
                        int digits = Convert.ToInt32(values[14]);

                        db_trades.Add(new Trade(id, xtb_order_id_1, xtb_order_id_2, s, trade_type, volume, stop_loss, profit, opened_at, closed_at, opened_price, closed_price, opened_reason, closed_reason, digits));
                    }
                    _trades = db_trades;
                }
                
                this.Close();

                return new Error(false, "selected");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
        
        public Error New_trade(ref Trade _trade)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO trades (symbol_id, trade_type_id, xtb_order_id_1, xtb_order_id_2, opened_at, opened_value, opened_reason, stop_loss_value, volume, profit, digits) VALUES (@symbol_id, @trade_type_id, @xtb_order_id_1, @xtb_order_id_2, @opened_at, @opened_value, @opened_reason, @stop_loss_value, @volume, @profit, @digits)", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@symbol_id", 1);
                cmd.Parameters.AddWithValue("@trade_type_id", 1);
                cmd.Parameters.AddWithValue("@xtb_order_id_1", 1);
                cmd.Parameters.AddWithValue("@xtb_order_id_2", 1);
                cmd.Parameters.AddWithValue("@opened_at", "");
                cmd.Parameters.AddWithValue("@opened_value", 1);
                cmd.Parameters.AddWithValue("@opened_reason", "");
                cmd.Parameters.AddWithValue("@stop_loss_value", 1);
                cmd.Parameters.AddWithValue("@volume", "0.0");
                cmd.Parameters.AddWithValue("@profit", "0.0");
                cmd.Parameters.AddWithValue("@digits", 1);
                
                cmd.Parameters["@symbol_id"].Value = _trade.Symbol.Id;
                cmd.Parameters["@trade_type_id"].Value = _trade.Trade_type;
                cmd.Parameters["@xtb_order_id_1"].Value = _trade.Xtb_order_id_1;
                cmd.Parameters["@xtb_order_id_2"].Value = _trade.Xtb_order_id_2;
                cmd.Parameters["@opened_at"].Value = _trade.Opened_at.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters["@opened_value"].Value = _trade.Opened_price;
                cmd.Parameters["@opened_reason"].Value = _trade.Opened_reason;
                cmd.Parameters["@stop_loss_value"].Value = _trade.Stop_loss;
                cmd.Parameters["@volume"].Value = _trade.Volume.ToString().Replace(",",".");
                cmd.Parameters["@profit"].Value = _trade.Profit.ToString().Replace(",", ".");
                cmd.Parameters["@digits"].Value = _trade.Digits;

                cmd.ExecuteNonQuery();

                this.Close();

                err = Get_trade_by_order_id(ref _trade);
                return err;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
        
        /*
        public Error Close_trade(Trade _trade)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE stock_values SET `calculations` = @calculations WHERE id = @id", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", 1);
                cmd.Parameters.AddWithValue("@calculations", "{}");

                foreach (Bid b in _bids)
                {
                    cmd.Parameters["@id"].Value = b.Id;
                    cmd.Parameters["@calculations"].Value = b.Calculations;
                    cmd.ExecuteNonQuery();
                }

                this.Close();
                return new Error(false, "bids calculations updated");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }*/

    }
}
