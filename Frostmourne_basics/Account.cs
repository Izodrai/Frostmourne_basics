using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Account
    {
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string Token { get; set; }

        public Account() {}

        public Account(string _xtb_login, string _xtb_pwd, string _token)
        {
            this.Login = _xtb_login;
            this.Pwd = _xtb_pwd;
            this.Token = _token;
        }
    }
}
