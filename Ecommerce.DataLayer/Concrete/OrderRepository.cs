using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.DataLayer.Concrete
{
    public class OrderRepository : GenericRepository<Order, DatabaseContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using var dbContext = new DatabaseContext();
            var orders = dbContext.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();

            if(!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(i => i.UserId == userId);
            }
            return orders.ToList(); 
        }
    }
}
