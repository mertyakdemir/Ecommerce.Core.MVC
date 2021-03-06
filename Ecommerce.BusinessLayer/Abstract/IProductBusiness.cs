﻿using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Abstract
{
    public interface IProductBusiness
    {
        Product GetOne(int id);

        List<Product> GetAll();

        void Create(Product entity);

        void Create(Product model, int[] categoryIds);

        void Update(Product entity);

        void Update(Product model, int[] categoryIds);

        void Delete(Product entity);

        Product GetCategoryDetails(int id);

        List<Product> TakeCategoryProducts(string name, int page, int pagesize);

        int FetchByCategoryProducts(string category);

        List<Product> HomePageProducts();

        List<Product> GetSearchResult(string searchtext);

        Product GetOneWithCategories(int id);
    }
}
