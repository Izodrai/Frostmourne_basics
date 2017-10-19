using MySql.Data.MySqlClient;

namespace Frostmourne_basics.Dbs
{
    public partial class Mysql
    {
        protected MySqlConnection Mysql_connector { get; set; }

        protected string Server { get; set; }

        protected string Database { get; set; }

        protected string Login { get; set; }

        protected string Pwd { get; set; }

        protected string Port { get; set; }

        public Mysql() { }
        
        public Mysql(string _server, string _port, string _database, string _login, string _password)
        {
            this.Server = _server;
            this.Database = _database;
            this.Login = _login;
            this.Pwd = _password;
            this.Port = _port;
        }

        public Error Connect()
        {
            try
            {
                this.Mysql_connector = new MySqlConnection("server=" + this.Server + ";port= " + this.Port + " ;database = " + this.Database + "; user id = " + this.Login + "; password = " + this.Pwd + ";Pooling=False");
                this.Mysql_connector.Open();

                if (!this.Mysql_connector.Ping())
                    return new Error(true, "Cannot ping database");

                return new Error(false, "Mysql Database connected");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return new Error(true, ex.Message);
            }
        }

        public void Close()
        {
            this.Mysql_connector.Close();
        }

    }
}
