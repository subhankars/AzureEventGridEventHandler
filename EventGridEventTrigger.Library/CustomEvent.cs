using System;

namespace EventGridEventTrigger.Library
{
    public static class CustomEvent<T>
    {
        public static EventGridEvent CreateCustomEvent(T obj)
        {
            return new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow,
                EventType = "MyCustomEventType",
                Subject = "MyCustomEventSubject",
                Data = obj
            };
        }
    }

    public class CustomData
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
    }
}
