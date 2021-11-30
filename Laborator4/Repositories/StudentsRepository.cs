using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborator4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Queues;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Laborator4.Repositories
{
    public class StudentsRepository : IStudentRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;

        private string _storageConnectionString;

        const string QueueName = "students-queue";

        public StudentsRepository(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }

        public async Task<StudentModel> CreateStudent(StudentModel student)
        {
            // var insertOperation = TableOperation.Insert(new StudentEntity(student));

            // await _studentsTable.ExecuteAsync(insertOperation);

            var jsonStudent = JsonConvert.SerializeObject(student);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String=System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(_storageConnectionString, QueueName);
            await queueClient.SendMessageAsync(base64String);
            queueClient.CreateIfNotExists();
            // var studentEntity = await getStudentLogic(student.University, student.CNP);

            // return new StudentModel(studentEntity);
            return student;
        }

        public async Task<List<StudentModel>> GetAllStudents()
        {
            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
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
            var studentModels = students.Select(student => new StudentModel(student.PartitionKey, student.RowKey, student.FirstName, student.LastName, student.Email, student.Year, student.PhoneNumber, student.Faculty)).ToList();
            return studentModels;
        }

        public async Task<StudentModel> GetStudent(string university, string cnp)
        {
            var studentEntity = await getStudentLogic(university, cnp);
            if (studentEntity != null)
            {
                return new StudentModel(studentEntity);
            }
            else
            {
                return null;
            }

        }

        public async Task<StudentModel> UpdateStudent(StudentModel student)
        {
            var studentEntity = await getStudentLogic(student.University, student.CNP);
            if (studentEntity != null)
            {
                studentEntity.FirstName = student.FirstName;
                studentEntity.LastName = student.LastName;
                studentEntity.Email = student.Email;
                studentEntity.Year = student.Year;
                studentEntity.PhoneNumber = student.PhoneNumber;
                studentEntity.Faculty = student.Faculty;
                var mergeOperation = TableOperation.Merge(studentEntity);


                await _studentsTable.ExecuteAsync(mergeOperation);

                var newStudentEntity = await getStudentLogic(studentEntity.PartitionKey, studentEntity.RowKey);
                return new StudentModel(newStudentEntity);
            }
            else
            {
                return null;
            }
        }


        public async Task<StudentModel> DeleteStudent(string university, string cnp)
        {
            var student = await getStudentLogic(university, cnp);
            if (student != null)
            {
                var entity = new DynamicTableEntity(student.PartitionKey, student.RowKey) { ETag = "*" };
                await _studentsTable.ExecuteAsync(TableOperation.Delete(entity));
                return new StudentModel(student);
            }
            else
            {
                return null;
            }
        }

        private async Task<StudentEntity> getStudentLogic(string university, string cnp)
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, university)).Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, cnp));
            TableContinuationToken token = null;
            TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
            if (resultSegment.Results != null)
            {
                return resultSegment.Results.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_storageConnectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("students");

            await _studentsTable.CreateIfNotExistsAsync();
        }
    }
}
