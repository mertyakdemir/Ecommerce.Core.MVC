using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.DataLayer.Concrete
{
    public class CartRepository : GenericRepository<Cart, DatabaseContext>, ICartRepository
    {
        public Cart GetByUserId(string userId)
        {
            using var dbContext = new DatabaseContext();
            return dbContext.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);
        }

        public override void Update(Cart entity)
        {
            using var dbContext = new DatabaseContext();
            dbContext.Carts.Update(entity);
            dbContext.SaveChanges();
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using var dbContext = new DatabaseContext();
            var cmd = @"delete from CartItems where CartId=@p0 And ProductId=@p1";
            dbContext.Database.ExecuteSqlRaw(cmd, cartId, productId);

        }

        public void ClearCart(int cartId)
        {
            using var dbContext = new DatabaseContext();
            var cmd = @"delete from CartItems where CartId=@p0";
            dbContext.Database.ExecuteSqlRaw(cmd, cartId);
        }
    }
}
