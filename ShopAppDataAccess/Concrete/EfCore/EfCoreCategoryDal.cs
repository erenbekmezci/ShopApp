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
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using(var db = new ShopContext())
            {
                string cmd = "delete  from \"ProductCategory\" where \"ProductId\"  = @p0 and \"CategoryId\" = @p1";

                db.Database.ExecuteSqlRaw(cmd, productId, categoryId);

            }
        }

        public Category GetByIdWithProducts(int id)
        {
            using (var db = new ShopContext())
            {
                var category = db.Categories.Where(i => i.CategoryId == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Product).FirstOrDefault();

                return category;
            }
        }
    }
}
