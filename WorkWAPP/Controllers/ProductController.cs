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
            IQueryable<Product> products = db.Products.Include(x => x.Categories);

            if (!String.IsNullOrEmpty(name)) {
                products = products.Where(p => p.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(categoryName))
            {
                products = products.Where(p => p.Categories.Name.Contains(categoryName));
            }
            if (price != null)
            {
                products = products.Where(p => p.Price == price);
            }


            ViewData["NameUp"] = sortOrder == SortState.NameUp ? SortState.NameDown : SortState.NameUp;
            ViewData["PriceUp"] = sortOrder == SortState.PriceUp ? SortState.PriceDown : SortState.PriceUp;
            ViewData["CategoryUp"] = sortOrder == SortState.CategoryUp ? SortState.CaregoryDown : SortState.CategoryUp;

            products = sortOrder switch
            {
                SortState.NameUp => products.OrderBy(s => s.Name),
                SortState.PriceUp => products.OrderBy(s => s.Price),
                SortState.CategoryUp => products.OrderBy(s => s.Categories.Name),
                SortState.NameDown => products.OrderByDescending(s => s.Name),
                SortState.PriceDown => products.OrderByDescending(s => s.Price),
                SortState.CaregoryDown => products.OrderByDescending(s => s.Categories.Name),

                _ => products.OrderBy(s => s.Name),
            };
            return View(products.AsNoTracking().ToList()); ;
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

            SelectList categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.Categories = categories;

            if (db.Add(product))
                return RedirectToAction("Index");
            return View();

        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            SelectList categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
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
