using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP4_salmen.Models;

namespace TP4_salmen.Data
{
    public class StudentContext : IdentityDbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<School> Schools { get; set; }
    }
}
