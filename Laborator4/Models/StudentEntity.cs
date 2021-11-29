using Microsoft.WindowsAzure.Storage.Table;

namespace Laborator4.Models
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string university, string cnp)
        {
            this.PartitionKey = university;
            this.RowKey = cnp;
        }
        public StudentEntity(StudentModel student)
        {
            this.PartitionKey = student.University;
            this.RowKey = student.CNP;
            this.FirstName = student.FirstName;
            this.LastName = student.LastName;
            this.Email = student.Email;
            this.Year = student.Year;
            this.PhoneNumber = student.PhoneNumber;
            this.Faculty = student.Faculty;
        }
        public StudentEntity() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public int Year { get; set; }
        public string PhoneNumber { get; set; }
        public string Faculty { get; set; }
    }
}