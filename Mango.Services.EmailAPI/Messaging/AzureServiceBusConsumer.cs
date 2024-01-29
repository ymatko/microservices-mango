using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBuConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly string orderCreated_Topic;
        private readonly string orderCreated_Email_Subscription;

        private ServiceBusProcessor _emailOrderPlacedProcessor;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            serviceBuConnectionString = _configuration.GetValue<string>("SrviceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUsertQueue");
            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

            var client = new ServiceBusClient(serviceBuConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerUserProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();
        }


        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }
        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage email = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
