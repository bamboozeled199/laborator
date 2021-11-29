using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Laborator7
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://laborator6cioaraemanuel.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZTHbpbKQ3U2px+mkapVRAenz+o5yCAM/wecbBMG+PI4=";
        const string QueueName = "coadalab6";
        static IQueueClient queueClient;
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("=============================================");
            Console.WriteLine("Press Enter key to exit after receiving all messages");
            Console.WriteLine("=============================================");

            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await queueClient.CloseAsync();

        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        static async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: Sequence number:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity path: {context.EntityPath}");
            Console.WriteLine($"- Executing action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}
