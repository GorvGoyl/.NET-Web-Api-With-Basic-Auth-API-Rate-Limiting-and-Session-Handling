using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoggingApplication.API.Models
{
    public class LogResponseModel
    {
        public bool success { get; set; }

        public LogResponseModel(bool state)
        {
            success = state;
        }
    }
}