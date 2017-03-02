using LoggingApplication.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace LoggingApplication.API.Helpers
{
    public class SessionHandler
    {
        /*
         
         Two cache objects are used to maintain session.
         1) identifier1 + app_id : token
         2) identifier2 + token : app_id

            sessionkey = identifier1 + app_id
            tokenkey = identifier2 + token

        One cache object is used to maintain rate limiting.
        1) identifier3 + app_id : count
        
            throttlekey = identifier3 + app_id

        */
        readonly object _syncLock = new object();
        public bool CheckSessionById(string app_id)
        {
            bool isSessionExist = false;
            string sessionKey = GetSessioncacheKey(app_id);
            if (HttpRuntime.Cache[sessionKey] != null)
            {
                isSessionExist = true;
            }

            return isSessionExist;
        }

        public string GetNewTokenFromSession(string app_id)
        {
            string newToken = string.Empty;
            try
            {
                newToken = Guid.NewGuid().ToString();
                string sessionCacheKey = GetSessioncacheKey(app_id);
                string oldToken = HttpRuntime.Cache[sessionCacheKey].ToString();
                string tokenOldCacheKey = GetTokenCacheKey(oldToken);
                string tokenNewCacheKey = GetTokenCacheKey(newToken);

                // remove old token key from cache
                HttpRuntime.Cache.Remove(tokenOldCacheKey);

                // add new token key to cache
                HttpRuntime.Cache.Add(tokenNewCacheKey,
                   app_id,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);

                // update token in session key
                HttpRuntime.Cache.Insert(sessionCacheKey,
                   newToken,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);

            }
            catch (Exception)
            {
            }

            return newToken;
        }

       public string CreateSession(string app_id)
        {

            string token = string.Empty;
            try
            {
                token = Guid.NewGuid().ToString();
                string sessionCacheKey = GetSessioncacheKey(app_id);
                string tokenCacheKey = GetTokenCacheKey(token);

                // create cache with sessionkey:token
                HttpRuntime.Cache.Add(sessionCacheKey,
                   token,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);

                // create cache with tokenkey:app_id
                HttpRuntime.Cache.Add(tokenCacheKey,
                   app_id,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);

            }
            catch (Exception)
            {
            }

            return token;
        }

        public bool CheckSessionByToken(string token)
        {
            bool isSessionExist = false;
            string tokenKey =  GetTokenCacheKey(token);
            if (HttpRuntime.Cache[tokenKey] != null)
            {
                isSessionExist = true;
            }

            return isSessionExist;
        }

        public void ExtendSession(string token)
        {
            string tokenCacheKey = GetTokenCacheKey(token);
            try
            {
                string app_id = HttpRuntime.Cache[tokenCacheKey].ToString();
                string sessionCacheKey = GetSessioncacheKey(app_id);

                // extend time for cache with sessionkey:token
                HttpRuntime.Cache.Add(sessionCacheKey,
                   token,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);

                // extend time for cache with tokenkey:app_id
                HttpRuntime.Cache.Add(tokenCacheKey,
                   app_id,
                   null,
                   DateTime.Now.AddMinutes(SessionLifetime_mins()),
                   Cache.NoSlidingExpiration,
                   CacheItemPriority.High,
                   null);


            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string GetSessioncacheKey(string app_id)
        {
            return Identifier.SESSION_IDENTIFIER + app_id;
        }

        public static string GetTokenCacheKey(string token)
        {
            return Identifier.SESSION_IDENTIFIER + token;
        }

        public static string GetAppIdFromToken(string token)
        {
            string app_id = string.Empty;

            try
            {
                string tokenCacheKey = GetTokenCacheKey(token);
                app_id = HttpRuntime.Cache[tokenCacheKey].ToString();
            }
            catch (Exception)
            {

                //throw;
            }

            return app_id;

        }

        public int SessionLifetime_mins()
        {
           
            int lifetime_mins = 0;

            if (HttpRuntime.Cache[Identifier.SESSION_LIFETIME] != null)
            {
                lifetime_mins = (int)HttpRuntime.Cache[Identifier.SESSION_LIFETIME];
            }
            else
            {
                lock (_syncLock)
                {
                    if (HttpRuntime.Cache[Identifier.SESSION_LIFETIME] == null)
                    {
                        HttpRuntime.Cache[Identifier.SESSION_LIFETIME] = new Core.Services.LogService().GetSessionLifetime();
                    }

                    lifetime_mins = (int)HttpRuntime.Cache[Identifier.SESSION_LIFETIME];
                }
            }
                return lifetime_mins;
            
        }

    }
}