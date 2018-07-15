using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EventGridEventTrigger.Library;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventGridEventFunctionApp
{
    public static class EventGridEventHandler
    {
        [FunctionName("EventGridEventHandler")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage request, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

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
