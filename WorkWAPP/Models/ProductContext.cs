using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWAPP.Models
{
    public class ProductContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        


        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
            //  Database.EnsureCreated();
           
            if (!Categories.Any()) {
                Categories.Add(new Category() { Name = "Еда" });
                Categories.Add(new Category() { Name = "Напитки" });
            }
            SaveChanges();
            if (!Products.Any())
            {
                Products.Add(new Product() {Name = "Купаты", Description = "Главное не перепутать", CategoriesId = 1, Categories = this.Categories.ToList()[0], Price = 100, Note = "И вот почему", SpecialNote = "https://www.youtube.com/watch?v=KD18xWXIiGM&t=2s" });
                Products.Add(new Product() {Name = "Трунок", Description = "Болтанка ликёра и энергетика.", CategoriesId = 2, Categories = this.Categories.ToList()[1], Price = 100, Note = "Если перепить, то можно встретиться с автором оригинального рецепта крамбамбули", SpecialNote = "Хоть голова после этого болеть не будет" });
            }
            SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=IVAN;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public Product PrepareProduct(Product product)
        {
            product.Categories = Categories.Find(product.CategoriesId);
            product.Price = Math.Abs(product.Price);
            return product;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Categories)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.CategoriesId);
        }
        public bool DeleteProduct(int id)
        {
            Product product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
            
                SaveChanges();
                return true;
            }
            return false;
        }
        public bool Add(Product product)
        {
            try
            {
                product = PrepareProduct(product);
                Products.Add(product);
                SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool Edit(int id,Product product)
        {
            try
            {

                product = PrepareProduct(product); 
                Products.Update(product);
                SaveChanges();
                return true;
            }
            catch { return false; }
        }



        public bool DeleteCategory(int id)
        {
            var prods = (Products.Where<Product>(p => p.CategoriesId == id));
            foreach (var prod in prods) {
                Products.Remove(prod);

            }
            Category category =Categories.FirstOrDefault(p => p.Id == id);
            if (category != null)
            {
                Categories.Remove(category);
                SaveChanges();
                return true;
            }
            return false;
        }
        public bool Add(Category category)
        {
            try
            {
                Categories.Add(category);
                SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public bool Edit(int id, Category category)
        {
            try
            {
                Categories.Update(category);
                SaveChanges();
                return true;
            }
            catch { return false; }
        }


        public IQueryable<Product> GetProducts(int? price, string name, string categoryName, SortState sortOrder, ViewDataDictionary viewData) {
            IQueryable<Product> products = Products.Include(x => x.Categories);

            if (!String.IsNullOrEmpty(name))
            {
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


            viewData["NameUp"] = sortOrder == SortState.NameUp ? SortState.NameDown : SortState.NameUp;
            viewData["PriceUp"] = sortOrder == SortState.PriceUp ? SortState.PriceDown : SortState.PriceUp;
            viewData["CategoryUp"] = sortOrder == SortState.CategoryUp ? SortState.CaregoryDown : SortState.CategoryUp;

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
            return products;
        }

    }
}
