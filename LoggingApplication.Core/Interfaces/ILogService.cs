using LoggingApplication.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingApplication.Core.Interfaces
{
    public interface ILogService
    {
        Application Register(string display_name);
        //AuthResponse Authorize(string app_id, string app_secret);
        int GetSessionLifetime();

        bool IsAppExist(string name);

        bool AuthenticateApp(string app_id, string app_secret);

        Task<bool> InsertLogAsync(Log log);
    }
}
