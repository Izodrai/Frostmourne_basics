using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        
        public Error Load_not_inactive_symbols(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "v_symbols_not_inactive");
        }
        
        public Error Load_active_symbols(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "v_symbols_active");
        }
        
        public Error Load_inactive_symbols(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "v_symbols_inactive");
        }
        
        public Error Load_simulation_symbols(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "v_symbols_simulation");
        }
        
        public Error Load_standby_symbols(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "v_symbols_standby");
        }

        public Error Load_all_symbols_status(ref List<Symbol> _sl)
        {
            return Load_symbols_state(ref _sl, "symbols");
        }

        protected Error Load_symbols_state(ref List<Symbol> _sl, string table)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;
            
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description, state, lot_max_size, lot_min_size FROM " + table, this.Mysql_connector);

                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _sl.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1])));
                    }
                }
                this.Close();
                return new Error(false, "Symbols loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
        
        public Error Load_symbol_state(ref Symbol _s)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference FROM symbols WHERE id = @id", this.Mysql_connector);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("id", _s.Id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _s.Id = Convert.ToInt32(values[0]);
                        _s.Name = Convert.ToString(values[1]);
                    }
                }
                this.Close();
                return new Error(false, "Symbol loaded");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Close();
                return new Error(true, ex.Message);
            }
        }
    }
}
