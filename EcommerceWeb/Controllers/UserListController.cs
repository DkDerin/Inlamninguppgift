using EcommerceWeb.Models;
using EcommerceWeb.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class UserListController : Controller
    {
        // GET: UserList
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var model = (from users in db.Users
                             select new
                             {
                                 UserId = users.Id,
                                 Email = users.Email,
                                 RoleNames = (from userRole in users.Roles
                                              join role in db.Roles on userRole.RoleId
                                              equals role.Id
                                              select role.Name).ToList()
                             }).ToList().Select(p => new UserIndexViewModel()
                             {
                                 UserId = p.UserId,
                                 Email = p.Email,
                                 UserRoles = string.Join(",", p.RoleNames)
                             });
                return View(model);
            }
        }


        //[AllowAnonymous]
        //public ActionResult UserList()
        //{
        //    var context = new Models.ApplicationDbContext();
        //    return View(context.Users.ToList());
        //}
        //[AllowAnonymous]
        //public ActionResult UserDelete(string id)
        //{
        //    var context = new Models.ApplicationDbContext();
        //    var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
        //    return View(user);
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult UserDelete(ApplicationUser appuser)
        //{
        //    var context = new Models.ApplicationDbContext();
        //    var user = context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();
        //    context.Users.Remove(user);
        //    context.SaveChanges();
        //    //var user = context.Users.Where(u => u.Id == id.ToString()).FirstOrDefault();
        //    return RedirectToAction("UserList");
        //}
        //[AllowAnonymous]
        //public ActionResult UserEdit(string id)
        //{
        //    var context = new Models.ApplicationDbContext();
        //    var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
        //    return View(user);
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult UserEdit(ApplicationUser appuser)
        //{
        //    var context = new Models.ApplicationDbContext();
        //    var user = context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();
        //    //context.Entry(appuser).State = EntityState.Modified;
        //    user.Email = appuser.Email;
        //    user.UserName = appuser.UserName;
        //    user.PhoneNumber = appuser.PhoneNumber;
        //    user.PasswordHash = user.PasswordHash;
        //    context.SaveChanges();
        //    //var user = context.Users.Where(u => u.Id == id.ToString()).FirstOrDefault();
        //    return RedirectToAction("UserList");
        //}

        //SetUpAvailableCategories För EDIT
        void SetupAvailableUsers(ViewModels.UserCRUDViewModel model)
        {
            model.AvailableUsers = new List<SelectListItem>
            {
                 new SelectListItem {Value = null , Text ="..Choose a Role.."},


            };
            using (var db = new ApplicationDbContext())
            {
                foreach (var use in db.Users)
                {
                    model.AvailableUsers.Add(new SelectListItem { Value = use.Id.ToString(), Text = use.UserName });
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(string id)
        {

            using (var db = new ApplicationDbContext())
            {
                var model = new UserCRUDViewModel();
                var user = UserManager.FindById(id);
                model.UserId = user.Id;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.UserRoles = UserManager.GetRoles(user.Id).SingleOrDefault();
                model.AvailableUsers = new SelectList(context.Roles, "Name", "Name");

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UserCRUDViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new ApplicationDbContext())
            {
                var user = UserManager.FindById(model.UserId);
                user.Id = model.UserId;

                string choosenRole = Request.Form["Roles"];

                if (model.UserRoles == "Admin")
                {
                    UserManager.RemoveFromRole(user.Id, "Admin");
                    UserManager.AddToRole(user.Id, "ProductManager");
                }
                else if (model.UserRoles == "ProductManager")
                {
                    UserManager.RemoveFromRole(user.Id, "ProductManager");
                    UserManager.AddToRole(user.Id, "Admin");
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}