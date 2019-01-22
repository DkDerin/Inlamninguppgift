using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcommerceWeb;
using EcommerceWeb.Models;
using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {    
        public ActionResult Index( string sort)
        {
            var model = new ViewModels.CategoryIndexViewModel();
            using (var db = new EcommerceModel())
            {
                model.Categories.AddRange(db.Categories.Select(r => new ViewModels.CategoryIndexViewModel.CategoryListViewModel
                {
                    Name = r.Name,
                    CategoryId = r.CategoryId

                }));
            }

            if (sort == "NamnAsc")
                model.Categories = model.Categories.OrderBy(r => r.Name).ToList();
            else if (sort == "NamnDesc")
                model.Categories= model.Categories.OrderByDescending(r => r.Name).ToList();

            model.CurrentSort = sort;
            
            return View(model);
        }

        [Authorize(Roles ="Admin, Manager")]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ViewModels.CategoryCreateViewModel();
           return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Create(ViewModels.CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new EcommerceModel())
            {
                var cat = new Models.CategoryModel
                {
                    Name = model.Name,
                };
                db.Categories.Add(cat);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new EcommerceModel())
            {
                var cat = db.Categories.Find(id);
                //new Createviewmodel för att Delete hade haft samma prop
                var model = new CategoryCreateViewModel
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
                };
                return View(model);
            }
        }



        [Authorize(Roles = "Admin, Manager")]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            using(var db=new EcommerceModel())
            {
                var obj = db.Categories.Find(id);
                if(obj != null)
                {
                    db.Categories.Remove(obj);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }


        //Edit Actionresult begins
        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            using (var db = new EcommerceModel())
            {
                var cats = db.Categories.FirstOrDefault(p => p.CategoryId == id);
                var model = new ViewModels.CategoryEditViewModel
                {
                    Name = cats.Name,     
                    CategoryId = cats.CategoryId,
                 


                };            
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Edit(ViewModels.CategoryEditViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);
            }
            using (var db = new EcommerceModel())
            {
                var cats = db.Categories.FirstOrDefault(r => r.CategoryId == model.CategoryId);
                cats.Name = model.Name;
                cats.CategoryId = model.CategoryId;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Category", new { id = model.CategoryId });
        }
    }
}