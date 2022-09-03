using Microsoft.EntityFrameworkCore;
using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopAppDataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if(context.Database.GetPendingMigrations().Count() == 0)
            {
                if(context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.AddRange(products);
                    context.AddRange(ProductCategories);
                }
               

                context.SaveChanges();
            }
        }

        private static Category[] categories =
        {
            new Category(){Name = "Telefon" , Url = "telefon"},
            new Category(){Name = "Elektronik" , Url = "elektronik"},
            new Category(){Name = "Bilgisayar" , Url = "bilgisayar"},
            new Category(){Name = "Beyaz Eşya" , Url = "beyaz-esya"} //bu kısımda name e göre urlye çevirebilen bir fonksiyon yaz sanırım trigger 
        };
        private static Product[] products =
        {
            new Product(){Name = "samsung s6", Url = "samsung-s6", Price = 8000  , ImageUrl = "1.jpg" , isApproved =true , isHome = true},
            new Product(){Name = "samsung s7", Url = "samsung-s7" , Price = 8000  , ImageUrl = "7.jpg", isApproved =true, isHome = false},
            new Product(){Name = "samsung s8", Url = "samsung-s8" , Price = 8000  , ImageUrl = "6.jpg", isApproved =true, isHome = true},
            new Product(){Name = "samsung s9", Url = "samsung-s9" , Price = 8000  , ImageUrl = "3.jpg", isApproved =true, isHome = false},
            new Product(){Name = "ACER ", Url = "acer" , Price = 8000  , ImageUrl = "3.jpg", isApproved =false , isHome = true},
            new Product(){Name = "MONSTER", Url = "monster" , Price = 8000  , ImageUrl = "3.jpg", isApproved =false, isHome = true},
            new Product(){Name = "iphone X", Url = "iphone-x" , Price = 8000  , ImageUrl = "3.jpg", isApproved =true, isHome = false},
            new Product(){Name = "iphone s9", Url = "iphone-s9" , Price = 8000  , ImageUrl = "3.jpg", isApproved =true, isHome = true},
            new Product(){Name = "iphone 11", Url = "iphone-11" , Price = 8000  , ImageUrl = "3.jpg", isApproved =false, isHome = false},
            new Product(){Name = "iphone s5", Url = "iphone-s5" , Price = 2000 , ImageUrl  = "2.jpg", isApproved =true, isHome = true}
        };

        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product = products[0] , Category = categories[0]},
            new ProductCategory(){Product = products[0] , Category = categories[1]},
            new ProductCategory(){Product = products[1] , Category = categories[0]},
            new ProductCategory(){Product = products[1] , Category = categories[1]},
            new ProductCategory(){Product = products[2] , Category = categories[0]},
            new ProductCategory(){Product = products[3] , Category = categories[2]},
            new ProductCategory(){Product = products[4] , Category = categories[2]},
            new ProductCategory(){Product = products[4] , Category = categories[1]}
        };
    }

}
