using LoggingApplication.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingApplication.Repository.Interfaces
{
    public interface IMySqlRepository
    {
        bool Create(string display_name, string app_id, string key_hash);

        int RetrieveSessionLifetime();

        bool AuthenticateApp(string app_id, string key_hash);

        bool IsAppExist(string name);

        bool InsertLog(Log log);

    }
}
