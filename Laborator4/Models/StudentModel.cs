using System.Globalization;

namespace Laborator4.Models
{
    public class StudentModel
    {
        public StudentModel(string university, string cnp, string firstName, string lastName, string email, int year, string phoneNumber, string faculty)
        {
            University = university;
            CNP = cnp;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Year = year;
            PhoneNumber = phoneNumber;
            Faculty = faculty;
        }
        public StudentModel(StudentEntity studentEntity)
        {
            University = studentEntity.PartitionKey;
            CNP = studentEntity.RowKey;
            FirstName = studentEntity.FirstName;
            LastName = studentEntity.LastName;
            Email = studentEntity.Email;
            Year = studentEntity.Year;
            PhoneNumber = studentEntity.PhoneNumber;
            Faculty = studentEntity.Faculty;
        }
        public StudentModel() { }
        public string University { get; set; }
        public string CNP { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public int Year { get; set; }
        public string PhoneNumber { get; set; }
        public string Faculty { get; set; }
    }
}