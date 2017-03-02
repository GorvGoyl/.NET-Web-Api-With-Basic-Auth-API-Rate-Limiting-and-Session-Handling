using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using LoggingApplication.Core.Interfaces;
using LoggingApplication.Core.Services;
using LoggingApplication.API.Models;
using LoggingApplication.API.Helper;
using Microsoft.Web.Http;
using LoggingApplication.API.Filters;

namespace LoggingApplication.API.Controllers
{

    [ApiVersion("1.0")]
    [ValidateViewModel]
    public class RegisterController : ApiController
    {
        private readonly ILogService _logService;

        public RegisterController()
        {
            _logService = new LogService();
        }

        [HttpPost]
        [Route("v{version:apiVersion}/register")]
        [Route("register")]
        public IHttpActionResult Post([FromBody]RegisterRequestModel application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("'display_name' should not be empty and should not exceed 32 characters.");
            }

            if (_logService.IsAppExist(application.display_name))
            {
                return BadRequest("display_name already exists.");
            }

            ApplicationResponseModel applicationResponse = WebAPIMapper.ApplicationObjMapper( _logService.Register(application.display_name));
            if (applicationResponse!=null)
            {
                return Ok(applicationResponse);
            }
            else
            {
                return InternalServerError();
            }
        }

    }
}
