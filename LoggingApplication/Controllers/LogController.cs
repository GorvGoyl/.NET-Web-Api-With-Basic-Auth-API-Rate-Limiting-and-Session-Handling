using LoggingApplication.API.Filters;
using LoggingApplication.API.Helper;
using LoggingApplication.API.Models;
using LoggingApplication.Core.Interfaces;
using LoggingApplication.Core.Services;
using LoggingApplication.DataObjects;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LoggingApplication.API.Controllers
{
    [ApiVersion("1.0")]
    [BasicAuthorization]
    [ValidateViewModel]
    public class LogController : ApiController
    {
        private readonly ILogService _logService;

        public LogController()
        {
            _logService = new LogService();
        }

        [Route("v{version:apiVersion}/log")]
        [Route("log")]
        [HttpPost]
        [Throttle]
        public async Task<IHttpActionResult> Log([FromBody]LogRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Log log = WebAPIMapper.LogObjMapper(model);
            bool isCreated = await _logService.InsertLogAsync(log);

            var response = new LogResponseModel(isCreated);
            return Ok(response);
        }



    }
}