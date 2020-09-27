using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Abstract
{
    public interface ICategoryBusiness
    {
        Category GetOne(int id);

        List<Category> GetAll();

        void Create(Category entity);

        void Update(Category entity);

        void Delete(Category entity);

        Category GetCategoryWithProducts(int categoryId);

        void DeleteFromCategory(int Id, int CategoryId);
    }
}
