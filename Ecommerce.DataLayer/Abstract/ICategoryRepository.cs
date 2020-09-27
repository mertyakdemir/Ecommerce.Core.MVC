using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DataLayer.Abstract
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Category GetCategoryWithProducts(int categoryId);

        void DeleteFromCategory(int Id, int CategoryId);
    }
}
