using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingApplication.DataObjects
{
    public class Log
    {
        public string application_id { get; set; }

        public string logger { get; set; }

        public string level { get; set; }

        public string message { get; set; }
    }
}
