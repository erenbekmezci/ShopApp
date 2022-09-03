using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        List<Product> GetProducsByCategory(string name, int page, int pageSize);
        List<Product> GetPopulerProducts();
        List<Product> GetAll();
        Product GetProducDetail(string productName);
        Product GetById(int id);

        bool Create(Product entity);
        void Delete(Product entity);
        void Update(Product entity);
        int GetCountByCategory(string name);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchProducts(string q);
        Product GetByIdWithCategories(int id);
        bool Update(Product entity, int[] categoryIds);
    }
}
