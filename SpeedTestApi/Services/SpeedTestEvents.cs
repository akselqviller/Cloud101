using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using SpeedTestApi.Models;

namespace SpeedTestApi.Services
{
    public class SpeedTestEvents : ISpeedTestEvents, IDisposable
    {
        private readonly EventHubClient client;

        public SpeedTestEvents(string connectionString, string entityPath)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                EntityPath = entityPath
            };
            
            client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public async Task PublishSpeedTest(TestResult speedTest)
        {
            var message = JsonConvert.SerializeObject(speedTest);
            var data = new EventData(Encoding.UTF8.GetBytes(message));
            
            await client.SendAsync(data);
        }

        public void Dispose()
        {
            client.CloseAsync();
        }
    }
}