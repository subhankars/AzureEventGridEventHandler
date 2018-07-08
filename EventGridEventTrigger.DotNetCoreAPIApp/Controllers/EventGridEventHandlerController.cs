using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [Produces("application/json")]
    [Route("api/EventGridEventHandler")]
    public class EventGridEventHandlerController : Controller
    {
    }
}