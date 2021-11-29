using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Laborator5
{
    public class NewStudentEntity : TableEntity
    {
        public NewStudentEntity(){}
        public NewStudentEntity(string university, DateTime timestamp, int count)
        {
            this.PartitionKey = university;
            this.RowKey = timestamp.ToString("dd-MM-yyyy");
            this.Count = count;
        }
        public int Count { get; set; }
    }
}