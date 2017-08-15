using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics.Commands
{
    public partial class Commands
    {
        public static Error Get_xtb_server_time(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, ref DateTime _xtbServerTime)
        {
            return Tool.Get_xtb_server_time(ref Xtb_api_connector, ref _xtbServerTime);
        }
        
    }
}
