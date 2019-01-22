namespace EcommerceWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EcommerceWeb.EcommerceModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EcommerceWeb.EcommerceModel context)
        {
            context.Categories.AddOrUpdate(c => c.CategoryId,
               new Models.CategoryModel { CategoryId = 1, Name = "Berries" },
               new Models.CategoryModel { CategoryId = 2, Name = "Fruits" },
               new Models.CategoryModel { CategoryId = 3, Name = "Vegetables" },
               new Models.CategoryModel { CategoryId = 4, Name = "Meat" },
               new Models.CategoryModel { CategoryId = 5, Name = "Bathroom" }

           );


            context.Products.AddOrUpdate(p => p.ProductId,
                new Models.ProductsModel { ProductId = 1, CategoryId = 1, Name = "BlueBerries", Description = "Freshly picked ultra delicious berries", Price = 13 },
                new Models.ProductsModel { ProductId = 2, CategoryId = 2, Name = "Pineapple", Description = "Mouth wattering golden pineapple juice", Price = 23 },
                new Models.ProductsModel { ProductId = 3, CategoryId = 3, Name = "Broccoli", Description = "A mini tree", Price = 7 },
                new Models.ProductsModel { ProductId = 4, CategoryId = 4, Name = "Chicken", Description = "Tasty chicken", Price = 45 },
                new Models.ProductsModel { ProductId = 5, CategoryId = 5, Name = "Towels", Description = "Small Bathroom towels", Price = 27 }
                );
        }
    }
}
