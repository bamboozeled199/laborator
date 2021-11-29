using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Linq;

namespace Laborator5
{
    class Program
    {
        private static CloudTableClient _tableClient;
        private static CloudTable _studentsTable;

        private static string _storageConnectionString;

        private static List<StudentEntity> students = new List<StudentEntity>();
        private static List<string> universities = new List<string> { "UPT", "UVT" };
        static void Main(string[] args)
        {
            _storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=azurestorageemanuel;AccountKey=O3jxhVjbZi6ua37j/Nw20NU1QgMxw1CakID2H633s/KfWGO1nRbKW0kTsr5KWiD71/2qAAzv2JPDh8dIFqnqNg==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable("students"); })
                .GetAwaiter()
                .GetResult();
            Task.Run(async () => { await GetAllStudents("students"); })
                .GetAwaiter()
                .GetResult();
            Task.Run(async () => { await InitializeTable("studentsNew"); })
                .GetAwaiter()
                .GetResult();
            Task.Run(async () => { await PopulateNewTable("studentsNew"); })
                .GetAwaiter()
                .GetResult();

        }

        private static async Task InitializeTable(string tableName)
        {
            var account = CloudStorageAccount.Parse(_storageConnectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference(tableName);

            await _studentsTable.CreateIfNotExistsAsync();
        }

        private static async Task GetAllStudents(string tableName)
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            var account = CloudStorageAccount.Parse(_storageConnectionString);
            _tableClient = account.CreateCloudTableClient();
            _studentsTable = _tableClient.GetTableReference(tableName);
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                foreach (StudentEntity entity in resultSegment.Results)
                {
                    students.Add(entity);
                }
            } while (token != null);
        }
        private static async Task PopulateNewTable(string tableName)
        {
            _studentsTable = _tableClient.GetTableReference(tableName);
            bool created = await _studentsTable.CreateIfNotExistsAsync();

            if (!created)
            {
                // not created, table already existed, delete all content
                await DeleteAllRows<NewStudentEntity>(tableName, _tableClient);
                Console.WriteLine("Azure Table: " + _studentsTable.Name + " Purged", _studentsTable);
            }
            _studentsTable = _tableClient.GetTableReference(tableName);
            await _studentsTable.CreateIfNotExistsAsync();
            int count;

            foreach (String university in universities)
            {
                count = 0;
                foreach (StudentEntity studentEntity in students)
                {

                    if (studentEntity.PartitionKey.CompareTo(university) == 0)
                    {
                        count++;
                    }
                }
                var student = new NewStudentEntity(university, DateTime.Now, count);
                var insertOperation = TableOperation.Insert(student);
                await _studentsTable.ExecuteAsync(insertOperation);
                Console.WriteLine("Inserted entity: " + student.PartitionKey + student.RowKey + student.Count);
            }
            var generalEntity = new NewStudentEntity("General", DateTime.Now, students.Count());
            var insertOperationGeneral = TableOperation.Insert(generalEntity);
            await _studentsTable.ExecuteAsync(insertOperationGeneral);
            Console.WriteLine("Inserted entity: " + generalEntity.PartitionKey + generalEntity.RowKey + generalEntity.Count);
        }
        private static async Task DeleteAllRows<T>(string table, CloudTableClient client) where T : ITableEntity, new()
        {
            // query all rows
            CloudTable tableref = client.GetTableReference(table);
            var query = new TableQuery<T>();
            TableContinuationToken token = null;

            do
            {
                var result = await tableref.ExecuteQuerySegmentedAsync(query, token);
                foreach (var row in result)
                {
                    var op = TableOperation.Delete(row);
                    tableref.ExecuteAsync(op);
                }
                token = result.ContinuationToken;
            } while (token != null);

        }
    }

}
