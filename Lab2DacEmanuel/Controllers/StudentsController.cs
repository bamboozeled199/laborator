using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories;
using Models;

namespace Lab2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        public StudentsController()
        {
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentsRepo.Students;
        }

        [HttpGet("{id}")]
        public Student GetStudent(int id)
        {
            return StudentsRepo.Students.FirstOrDefault(student => student.Id == id);
        }

        [HttpPost]
        public void Post([FromBody] Student student)
        {
            StudentsRepo.Students.Add(student);
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            StudentsRepo.Students.Remove(StudentsRepo.Students.FirstOrDefault(student => student.Id == id));
        }
    }
}
