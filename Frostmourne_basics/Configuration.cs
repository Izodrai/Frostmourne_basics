using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Configuration
    {
        public string Environnement { get; set; }
        public bool Prod { get; set; }

        public string Xtb_login { get; set; }
        public string Xtb_pwd { get; set; }
        public Server Xtb_server { get; set; }

        public string Mysql_host { get; set; }
        public string Mysql_login { get; set; }
        public string Mysql_pwd { get; set; }
        public string Mysql_database { get; set; }
        public string Mysql_port { get; set; }

        public double Stop_loss { get; set; }

        public void Set_server(string type)
        {
            if (type == "prod")
            {
                this.Xtb_server = Servers.REAL;
            }
                else
            {
                this.Xtb_server = Servers.DEMO;
            }
        }
    }
}
