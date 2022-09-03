using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.entites
{
    public class Product
    {
        
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Url { get; set; }
        public string Description { get; set; }

        public bool isApproved { get; set; }
        public bool isHome { get; set; }

        public string ImageUrl { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }




    }
}
