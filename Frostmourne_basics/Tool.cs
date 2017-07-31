using Frostmourne_basics.Dbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands;
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

    }
}
