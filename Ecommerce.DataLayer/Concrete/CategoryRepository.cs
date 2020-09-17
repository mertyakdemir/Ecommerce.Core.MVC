using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DataLayer.Concrete
{
    public class CategoryRepository : GenericRepository<Category, DatabaseContext>, ICategoryRepository
    {
        
    }
}
