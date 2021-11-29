using Laborator4.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laborator4.Repositories
{
    public interface IStudentRepository
    {
        Task<List<StudentModel>> GetAllStudents();
        Task<StudentModel> GetStudent(string university, string cnpu);
        Task<StudentModel> CreateStudent(StudentModel student);
        Task<StudentModel> UpdateStudent(StudentModel student);
        Task<StudentModel> DeleteStudent(string university, string cnpu);
    }
}