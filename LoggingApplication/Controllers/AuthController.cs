using LoggingApplication.Core.Interfaces;
using LoggingApplication.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using LoggingApplication.DataObjects;
using LoggingApplication.API.Models;
using LoggingApplication.API.Helper;
using LoggingApplication.API.Helpers;
using Microsoft.Web.Http;

namespace LoggingApplication.API.Controllers
{
    [ApiVersion("1.0")]
    public class AuthController : ApiController
    {
        private readonly ILogService _logService;
        private readonly SessionHandler _sessionHandler;
        public AuthController()
        {
            _logService = new LogService();
            _sessionHandler = new SessionHandler();
        }

        [Route("v{version:apiVersion}/auth")]
        [Route("auth")]
        [HttpPost]
        public IHttpActionResult Post()
        {
            string authToken = AuthHelper.GetAuthToken(Request);
            if (authToken!=null && authToken.IndexOf(":") != -1)
            {

                string app_id = authToken.Substring(0, authToken.IndexOf(":"));
                string app_secret = authToken.Substring(authToken.IndexOf(":") + 1);
                string access_token = null;

                //check if app_id exists in any active session
                if (_sessionHandler.CheckSessionById(app_id))
                {
                    // Preserves the session and replace old token with new one.
                    access_token = _sessionHandler.GetNewTokenFromSession(app_id);

                }

                // validate app_id and app_secret from dB
                else if (AuthHelper.AuthenticateApp(app_id,app_secret))
                {
                    // create a new session for app_id
                    access_token = _sessionHandler.CreateSession(app_id);
                }
                else
                {
                    return Unauthorized();
                }

                if (string.IsNullOrWhiteSpace(access_token))
                {
                    return InternalServerError( new Exception("Error generating access token."));
                }
                return Ok(new AuthResponseModel(access_token));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}