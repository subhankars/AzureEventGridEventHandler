// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGridExtensionConfig?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using EventGridEventTrigger.Library;

namespace EventGridEventTrigger.AzureFunctionAppV2
{
    public static class EventGridEventHandler
    {
        [FunctionName("EventGridEventHandler")]
        public static void Run([EventGridTrigger] Microsoft.Azure.EventGrid.Models.EventGridEvent eventGridEvent, TraceWriter log)
        {
            log.Info(eventGridEvent.Data.ToString());
            var eventData = eventGridEvent.Data as CustomData;
            var customEvent = CustomEvent<CustomData>.CreateCustomEvent(eventData);
        }
    }
}
