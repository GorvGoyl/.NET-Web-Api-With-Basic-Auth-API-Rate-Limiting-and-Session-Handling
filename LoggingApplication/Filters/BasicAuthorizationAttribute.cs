using LoggingApplication.API.Helper;
using LoggingApplication.API.Helpers;
using LoggingApplication.Core.Interfaces;
using LoggingApplication.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LoggingApplication.API.Filters
{
    public class BasicAuthorizationAttribute : AuthorizationFilterAttribute
    {

        public BasicAuthorizationAttribute()
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            SessionHandler sessionHandler = new SessionHandler();
            string accessToken = AuthHelper.GetAuthToken(actionContext.Request);
            if (accessToken != null)
            {
                if (sessionHandler.CheckSessionByToken(accessToken))
                {
                    sessionHandler.ExtendSession(accessToken);
                }
                else
                {
                    Forbidden(actionContext);
                }
            }
            else
            {
                BadRequest(actionContext);
            }

            base.OnAuthorization(actionContext);
        }


        private void Forbidden(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid access token");
        }

        private void BadRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}