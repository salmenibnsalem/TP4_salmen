using System.Collections.Generic;

namespace TP4_salmen.Models
{
    public class School
    {
        public int SchoolID { get; set; }
        public required string SchoolName { get; set; }
        public required string SchoolAddress { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
