using Microsoft.AspNetCore.Mvc.RazorPages;
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
            Database.EnsureCreated();
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
                product.Id = Products.ToList().Count!=0?Products.ToList()[Products.ToList().Count - 1].Id + 1:1;
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
                category.Id = Categories.ToList().Count!=0?Categories.ToList()[Categories.ToList().Count - 1].Id + 1:1;
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
        
    }
}
