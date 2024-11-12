using System.Collections.Generic;
using System.Linq;
using TP4_salmen.Data;
using TP4_salmen.Models;
using Microsoft.EntityFrameworkCore;

namespace TP4_salmen.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context) => _context = context;

        public IList<Student> GetAll() => _context.Students.Include(s => s.School).OrderBy(s => s.StudentName).ToList();

        public Student? GetById(int id) => _context.Students.Include(s => s.School).SingleOrDefault(s => s.StudentId == id);

        public void Add(Student s)
        {
            _context.Students.Add(s);
            _context.SaveChanges();
        }

        public void Edit(Student s)
        {
            var existing = _context.Students.Find(s.StudentId);
            if (existing != null)
            {
                existing.StudentName = s.StudentName;
                existing.Age = s.Age;
                existing.BirthDate = s.BirthDate;
                existing.SchoolID = s.SchoolID;
                _context.SaveChanges();
            }
        }

        public void Delete(Student s)
        {
            var existing = _context.Students.Find(s.StudentId);
            if (existing != null)
            {
                _context.Students.Remove(existing);
                _context.SaveChanges();
            }
        }

        public IList<Student> GetStudentsBySchoolID(int? schoolId)
            => _context.Students.Where(s => s.SchoolID == schoolId).Include(s => s.School).OrderBy(s => s.StudentName).ToList();

        public IList<Student> FindByName(string name)
            => _context.Students.Where(s => s.StudentName.Contains(name)).Include(s => s.School).ToList();
    }
}
