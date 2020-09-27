using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DataLayer.Abstract
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
