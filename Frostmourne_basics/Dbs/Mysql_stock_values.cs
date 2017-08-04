using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        public Error Load_bids_values_for_symbol_between_to_date(ref List<Bid> _bids, DateTime _from_d, DateTime _to_d, Symbol _symbol)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, bid_at, last_bid, calculations FROM stock_values WHERE symbol_id = @symbol_id AND bid_at >= @from AND bid_at <= @to", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol.Id);
                cmd.Parameters.AddWithValue("from", _from_d.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("to", _to_d.ToString("yyyy-MM-dd HH:mm:ss"));

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        _bids.Add(new Bid(Convert.ToInt32(values[0]), _symbol, DateTime.Parse(Convert.ToString(values[1])), Convert.ToDouble(values[2]), Convert.ToString(values[3])));
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
        
        public Error Load_last_value_for_symbol(ref Bid _bid, Symbol _symbol)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, bid_at, last_bid, calculations FROM stock_values WHERE symbol_id = @symbol_id AND bid_at = (SELECT MAX(bid_at) FROM stock_values WHERE symbol_id = @symbol_id)", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol.Id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        _bid = new Bid(Convert.ToInt32(values[0]), _symbol, DateTime.Parse(Convert.ToString(values[1])), Convert.ToDouble(values[2]), Convert.ToString(values[3]));
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
        
        public Error Count_value_for_symbol(ref int _ct, Symbol _symbol)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM stock_values WHERE symbol_id = @symbol_id", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol.Id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        _ct = Convert.ToInt32(values[0]);
                    }

                }
                this.Close();
                return new Error(false, "counted");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        public Error Count_value_by_day_for_symbol(Symbol _symbol, DateTime _from, DateTime _to, ref List<Bid_by_date> bids_ct)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*), DATE(bid_at) FROM stock_values WHERE symbol_id = @symbol_id AND DATE(bid_at) BETWEEN @from AND @to GROUP BY DATE(bid_at)", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("symbol_id", _symbol.Id);
                cmd.Parameters.AddWithValue("from", _from.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("to", _to.ToString("yyyy-MM-dd HH:mm:ss"));
            
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        bids_ct.Add(new Bid_by_date(DateTime.Parse(Convert.ToString(values[1])), Convert.ToInt32(values[0])));
                    }
                }
                this.Close();
                return new Error(false, "counted");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
        
        public Error Insert_or_update_bids_values(List<Bid> _bids)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO stock_values (`symbol_id`, `bid_at`, `last_bid`, `calculations`) VALUES (@symbol_id, @bid_at, @last_bid, @calculations) ON DUPLICATE KEY UPDATE `last_bid`= @last_bid, `calculations` = @calculations", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@symbol_id", 1);
                cmd.Parameters.AddWithValue("@last_bid", 1);
                cmd.Parameters.AddWithValue("@bid_at", "One");
                cmd.Parameters.AddWithValue("@calculations", "[]");

                foreach (Bid b in _bids)
                {
                    cmd.Parameters["@symbol_id"].Value = b.Symbol.Id;
                    cmd.Parameters["@last_bid"].Value = b.Last_bid;
                    cmd.Parameters["@bid_at"].Value = b.Bid_at.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters["@calculations"].Value = b.Calculations;
                    cmd.ExecuteNonQuery();
                }

                this.Close();
                return new Error(false, "bids inserted or updated");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }

        public Error Update_bid_calculations(List<Bid> _bids)
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
                cmd.Parameters.AddWithValue("@calculations", "[]");

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
        }

    }
}
