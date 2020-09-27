using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DataLayer.Abstract
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        List<Product> BestSellerProducts();

        Product GetCategoryDetails(int id);

        List<Product> TakeCategoryProducts(string name, int page, int pagesize);

        int FetchByCategoryProducts(string category);

        List<Product> HomePageProducts();

        List<Product> GetSearchResult(string searchtext);

        Product GetOneWithCategories(int id);

        void Update(Product entity, int[] categoryIds);

        void Create(Product entity, int[] categoryIds);

    }
}
