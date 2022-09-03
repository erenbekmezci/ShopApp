using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);

        Cart GetCartByUserId(string userId);

        void AddToCart(string userid, int productid, int quantity);

        void DeleteFromCart(string userid, int productid);
        
    }
}
