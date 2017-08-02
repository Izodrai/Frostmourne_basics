using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Sync;

namespace Frostmourne_basics
{
    public class Tool
    {

        public static Error InitAll(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB)
        {
            Error err;
            Credentials Credentials;
            
            //////////////////////////////////////////////
            //
            // Test de connexion aux serveurs xtb
            //
            //////////////////////////////////////////////

            Xtb_api_connector = new SyncAPIConnector(configuration.Xtb_server);


            //////////////////////////////////////////////
            //
            // Renseignement des Credentials 
            //   
            //////////////////////////////////////////////

            Credentials = new Credentials(configuration.Xtb_login, configuration.Xtb_pwd);

            //////////////////////////////////////////////
            //
            // Tentative d'authentification au serveur XTB
            //
            //////////////////////////////////////////////
            
            try
            {
                APICommandFactory.ExecuteLoginCommand(Xtb_api_connector, Credentials);
            } catch (Exception e)
            {
                return new Error(true, "Error during ExecuteLoginCommand : " + e.Message);
            }
            
            //////////////////////////////////////////////
            //
            // Tentative d'authentification au serveur Atiesh
            //
            //////////////////////////////////////////////

            MyDB = new Mysql(configuration.Mysql_host, configuration.Mysql_port, configuration.Mysql_database, configuration.Mysql_login, configuration.Mysql_pwd);

            err = MyDB.Connect();
            if (err.IsAnError)
            {
                return err;
            }
            MyDB.Close();
            
            Log.GreenInfo("XTB and DB server connected");
            Log.JumpLine();

            return new Error(false, "Init success");
        }

        public static Error Get_xtb_server_time(ref SyncAPIConnector Xtb_api_connector, ref Configuration configuration, ref Mysql MyDB, ref DateTime _xtbServerTime)
        {
            ServerTimeResponse serverTimeResponse = APICommandFactory.ExecuteServerTimeCommand(Xtb_api_connector, true);

            _xtbServerTime = DateTime.Parse(serverTimeResponse.TimeString);
            
            return new Error(false, "");
        }

        public static Error Cast_xtb_server_time_to_utc(ref DateTime _xtbServerTime)
        {
            DateTime tNow = DateTime.UtcNow;
            
            int diff = tNow.Hour - _xtbServerTime.Hour;
            
            _xtbServerTime = _xtbServerTime.AddHours(diff);
            
            return new Error(false, "");
        }

        public static long DateTimeToLongUnixTimeStamp(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double timeStamp = (date.ToUniversalTime() - epoch).TotalSeconds;
            return Convert.ToInt64(timeStamp) * 1000;
        }

        public static DateTime LongUnixTimeStampToDateTime(long? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(unixTimeStamp / 1000)).ToUniversalTime();
            return dtDateTime;
        }
    }
}
