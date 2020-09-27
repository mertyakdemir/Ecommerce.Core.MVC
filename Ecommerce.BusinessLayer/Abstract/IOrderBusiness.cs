using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Abstract
{
    public interface IOrderBusiness
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}
