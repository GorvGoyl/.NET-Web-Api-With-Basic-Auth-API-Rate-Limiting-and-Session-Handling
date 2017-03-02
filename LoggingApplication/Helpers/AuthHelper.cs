using LoggingApplication.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace LoggingApplication.API.Helpers
{
    public static class AuthHelper
    {
        public static string GetAuthToken(HttpRequestMessage request)
        {
            if (request.Headers.Authorization != null &&
                request.Headers.Authorization.Parameter != null)
            {
                string authToken = request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                return decodedToken;
            }

            return null;
        }

        public static bool AuthenticateApp(string app_id, string app_secret)
        {
            bool isExist = false;

            isExist = new Core.Services.LogService().AuthenticateApp(app_id, app_secret);

            return isExist;
        }

    }
}