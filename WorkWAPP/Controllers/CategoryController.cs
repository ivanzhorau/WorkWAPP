using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkWAPP.Models;

namespace WorkWAPP.Controllers
{
    public class CategoryController : Controller
    {
        ProductContext db;

        public CategoryController(ProductContext context)
        {
            db = context;

        }
        // GET: CategoryController
        public IActionResult Index()
        {
            return View(db.Categories.ToList());
        }



        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {

            if (db.Add(category))
                return RedirectToAction("Index");
            return View();

        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(db.Categories.Find(id));
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            if (db.Edit(id,category))
                return RedirectToAction("Index");
            return View();
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {

            if (db.DeleteCategory(id))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
