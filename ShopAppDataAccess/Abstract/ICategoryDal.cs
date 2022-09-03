using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAppDataAccess.Abstract
{
    public interface ICategoryDal : IRepository<Category>
    {
        Category GetByIdWithProducts(int id);

        void DeleteFromCategory(int categoryId ,int productId);
    }
}
