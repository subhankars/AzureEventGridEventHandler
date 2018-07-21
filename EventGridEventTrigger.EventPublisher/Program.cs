using EventGridEventTrigger.Library;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventGridEventTrigger.EventPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var newEvent = PublishEventGridEvent();
            newEvent.Wait();
            Console.WriteLine(newEvent.Result.Content.ReadAsStreamAsync().Result);
        }

        private static async Task<HttpResponseMessage> PublishEventGridEvent()
        {
            var eventTopicEndpoint = "{event topic endpoint to be taken from Azure Portal}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "sas key for event grid topic");
            var customData = new CustomData() { Name = "John Doe" , Age = 32, Height = 176, Weight = 76};
            var customEventData = CustomEvent<CustomData>.CreateCustomEvent(customData);
            var jsonContent = JsonConvert.SerializeObject(customEventData);
            var httpRequestContent = new StringContent("[" + jsonContent + "]", Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(eventTopicEndpoint, httpRequestContent);
        }
    }
}
