using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP4_salmen.Models;
using TP4_salmen.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace TP4_salmen.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISchoolRepository _schoolRepository;

        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository)
        {
            _studentRepository = studentRepository;
            _schoolRepository = schoolRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = schools != null && schools.Any()
                ? new SelectList(schools, "SchoolID", "SchoolName")
                : new SelectList(Enumerable.Empty<School>(), "SchoolID", "SchoolName");

            return View(_studentRepository.GetAll());
        }

        public IActionResult Search(string name, int? schoolid)
        {
            var result = _studentRepository.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                result = _studentRepository.FindByName(name);
            }
            else if (schoolid != null)
            {
                result = _studentRepository.GetStudentsBySchoolID(schoolid);
            }

            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = schools != null && schools.Any()
                ? new SelectList(schools, "SchoolID", "SchoolName")
                : new SelectList(Enumerable.Empty<School>(), "SchoolID", "SchoolName");

            return View("Index", result);
        }

        public IActionResult Create()
        {
            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = new SelectList(schools ?? Enumerable.Empty<School>(), "SchoolID", "SchoolName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Add(student);
                return RedirectToAction("Index");
            }
            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = new SelectList(schools ?? Enumerable.Empty<School>(), "SchoolID", "SchoolName", student.SchoolID);
            return View(student);
        }

        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = new SelectList(schools ?? Enumerable.Empty<School>(), "SchoolID", "SchoolName", student.SchoolID);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _studentRepository.Edit(student);
                return RedirectToAction("Index");
            }
            var schools = _schoolRepository.GetAll();
            ViewBag.SchoolID = new SelectList(schools ?? Enumerable.Empty<School>(), "SchoolID", "SchoolName", student.SchoolID);
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student != null)
            {
                _studentRepository.Delete(student);
            }
            return RedirectToAction("Index");
        }
    }
}
