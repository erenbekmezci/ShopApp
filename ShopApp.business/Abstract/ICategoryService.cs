using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        List<Category> GetAll();

        Category GetById(int id);

        void Create(Category entity);
        void Delete(Category entity);
        void Update(Category entity);
        Category GetByIdWithProducts(int id);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
