using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.ViewModels
{
    public class CategoryViewViewModel
    {
        [Required(ErrorMessage = "This field has to be filled.")]
        public string Name { get; set; }
    }
}