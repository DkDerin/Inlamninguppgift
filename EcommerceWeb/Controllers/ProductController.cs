using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceWeb.Models;
using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string sort, int Id)
        {
            var model = new ViewModels.ProductIndexViewModel();
            using (var db = new EcommerceModel())
            {
                model.ProductList.AddRange(db.Products.Select(p => new ViewModels.ProductIndexViewModel.ProductListViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId

                }).Where(p => p.CategoryId == Id));
            }

            if (sort == "NamnAsc")
                model.ProductList = model.ProductList.OrderBy(r => r.Name).ToList();
            else if (sort == "NamnDesc")
                model.ProductList = model.ProductList.OrderByDescending(r => r.Name).ToList();

            model.CurrentSort = sort;
            return View(model);
        }

        [HttpGet]
        public ActionResult View(int Id)
        {
            var viewModel = new ProductViewViewModel();
            using (var db = new EcommerceModel())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == Id);
                viewModel.Name = product.Name;
                viewModel.Price = product.Price;
                viewModel.Description = product.Description;
                viewModel.CategoryId = product.CategoryId;
                viewModel.ProductId = product.ProductId;
            }
            return View(viewModel);

        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ViewModels.ProductCreateViewModel();
            SetupAvailableCatagories(model);
            
            return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Create(ViewModels.ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new EcommerceModel())
            {
                var pro = new Models.ProductsModel

                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };
                db.Products.Add(pro);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Product", new { id = model.CategoryId });
        }

        //SetUpAvailableCat FÖR CREATE
        void SetupAvailableCatagories(ViewModels.ProductCreateViewModel model)
        {
            model.AvailableCategory = new List<SelectListItem>
            {
                 new SelectListItem {Value = null , Text ="..Choose a catagory.."},


            };
            using (var db = new EcommerceModel())
            {
                foreach(var cat in db.Categories)
                {
                    model.AvailableCategory.Add(new SelectListItem { Value = cat.CategoryId.ToString(), Text = cat.Name  });
                }
            }


        }

        //SetUpAvailableCategories För EDIT
        void SetupAvailableCatagories(ViewModels.ProductEditViewModel model)
        {
            model.AvailableCategory = new List<SelectListItem>
            {
                 new SelectListItem {Value = null , Text ="..Choose a catagory.."},


            };
            using (var db = new EcommerceModel())
            {
                foreach (var cat in db.Categories)
                {
                    model.AvailableCategory.Add(new SelectListItem { Value = cat.CategoryId.ToString(), Text = cat.Name });
                }
            }


        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            using (var db = new EcommerceModel())
            {
                var prod = db.Products.FirstOrDefault(p => p.ProductId == id);
                var model = new ViewModels.ProductEditViewModel                                
                {
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                    CategoryId = prod.CategoryId,
                    ProductId = prod.ProductId,

                    
                };
                SetupAvailableCatagories(model);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Edit(ViewModels.ProductEditViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);
            }
            using (var db = new EcommerceModel())
            {
                var prod = db.Products.FirstOrDefault(r => r.ProductId == model.ProductId);
                prod.Name = model.Name;
                prod.Description = model.Description;               
                prod.Price = model.Price;
                prod.CategoryId= model.CategoryId;            
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Product", new { id = model.CategoryId });
        }



        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new EcommerceModel())
            {
                var prod = db.Products.Find(id);
                //using new prodCreate, it has the same prop as if i would do a deleteviewmodel
                var model = new ProductCreateViewModel
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,                   
                    CategoryId = prod.CategoryId
                };
                return View(model);
            }
        }


        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            using (var db = new EcommerceModel())
            {
                var obj = db.Products.Find(id);
                if (obj != null)
                {
                    db.Products.Remove(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product", new { id = obj.CategoryId });
                }
                return RedirectToAction("Index", "Home");
            }
        }



        //[HttpGet]
        //public ActionResult Search(string search, string sort)
        //{
        //    var model = new ProductIndexViewModel();
        //    model.SearchAll = search;

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        using (var db = new EcommerceModel())
        //        {
        //            model.ProductList.AddRange(db.Products
        //                .Select(x => new ProductIndexViewModel.ProductListViewModel
        //                {
        //                    ProductId = x.ProductId,
        //                    Name = x.Name,
        //                    Description = x.Description,
        //                    Price = x.Price,
        //                    CategoryId = x.CategoryId
        //                }));

        //            model.ProductList = model.ProductList.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) ||
        //                x.Description.ToUpper().Contains(search.ToUpper())).ToList();

        //            model = Sort(model, sort);

        //            return View("Search", model);
        //        }
        //    }

        //    return View("Search", model);
        //}

        private EcommerceModel db = new EcommerceModel();
        


        public ViewResult Search(string sortOrder, string searchString)
        {
            EcommerceModel db = new EcommerceModel();
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CurrentFilter = searchString;
            var prod = from s in db.Products
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                prod = prod.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }
            var prods = from p in db.Products
                        select p;

            switch (sortOrder)
            {
                case "name_desc":
                    prod = prod.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    prod = prod.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    prod = prod.OrderByDescending(s => s.Price);
                    break;
                default:
                    prod = prod.OrderBy(s => s.Name);
                    break;
            }

            return View(prod);
        }

        //public ActionResult Search2(string sortOrder)
        //{
        //    var prod = from p in db.Products
        //               select p;
        //}

        public void SortProducts(string sortOrder, ViewModels.ProductIndexViewModel model)
        {
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SortPrice = sortOrder == "Price" ? "price_desc" : "price";
            var prods = from p in db.Products
                        select p;

            switch (sortOrder)
            {
                case "name_desc":
                    model.ProductList = model.ProductList.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Price":
                    model.ProductList = model.ProductList.OrderBy(s => s.Price).ToList();
                    break;
                case "price_desc":
                    model.ProductList = model.ProductList.OrderByDescending(s => s.Price).ToList();
                    break;
                default:
                    model.ProductList = model.ProductList.OrderBy(s => s.Name).ToList();
                    break;
            }
        }



        //public ProductIndexViewModel Sort(ProductIndexViewModel model, string sort)
        //{
        //    if (sort == "ProductNameAsc")
        //        model.ProductList = model.ProductList.OrderBy(r => r.Name).ToList();
        //    else if (sort == "ProductNameDesc")
        //        model.ProductList = model.ProductList.OrderByDescending(r => r.Name).ToList();


        //    if (sort == "ProductPriceAsc")
        //        model.ProductList = model.ProductList.OrderBy(r => r.Price).ToList();
        //    else if (sort == "ProductPriceDesc")
        //        model.ProductList = model.ProductList.OrderByDescending(r => r.Price).ToList();
        //    model.CurrentSort = sort;
        //    return model;
        //}


        //public ActionResult Search(string searchString, string sort)
        //{
        //    var model = new ProductIndexViewModel();
        //    using (var db = new EcommerceModel())
        //    {
        //        if (!string.IsNullOrEmpty(searchString))
        //        {
        //            var products = db.Products.Where(p => p.Name.Contains(searchString));
        //            foreach (var p in products)
        //            {
        //                var pModel = new ProductIndexViewModel.ProductListViewModel
        //                {
        //                    ProductId = p.ProductId,
        //                    Name = p.Name,
        //                    Description = p.Description,
        //                    Price = p.Price,
        //                    CategoryId = p.Category.CategoryId

        //                };
        //                model.ProductList.Add(pModel);
        //            }
        //            model = Sort(model, sort);
        //        }
        //        return View(model);
        //    }
        //}


    }

}