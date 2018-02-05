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
        public string Xtb_login { get; set; }
        public string Xtb_pwd { get; set; }
        public Server Xtb_server { get; set; }

        public Configuration(string _xtb_login, string _xtb_pwd, string _type)
        {
            this.Xtb_login = _xtb_login;
            this.Xtb_pwd = _xtb_pwd;
            Set_server(_type);
        }

        public void Set_server(string type)
        {
            if (type == "real")
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