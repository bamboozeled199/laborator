using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Laborator6
{
    class Program
    {
        const string ServiceBusConnectionString="Endpoint=sb://laborator6cioaraemanuel.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZTHbpbKQ3U2px+mkapVRAenz+o5yCAM/wecbBMG+PI4=";
        const string QueueName="coadalab6";
        static IQueueClient queueClient;
        public static async Task Main(string[] args)
        {
            const int numberOfMessages=10;
            queueClient=new QueueClient(ServiceBusConnectionString,QueueName);

            Console.WriteLine("=============================================");
            Console.WriteLine("Press Enter key to exit after sending all messages");
            Console.WriteLine("=============================================");

            await SendMessagesAsync(numberOfMessages);

            Console.ReadKey();

            await queueClient.CloseAsync();

        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for(var i=0;i<numberOfMessagesToSend;i++)
                {
                    string messageBody=$"Hello, world: {i}";
                    var message=new Message(Encoding.UTF8.GetBytes(messageBody));
                    Console.WriteLine($"Sending message: {messageBody}");
                    await queueClient.SendAsync(message);
                }
            } catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception {exception.Message}");
            }
        }
    }
}
