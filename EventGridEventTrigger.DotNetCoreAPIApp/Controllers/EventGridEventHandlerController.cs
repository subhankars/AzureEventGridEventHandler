using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventGridEventTrigger.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [Produces("application/json")]
    public class EventGridEventHandlerController : Controller
    {
        private const string subscriptionValidationEvent = "Microsoft.EventGrid.SubscriptionValidationEvent";

        [HttpPost]
        [Route("api/EventGridEventHandler")]
        public JObject Post([FromBody]object request)
        {
            var response = string.Empty;
            var requestMessageContent = JsonConvert.SerializeObject(request);
            var eventGridEvent = JsonConvert.DeserializeObject<EventGridEvent[]>(requestMessageContent)
                .FirstOrDefault();
            var data = eventGridEvent.Data as JObject;

            // Validate whether EventType is of SubscriptionValidationEvent type
            if (string.Equals(eventGridEvent.EventType, subscriptionValidationEvent, StringComparison.OrdinalIgnoreCase))
            {
                var eventData = data.ToObject<SubscriptionValidationEventData>();
                var responseData = new SubscriptionValidationResponseData
                {
                    ValidationResponse = eventData.ValidationCode
                };

                if (responseData.ValidationResponse != null)
                {
                    return JObject.FromObject(responseData);
                }
            }
            else
            {
                // Handle your custom event
            }

            return new JObject(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}