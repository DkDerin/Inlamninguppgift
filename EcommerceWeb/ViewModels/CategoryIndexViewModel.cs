using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.ViewModels
{
    public class CategoryIndexViewModel
    {

        public string SearchAll { get; set; }
        

        public CategoryIndexViewModel()
        {
            Categories = new List<CategoryListViewModel>();
        }
        public class CategoryListViewModel
        {
            public int CategoryId { get; set; }
            [Required(ErrorMessage = "This field has to be filled.")]
            public string Name { get; set; }
        }
        public List<CategoryListViewModel> Categories { get; set; }

        public string CurrentSort { get; set; }

    }
}