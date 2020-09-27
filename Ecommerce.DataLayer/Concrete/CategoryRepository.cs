using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.DataLayer.Concrete
{
    public class CategoryRepository : GenericRepository<Category, DatabaseContext>, ICategoryRepository
    {
        public Category GetCategoryWithProducts(int categoryId)
        {
            using var dbContext = new DatabaseContext();

            return dbContext.Categories
                        .Where(i => i.Id == categoryId)
                        .Include(i => i.CategoryProducts)
                        .ThenInclude(i => i.Product)
                        .FirstOrDefault();
        }

        public void DeleteFromCategory(int Id, int CategoryId)
        {
            using var dbContext = new DatabaseContext();
            var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1";
            dbContext.Database.ExecuteSqlRaw(cmd, Id, CategoryId);
        }
    }
}
