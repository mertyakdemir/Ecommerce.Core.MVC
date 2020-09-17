using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.DataLayer.Concrete
{
    public class ProductRepository : GenericRepository<Product, DatabaseContext>, IProductRepository
    {
        public Product GetCategoryDetails(int id)
        {
            using var dbContext = new DatabaseContext();
            return dbContext.Products.Where(i => i.Id == id)
                                     .Include(i => i.ProductCategories)
                                     .ThenInclude(i => i.Category)
                                     .FirstOrDefault();
        }

        public List<Product> TakeCategoryProducts(string name, int page, int pagesize)
        {
            using var dbContext = new DatabaseContext();

            var products = dbContext.Products.Where(i => i.IsOnline).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(c => c.Category.Url == name));
            }
            return products.Skip((page-1)*pagesize).Take(pagesize).ToList();
        }

        //for pagination
        public int FetchByCategoryProducts(string category)
        {
            using var dbContext = new DatabaseContext();
            var products = dbContext.Products.Where(i => i.IsOnline).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(c => c.Category.Url == category));
            }
            return products.Count();
        }

        public List<Product> HomePageProducts()
        {
            using var dbContext = new DatabaseContext();
            return dbContext.Products.Where(i => i.IsOnline && i.IsMain).ToList();
        }

        public List<Product> GetSearchResult(string searchtext)
        {
            using var dbContext = new DatabaseContext();

            var products = dbContext.Products.Where(i => i.IsOnline && (i.ProductName.ToLower().Contains(searchtext) || i.ProductDescription.ToLower().Contains(searchtext))).AsQueryable();

            return products.ToList();
        }

        public List<Product> BestSellerProducts()
        {
            throw new NotImplementedException();
        }
    }
}
