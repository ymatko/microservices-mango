namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public interface IRabbitMQAuthMessageSender
    {
        void SendMessaga(Object message, string queueName);
    }
}
