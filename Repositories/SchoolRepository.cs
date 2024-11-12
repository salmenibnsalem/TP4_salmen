using System.Collections.Generic;
using System.Linq;
using TP4_salmen.Data;
using TP4_salmen.Models;
using Microsoft.EntityFrameworkCore;

namespace TP4_salmen.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly StudentContext _context;

        public SchoolRepository(StudentContext context) => _context = context;

        public IList<School> GetAll() => _context.Schools.OrderBy(s => s.SchoolName).ToList() ?? new List<School>();

        public School GetById(int id) => _context.Schools.Find(id);

        public void Add(School school)
        {
            _context.Schools.Add(school);
            _context.SaveChanges();
        }
        public void Edit(School school)
        {
            var existingSchool = _context.Schools.Find(school.SchoolID);
            if (existingSchool != null)
            {
                existingSchool.SchoolName = school.SchoolName;
                existingSchool.SchoolAddress = school.SchoolAddress;
                _context.SaveChanges();
            }
        }
        public void Delete(School school)
        {
            var existingSchool = _context.Schools.Find(school.SchoolID);
            if (existingSchool != null)
            {
                _context.Schools.Remove(existingSchool);
                _context.SaveChanges();
            }
        }
        public double StudentAgeAverage(int schoolId)
        {
            int count = StudentCount(schoolId);
            return count == 0 ? 0 : _context.Students.Where(s => s.SchoolID == schoolId).Average(s => s.Age);
        }
        public int StudentCount(int schoolId) => _context.Students.Count(s => s.SchoolID == schoolId);
    }
}
