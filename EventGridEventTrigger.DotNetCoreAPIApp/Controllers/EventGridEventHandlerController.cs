using EventGridEventTrigger.Library;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace EventGridEventTrigger.DotNetCoreAPIApp.Controllers
{
    [Produces("application/json")]
    public class EventGridEventHandlerController : Controller
    {
        [HttpPost]
        [Route("api/EventGridEventHandler")]
        public JObject Post([FromBody]object request)
        {
            var requestMessageContent = JsonConvert.SerializeObject(request);
            var eventGridEvent = JsonConvert.DeserializeObject<EventGridEvent[]>(requestMessageContent)
                .FirstOrDefault();
            var data = eventGridEvent.Data as JObject;

            // Validate whether EventType is of "Microsoft.EventGrid.SubscriptionValidationEvent"
            if (string.Equals(eventGridEvent.EventType, Constants.SubscriptionValidationEvent, StringComparison.OrdinalIgnoreCase))
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
                var eventData = data.ToObject<CustomData>();
                var customEvent = CustomEvent<CustomData>.CreateCustomEvent(eventData);
                return JObject.FromObject(customEvent);
            }

            return new JObject(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}