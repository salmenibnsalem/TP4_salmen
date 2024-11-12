using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP4_salmen.Models;
using TP4_salmen.Repositories;

namespace TP4_salmen.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class SchoolController : Controller
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        [AllowAnonymous]
        public IActionResult Index() => View(_schoolRepository.GetAll());

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(School school)
        {
            if (ModelState.IsValid)
            {
                _schoolRepository.Add(school);
                return RedirectToAction("Index");
            }
            return View(school);
        }

        public IActionResult Edit(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, School school)
        {
            if (id != school.SchoolID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _schoolRepository.Edit(school);
                return RedirectToAction("Index");
            }
            return View(school);
        }

        public IActionResult Delete(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school != null)
            {
                _schoolRepository.Delete(school);
            }
            return RedirectToAction("Index");
        }
    }
}
