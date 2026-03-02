using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mission8_S4_G2.Models;
using SQLitePCL;
using System.Diagnostics;

namespace Mission8_S4_G2.Controllers
{

    public class HomeController : Controller
    {
        private QuadrantsContext _context;

        public HomeController(QuadrantsContext temp)
        {
            _context = temp;
        }
        public IActionResult Index()
        {
            var tasks = _context.Tasks.Include(t => t.Category).ToList();
            return View("~/Views/Tasks/Index.cshtml", tasks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories
                    .OrderBy(c => c.CategoryName)
                    .ToList(),
                "CategoryId",
                "CategoryName");
            return View(new Models.Task());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .OrderBy(c => c.CategoryName)
                    .ToList(),
                "CategoryId",
                "CategoryName");
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .OrderBy(c => c.CategoryName)
                    .ToList(),
                "CategoryId",
                "CategoryName");
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Update(task);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .OrderBy(c => c.CategoryName)
                    .ToList(),
                "CategoryId",
                "CategoryName");
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkCompleted(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            task.Completed = true;
            _context.Update(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

    }
}