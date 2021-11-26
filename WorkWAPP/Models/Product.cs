using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWAPP.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public int CategoriesId { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public string SpecialNote { get; set; }
      
        public virtual Category Categories { get; set; }



        

    }
    public enum SortState { 
        NameUp,
        PriceUp,
        CategoryUp,
        NameDown,
        PriceDown,
        CaregoryDown
    }
}
