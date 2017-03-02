using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoggingApplication.DataObjects;
namespace LoggingApplication.API.Models
{
    public class ApplicationResponseModel
    {
        public string application_id { get; set; }

        public string application_secret { get; set; }

        public string display_name { get; set; }

        //public Application Application { get; set; }

        //public bool Success { get; set; }

        //public Exception OperationException { get; set; }
    }
}