using LoggingApplication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingApplication.DataObjects;
using LoggingApplication.Repository.Interfaces;
using LoggingApplication.Repository.Repositories;
using LoggingApplication.Core.Helpers;

namespace LoggingApplication.Core.Services
{
    public class LogService : ILogService
    {
        private IMySqlRepository _repository;
        public LogService()
        {
            _repository = new MySqlRepository();
        }

        public async Task<bool> InsertLogAsync(Log log)
        {
            return await Task.Run(() => InsertLog(log));

        }

        public Application Register(string display_name)
        {
            bool IsRowCreated = false;

            Application application = new Application();
            try
            {
                application.application_id = Guid.NewGuid().ToString("n").Substring(0, 32);
                application.application_secret = Guid.NewGuid().ToString("n").Substring(0, 32);
                application.display_name = display_name;
                if (_repository.Create(application.display_name,application.application_id, Hasher.GetStringSha256Hash(application.application_secret)))
                {
                   IsRowCreated = true;
                }

            }
            
            catch (Exception)
            {

            }

            return IsRowCreated?application:null;

        }

        public int GetSessionLifetime()
        {
            int lifetime = 0;

            try
            {
                lifetime = _repository.RetrieveSessionLifetime();
            }
           
            catch (Exception)
            {

            }

            return lifetime;

        }

        public bool IsAppExist(string name)
        {
            bool isExist = false;

            try
            {
                isExist = _repository.IsAppExist(name);
            }

            catch (Exception)
            {

                throw;
            }

            return isExist;
        }

        public bool AuthenticateApp(string app_id, string app_secret)
        {
            bool isExist = false;

            try
            {
                isExist = _repository.AuthenticateApp(app_id, Hasher.GetStringSha256Hash(app_secret));
            }
           
            catch (Exception)
            {

                throw;
            }

            return isExist;
        }

        private bool InsertLog(Log log)
        {
            bool isCreated = false;

            try
            {
                 isCreated = _repository.InsertLog(log);
            }
            catch (Exception)
            {

                throw;
            }

            return isCreated;
        }

    }
}
