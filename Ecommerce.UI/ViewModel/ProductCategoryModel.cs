using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.ViewModel
{
    public class ProductCategoryModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
