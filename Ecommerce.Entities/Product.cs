using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecommerce.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public bool IsOnline { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public bool IsMain { get; set; }
    }
}
