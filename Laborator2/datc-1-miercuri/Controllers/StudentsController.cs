using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using datc_1_miercuri.Models;
using datc_1_miercuri.Repositories;
using System.Linq;

namespace datc_1_miercuri.Controllers
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
          return StudentsRepo.Students.FirstOrDefault(student=>student.Id==id);
        }

        [HttpPost]
        public void Post([FromBody] Student student)
        {
          StudentsRepo.Students.Add(student);
        }
    }
}
