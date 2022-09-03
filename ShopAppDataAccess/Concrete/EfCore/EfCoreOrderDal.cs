using ShopApp.entites;
using ShopAppDataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAppDataAccess.Concrete.EfCore
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order , ShopContext> , IOrderDal
    {

    }
}
