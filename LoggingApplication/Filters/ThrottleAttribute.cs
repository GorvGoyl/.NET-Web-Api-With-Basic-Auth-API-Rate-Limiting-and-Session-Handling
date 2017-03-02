using LoggingApplication.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LoggingApplication.API.Filters
{
    public class ThrottleAttribute : ActionFilterAttribute
    {
        private int _API_RATEQUOTA = 60;

        // per x minute value
        private int _API_TIMELIMIT = 1;
        
        private int _API_BLOCKDURATION = 5;

        private readonly object syncLock = new object();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var access_token = AuthHelper.GetAuthToken(actionContext.Request);

            if (access_token != null)
            {
                string app_id = SessionHandler.GetAppIdFromToken(access_token);

                string throttleBaseKey = GetThrottleBaseKey(app_id);
                string throttleCounterKey = GetThrottleCounterKey(app_id);

                lock (syncLock)
                {
                    //add a listner for request count
                    if (HttpRuntime.Cache[throttleBaseKey] == null)
                    {
                        HttpRuntime.Cache.Add(throttleBaseKey,
                            DateTime.UtcNow,
                            null,
                            DateTime.Now.AddMinutes(_API_TIMELIMIT),
                            Cache.NoSlidingExpiration,
                            CacheItemPriority.High,
                            null);

                        HttpRuntime.Cache.Add(throttleCounterKey,
                           1,
                           null,
                           DateTime.Now.AddMinutes(_API_TIMELIMIT),
                           Cache.NoSlidingExpiration,
                           CacheItemPriority.High,
                           null);
                    }
                    else
                    {
                        //listener exists for request count
                        var current_requests = (int)HttpRuntime.Cache[throttleCounterKey];

                        if (current_requests < _API_RATEQUOTA)
                        {
                           HttpRuntime.Cache.Insert(throttleCounterKey,
                           current_requests + 1,
                           null,
                           DateTime.Now.AddMinutes(_API_TIMELIMIT),
                           Cache.NoSlidingExpiration,
                           CacheItemPriority.High,
                           null);
                        }

                        //hit rate limit, wait for another 5 minutes 
                        else
                        {
                            HttpRuntime.Cache.Insert(throttleBaseKey,
                           DateTime.UtcNow,
                           null,
                           DateTime.Now.AddMinutes(_API_BLOCKDURATION),
                           Cache.NoSlidingExpiration,
                           CacheItemPriority.High,
                           null);

                            HttpRuntime.Cache.Insert(throttleCounterKey,
                          current_requests + 1,
                          null,
                          DateTime.Now.AddMinutes(_API_BLOCKDURATION),
                          Cache.NoSlidingExpiration,
                          CacheItemPriority.High,
                          null);

                            Forbidden(actionContext);
                        }
                    }
                }
            }
            else
            {
                BadRequest(actionContext);
            }

            base.OnActionExecuting(actionContext);
        }

        private string GetThrottleBaseKey(string app_id)
        {
            return Identifier.THROTTLE_BASE_IDENTIFIER + app_id;
        }

        private string GetThrottleCounterKey(string app_id)
        {
            return Identifier.THROTTLE_COUNTER_IDENTIFIER + app_id;
        }

        private void BadRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private void Forbidden(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Application Rate Limit Exceeded");
        }

    }
}