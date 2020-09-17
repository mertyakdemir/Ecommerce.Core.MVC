using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.ViewModel
{
    public class Pagination
    {
        public int SumItems { get; set; }
        public int ItemsForPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

        public int SumPages()
        {
            return (int)Math.Ceiling((decimal)SumItems / ItemsForPage);
        }
    }

    public class ProductViewModel
    {
        public Pagination Pagination { get; set; }
        public List<Product> Products { get; set; }
    }
}
