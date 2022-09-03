using Microsoft.EntityFrameworkCore;
using ShopApp.entites;
using ShopAppDataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopAppDataAccess.Concrete.EfCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public void DeleteFromCart(int cartid, int productid)
        {
            using(var db = new ShopContext())
            {
                string cmd = "delete  from \"CartItems\" where \"ProductId\"  = @p0 and \"CartId\" = @p1";
                db.Database.ExecuteSqlRaw(cmd, productid,cartid);

            }


        }

        public Cart GetByUserId(string userid)
        {
            using(var db = new ShopContext())
            {
                return db.Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserId == userid);
            }
        }

        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

    }
}
