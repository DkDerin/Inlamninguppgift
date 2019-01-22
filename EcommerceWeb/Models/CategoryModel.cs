using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EcommerceWeb.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<ProductsModel> Products { get; set; }
    }

    //public class DBCategory : DbContext
    //{
    //    public DbSet<CategoryModel> Categories { get; set; }

    //}
}