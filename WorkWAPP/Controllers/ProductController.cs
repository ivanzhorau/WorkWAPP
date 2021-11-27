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
    public class ProductController : Controller
    {
        ProductContext db;

        public ProductController(ProductContext context)
        {
            db = context;

        }
        // GET: ProductController
       
        public IActionResult Index(int? price, string name, string categoryName, SortState sortOrder = SortState.NameUp)
        {
            
            return View(db.GetProducts(price, name, categoryName, sortOrder, ViewData).AsNoTracking().ToList()); ;
        }


        // GET: ProductController/Create
        public IActionResult Create()
        {
            SelectList categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {

            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name"); ;

            if (db.Add(product))
                return RedirectToAction("Index");
            return View();

        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
             
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name"); 
            return View(db.Products.Find(id));
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            if (db.Edit(id,product))
                return RedirectToAction("Index");
            return View();
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {

            if (db.DeleteProduct(id))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
