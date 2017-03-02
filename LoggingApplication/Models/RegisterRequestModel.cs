using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoggingApplication.API.Models
{
    public class RegisterRequestModel
    {
        [Required]
        [StringLength(32, MinimumLength = 1)]
        public string display_name { get; set; }
    }
}