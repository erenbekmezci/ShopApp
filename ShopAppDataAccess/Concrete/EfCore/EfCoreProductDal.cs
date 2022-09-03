using Microsoft.EntityFrameworkCore;
using ShopApp.entites;
using ShopAppDataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopAppDataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var db = new ShopContext())
            {
                var product = db.Products.Where(i => i.ProductId == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category).FirstOrDefault();

                return product;
            }
        }

        public int GetCountByCategory(string name)
        {
            using (var db = new ShopContext())
            {

                var products = db.Products.Where(i => i.isApproved).AsQueryable(); //sonradan sorgulama yapabilmek için asqueraylbe

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(i => i.Category.Url == name));
                }

                return products.Count();

            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var db = new ShopContext())
            {
                var products = db.Products.Where(i => i.isApproved && i.isHome).ToList();

                return products;
            }
        }

        public List<Product> GetPopulerProducts()
        {
            using (var db = new ShopContext())
            {
                var products = db.Products.ToList();

                return products;
            }
        }

        public Product GetProducDetail(string productName)
        {
            using (ShopContext db = new ShopContext())
            {
                var product = db.Products.Where(i => i.Url == productName)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category).FirstOrDefault();

                return product;
            }
        }

        public List<Product> GetProducsByCategory(string name, int page, int pageSize)
        {
            using (var db = new ShopContext())
            {

                var products = db.Products.Where(i => i.isApproved).AsQueryable(); //sonradan sorgulama yapabilmek için asqueraylbe

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(i => i.Category.Url == name));
                }

                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            }
        }

        public List<Product> GetSearchProducts(string searchString)
        {
            using (var db = new ShopContext())
            {

                var products = db.Products.Where(i => i.isApproved && (i.Name.ToLower().Contains(searchString) || i.Description.ToLower().Contains(searchString))).AsQueryable();



                return products.ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var db = new ShopContext())
            {

                var product = db.Products.Include(i => i.ProductCategories).FirstOrDefault(i => i.ProductId == entity.ProductId);

                if(product!=null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Url = entity.Url;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.isHome = entity.isHome;
                    product.isApproved = entity.isApproved;

                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        ProductId = entity.ProductId,
                        CategoryId = catid
                    }).ToList();

                    db.SaveChanges();

                }



                
            }


        }
    } 
}
