using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace LoggingApplication.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.AddApiVersioning();

            //enable versioning via url path
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                }
            };

            config.MapHttpAttributeRoutes(constraintResolver);

        }
    }
}
