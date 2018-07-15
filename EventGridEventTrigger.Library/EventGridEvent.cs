using Newtonsoft.Json;
using System;

namespace EventGridEventTrigger.Library
{
    public class EventGridEvent
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "ËventTime")]
        public DateTime EventTime { get; set; }
        public object Data { get; set; }
    }
}
