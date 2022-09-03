using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopAppDataAccess.Abstract
{
    public interface IProductDal : IRepository<Product>
    {
        void Update(Product entity, int[] categoryIds);
        Product GetByIdWithCategories(int id);
        List<Product> GetSearchProducts(string searchString);
        List<Product> GetHomePageProducts();
        List<Product> GetPopulerProducts();
        Product GetProducDetail(string productName);
        List<Product> GetProducsByCategory(string name , int page , int pageSize);
        int GetCountByCategory(string name);

    }
}
