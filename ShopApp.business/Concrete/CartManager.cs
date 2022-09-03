using ShopApp.business.Abstract;
using ShopApp.entites;
using ShopAppDataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public void AddToCart(string userid, int productid, int quantity)
        {
            var cart = GetCartByUserId(userid);

            if(cart != null)
            {
                var index = cart.CartItems.FindIndex(i=> i.ProductId == productid); //o ürüne ait bir kayıt ar mı varsa sadece update
                if(index<0) // yoksa yeni kayıt ekleme
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productid,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartDal.Update(cart);
            }
        }

        public void DeleteFromCart(string userid, int productid)
        {
            var cart = GetCartByUserId(userid);
            if(cart != null)
            {
                _cartDal.DeleteFromCart(cart.Id, productid);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartDal.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartDal.Create(new entites.Cart() { UserId = userId });
        }
    }
}
