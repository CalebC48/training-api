using CAP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        /// <summary>
        ///     This is the default controller for the CAP API. It returns a nice poem.
        /// </summary>
        /// <returns>A nice poem</returns>
        /// <response code="200">Returns a nice string</response> 
        [HttpGet]
        [Produces("text/plain")]
        public string Get()
        {
            try
            {
                return "A Poem About This Page\n" +
                        "...Then this white page beguiling my sad fancy into smiling\n" +
                        "By the plain and simple decorum of the coutenance it wore\n" +
                        "\"Though thy font be bland and shaven, thou,\" I said, \"art sure no craven\",\n" +
                        "Simply text and error code showing, not a single whit more-\n" +
                        "Tell me what thy lordly name is, I swear I found this before!\"\n" +
                        "Quoth the Server \"404\"\n";
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                return e.Message;
            }
        }
    }
}
