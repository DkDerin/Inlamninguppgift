using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.ViewModels
{
    public class UserIndexViewModel
    {
        public UserIndexViewModel()
        {
            UserList = new List<UserListViewModel>();
        }

        public class UserListViewModel
        {
            public string UserId { get; set; }
            public string UserRoles { get; set; }
            [Required(ErrorMessage = "This field has to be filled")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public string UserId { get; set; }
        public string UserRoles { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<UserListViewModel> UserList { get; set; }
    }

    public class UserCRUDViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        public string UserRoles { get; set; }

        public string UserDropDown { get; set; }
        public List<SelectListItem> AvailableUsers { get; set; }
    }
}