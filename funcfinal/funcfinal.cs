using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace students
{
    public class funcfinal
    {
        [FunctionName("funcfinal")]
        [return: Table("students")]
        public StudentEntity Run([QueueTrigger("students-queue", Connection = "azurestorageemanuel_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            
            var studentModel = JsonConvert.DeserializeObject<StudentModel>(myQueueItem);
            var studentEntity = new StudentEntity(studentModel);
            Console.WriteLine("PK:" + studentEntity.PartitionKey);
            Console.WriteLine("RK: " + studentEntity.RowKey);
            Console.WriteLine("FirstName: " + studentEntity.FirstName);
            Console.WriteLine("LastName: " + studentEntity.LastName);
            Console.WriteLine("Email: " + studentEntity.Email);
            Console.WriteLine("PhoneNumber: " + studentEntity.PhoneNumber);
            Console.WriteLine("Year: " + studentEntity.Year);
            Console.WriteLine("Faculty: " + studentEntity.Faculty);

            return studentEntity;
        }
    }
}
