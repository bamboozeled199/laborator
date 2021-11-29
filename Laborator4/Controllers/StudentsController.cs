using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborator4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Laborator4.Repositories;

namespace Laborator4.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {

        private IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("{university}/{cnp}")]
        public async Task<StudentModel> Get(string university, string cnp)
        {
            return await _studentRepository.GetStudent(university, cnp);
        }


        [HttpGet]
        public async Task<IEnumerable<StudentModel>> Get()
        {
            return await _studentRepository.GetAllStudents();
        }

        [HttpPost]
        public async Task<StudentModel> Post([FromBody] StudentModel student)
        {
            try
            {
                return await _studentRepository.CreateStudent(student);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<StudentModel> Update([FromBody] StudentModel student)
        {
            return await _studentRepository.UpdateStudent(student);
        }

        [HttpDelete("{university}/{cnp}")]
        public async Task<StudentModel> Delete(string university, string cnp)
        {
            return await _studentRepository.DeleteStudent(university, cnp);
        }
    }
}
