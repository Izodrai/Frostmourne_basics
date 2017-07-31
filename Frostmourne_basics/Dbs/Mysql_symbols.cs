using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        public Error Load_data_retrieve_symbols(ref List<Symbol> _ss)
        {
            return Load_state_symbols(ref _ss, "v_symbols_not_inactive");
        }
        
        public Error Load_active_symbols(ref List<Symbol> _ss)
        {
            return Load_state_symbols(ref _ss, "v_symbols_active");
        }
        
        public Error Load_inactive_symbols(ref List<Symbol> _ss)
        {
            return Load_state_symbols(ref _ss, "v_symbols_inactive");
        }
        
        public Error Load_simulation_symbols(ref List<Symbol> _ss)
        {
            return Load_state_symbols(ref _ss, "v_symbols_simulation");
        }
        
        public Error Load_standby_symbols(ref List<Symbol> _ss)
        {
            return Load_state_symbols(ref _ss, "v_symbols_standby");
        }
        
        protected Error Load_state_symbols(ref List<Symbol> _ss, string table)
        {
            Error err;

            err = this.Connect();
            if (err.IsAnError)
                return err;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, reference, description, state FROM " + table, this.Mysql_connector);

                cmd.Parameters.Clear();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        _ss.Add(new Symbol(Convert.ToInt32(values[0]), Convert.ToString(values[1]), Convert.ToString(values[2]), Convert.ToString(values[3])));
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
    }
}
