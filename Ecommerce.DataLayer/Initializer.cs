using Ecommerce.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.DataLayer
{
    public static class Initializer
    {
       public static void Seed()
        {
            var context = new DatabaseContext();


            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRangeAsync(productCategories);
                }

                context.SaveChanges();
            }
        }

        private static Category[] Categories = {
            new Category() { CategoryName="Phone", Url="phone"},
            new Category() { CategoryName="Computer", Url="computer"},
            new Category() { CategoryName="Apple", Url="apple"},
            new Category() { CategoryName="Samsung", Url="samsung"},
            new Category() { CategoryName="Technology", Url="technology"},
        };

        private static Product[] Products =
        {
            new Product(){ ProductName="Iphone", ProductPrice=1000, ProductImage="1.jpg"},
            new Product(){ ProductName="Samsung", ProductPrice=800, ProductImage="2.jpg"},
            new Product(){ ProductName="Huawei", ProductPrice=700, ProductImage="3.jpg"},
            new Product(){ ProductName="Xiaomi", ProductPrice=600, ProductImage="4.jpg"},
            new Product(){ ProductName="Apple", ProductPrice=5000, ProductImage="5.jpg"},
            new Product(){ ProductName="Asus", ProductPrice=3000, ProductImage="6.jpg"},
            new Product(){ ProductName="Lenovo", ProductPrice=2000, ProductImage="7.jpg"}
        };

        private static ProductCategory[] productCategories =
        {
            new ProductCategory() {Product=Products[0], Category=Categories[0]},
            new ProductCategory() {Product=Products[0], Category=Categories[2]},
            new ProductCategory() {Product=Products[0], Category=Categories[4]},
            new ProductCategory() {Product=Products[1], Category=Categories[0]},
            new ProductCategory() {Product=Products[1], Category=Categories[3]},
            new ProductCategory() {Product=Products[2], Category=Categories[0]},
            new ProductCategory() {Product=Products[3], Category=Categories[0]},
            new ProductCategory() {Product=Products[4], Category=Categories[1]},
            new ProductCategory() {Product=Products[4], Category=Categories[2]},
            new ProductCategory() {Product=Products[4], Category=Categories[4]},
            new ProductCategory() {Product=Products[5], Category=Categories[1]},
            new ProductCategory() {Product=Products[5], Category=Categories[4]},
            new ProductCategory() {Product=Products[6], Category=Categories[1]},
            new ProductCategory() {Product=Products[6], Category=Categories[4]},
        };
    }
}
