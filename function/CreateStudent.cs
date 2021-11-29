using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace students
{
    public static class CreateStudent
    {
        [Function("CreateStudent")]
        [TableOutput("students")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "azurestorageemanuel_STORAGE")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("CreateStudent");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
