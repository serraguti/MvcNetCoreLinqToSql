using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSql.Models;
using MvcNetCoreLinqToSql.Repositories;

namespace MvcNetCoreLinqToSql.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController()
        {
            this.repo = new RepositoryEnfermos();
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        public IActionResult Details(int inscripcion)
        {
            Enfermo enfermo = this.repo.FindEnfermo(inscripcion);
            return View(enfermo);
        }

        public IActionResult Delete(int inscripcion)
        {
            this.repo.DeleteEnfermo(inscripcion);
            return RedirectToAction("Index");
        }
    }
}
