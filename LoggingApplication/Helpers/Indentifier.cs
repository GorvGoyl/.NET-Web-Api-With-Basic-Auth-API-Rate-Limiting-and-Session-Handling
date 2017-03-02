using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoggingApplication.API.Helpers
{
    public static class Identifier
    {
        public static readonly string SESSION_LIFETIME = "LA_SESSION_LEFETIME_";
        public static readonly string SESSION_IDENTIFIER = "LA_SESSION_";
        public static readonly string THROTTLE_BASE_IDENTIFIER = "LA_THROTTLE_BASE_";
        public static readonly string TOKEN_IDENTIFIER = "LA_TOKEN_";
        public static readonly string THROTTLE_COUNTER_IDENTIFIER = "LA_THROTTLE_COUNT_";
    }
}