using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}