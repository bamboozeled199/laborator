using System;

namespace Models
{
    public class StudentModel
    {
        public StudentModel(string University, string CNP, string FirstName, string LastName, string Email, int Year, string PhoneNumber, string Faculty)
        {
            this.University = University;
            this.CNP = CNP;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Year = Year;
            this.PhoneNumber = PhoneNumber;
            this.Faculty = Faculty;
        }
        public StudentModel(string University, string CNP)
        {
            this.University = University;
            this.CNP = CNP;
        }
        public StudentModel() { }
        public string University { get; set; }
        public string CNP { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }

        public string Faculty { get; set; }
    }
}