using System;
using Microsoft.Azure.Cosmos.Table;

namespace Models
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string University, string CNP, string FirstName, string LastName, string Email, int Year, string PhoneNumber, string Faculty)
        {
            this.PartitionKey = University;
            this.RowKey = CNP;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Year = Year;
            this.PhoneNumber = PhoneNumber;
            this.Faculty = Faculty;
        }
        public StudentEntity(string University, string CNP)
        {
            this.PartitionKey = University;
            this.RowKey = CNP;
        }

        public StudentEntity(StudentModel studentModel)
        {
            this.PartitionKey = studentModel.University;
            this.RowKey = studentModel.CNP;
            this.FirstName = studentModel.FirstName;
            this.LastName = studentModel.LastName;
            this.Email = studentModel.Email;
            this.Year = studentModel.Year;
            this.PhoneNumber = studentModel.PhoneNumber;
            this.Faculty = studentModel.Faculty;
        }
        public StudentEntity() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }

        public string Faculty { get; set; }

    }
}