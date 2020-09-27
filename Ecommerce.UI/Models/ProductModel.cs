using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public bool IsOnline { get; set; }
        public bool IsMain { get; set; }
        public List<Category> SelectedCategories { get; set; }
        public List<Category> SelectCategory { get; set; }
    }
}
