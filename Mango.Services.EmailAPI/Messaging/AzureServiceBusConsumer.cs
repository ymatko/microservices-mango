using Azure.Messaging.ServiceBus;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer
    {
        private readonly string serviceBuConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailBusProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBuConnectionString = _configuration.GetValue<string>("SrviceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBuConnectionString);
            _emailBusProcessor = client.CreateProcessor(emailCartQueue);
        }
    }
}
