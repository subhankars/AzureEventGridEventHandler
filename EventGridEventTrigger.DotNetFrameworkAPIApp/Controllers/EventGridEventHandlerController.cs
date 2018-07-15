using EventGridEventTrigger.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EventGridEventTriggerNetFrameworkAPIApp.Controllers
{
    public class EventGridEventHandlerController : ApiController
    {
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("api/EventGridEventHandler")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            var requestMessageContent = await request.Content.ReadAsStringAsync();
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
                    return request.CreateResponse(HttpStatusCode.OK, responseData);
                }
            }
            else
            {
                // Handle your custom event
                var eventData = data.ToObject<CustomData>();
                var customEvent = CustomEvent<CustomData>.CreateCustomEvent(eventData);
                return request.CreateResponse(HttpStatusCode.OK, customEvent);
            }

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
