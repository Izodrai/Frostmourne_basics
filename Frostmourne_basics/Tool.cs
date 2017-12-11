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
        public static Error InitXtb(ref Configuration configuration, ref SyncAPIConnector Xtb_api_connector)
        {
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
                return new Error(false, "Init success");
            }
            catch (Exception e)
            {
                return new Error(true, "Error during ExecuteLoginCommand : " + e.Message);
            }
        }

        public static Error CloseXtb(ref SyncAPIConnector Xtb_api_connector)
        {
            try
            {
                APICommandFactory.ExecuteLogoutCommand(Xtb_api_connector);
                return new Error(false, "Logout success");
            }
            catch (Exception e)
            {
                return new Error(true, "Error during ExecuteLogoutCommand : " + e.Message);
            }
        }
        
    }
}
