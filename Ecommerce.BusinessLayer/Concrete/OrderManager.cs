using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Concrete
{
    public class OrderManager : IOrderBusiness
    {
        private IOrderRepository orderRepository;

        public OrderManager(IOrderRepository _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        public void Create(Order entity)
        {
            orderRepository.Create(entity);
        }

        public List<Order> GetOrders(string userId)
        {
            return orderRepository.GetOrders(userId);
        }
    }
}
