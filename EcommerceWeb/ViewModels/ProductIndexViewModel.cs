using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.ViewModels
{
    public class ProductIndexViewModel
    {

        public string SearchAll { get; set; }
        

        public ProductIndexViewModel()
        {
            ProductList = new List<ProductListViewModel>();
        }
        public class ProductListViewModel
        {
            public int ProductId { get; set; }
            [Required(ErrorMessage = "* You cant leave the Product name blank")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "* Name must have min length of 3 and max Length of 50")]
            public string Name { get; set; }
            [Required(ErrorMessage = "* You cant leave the Product name blank")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "* Name must have min length of 3 and max Length of 50")]
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }

        }
        public List<ProductListViewModel> ProductList { get; set; }
        public string CurrentSort { get; set; }
        public string SortName { get; set; }
        public string SortPrice { get; set; }
    }
}