using ShopApp.business.Abstract;
using ShopApp.entites;
using ShopAppDataAccess.Abstract;
using ShopAppDataAccess.Concrete.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.business.Concrete
{
    public class ProductManager : IProductService
    {
        //EfCoreProductDal _productDal = new EfCoreProductDal(); //bağımlılık çok fazla

        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }


        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productDal.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            _productDal.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll().ToList();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productDal.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string name)
        {
            return _productDal.GetCountByCategory(name);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productDal.GetHomePageProducts();
        }

        public List<Product> GetPopulerProducts()
        {
            return _productDal.GetAll(p=> p.Price>5000).ToList();
        }

        public Product GetProducDetail(string productName)
        {
            return _productDal.GetProducDetail(productName);
        }

        public List<Product> GetProducsByCategory(string name, int page, int pageSize)
        {
            return _productDal.GetProducsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchProducts(string q)
        {
            return _productDal.GetSearchProducts(q);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz.";
                    return false;
                }
                _productDal.Update(entity, categoryIds);
                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz.\n";
                isValid = false;
            }

            if (entity.Price < 0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz.\n";
                isValid = false;
            }

            return isValid;
        }
    }
}
