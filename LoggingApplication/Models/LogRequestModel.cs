using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LoggingApplication.API.Models
{
    public class LogRequestModel
    {
        [Required]
        [StringLength(32,MinimumLength =1)]
        public string application_id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string logger { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string level { get; set; }

        [Required]
        [StringLength(2048, MinimumLength = 1)]
        public string message { get; set; }
    }
}