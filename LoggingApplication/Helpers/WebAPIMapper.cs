using LoggingApplication.API.Models;
using LoggingApplication.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoggingApplication.API.Helper
{
    public static class WebAPIMapper
    {
        public static ApplicationResponseModel ApplicationObjMapper(Application application)
        {
            try
            {
                if (application != null)
                {
                    return new ApplicationResponseModel
                    {
                        application_id = application.application_id,
                        display_name = application.display_name,
                        application_secret = application.application_secret
                    };
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
        public static Log LogObjMapper(LogRequestModel model)
        {
            try
            {
                if (model != null)
                {
                    return new Log
                    {
                        application_id = model.application_id,
                       level = model.level,
                       logger = model.logger,
                       message = model.message
                    };
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

    }
}