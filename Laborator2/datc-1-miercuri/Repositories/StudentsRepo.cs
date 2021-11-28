using System.Collections.Generic;
using datc_1_miercuri.Models;

namespace datc_1_miercuri.Repositories
{
    public static class StudentsRepo
    {
        public static List<Student> Students = new List<Student>(){
            new Student(){Id=1,Faculty="AC",Name="Emanuel",Year=4},
            new Student(){Id=2,Faculty="ETC",Name="Cioara",Year=2}
        };
    }
}