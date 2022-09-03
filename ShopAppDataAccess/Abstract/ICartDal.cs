using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAppDataAccess.Abstract
{
    public interface ICartDal : IRepository<Cart>
    {
        Cart GetByUserId(string userid);
        void DeleteFromCart(int cartid, int productid);
    }
}
