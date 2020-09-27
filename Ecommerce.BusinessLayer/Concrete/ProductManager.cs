using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.DataLayer.Abstract;
using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Concrete
{
    public class ProductManager : IProductBusiness
    {
        private IProductRepository productRepository;

        public ProductManager(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        public void Create(Product entity)
        {
            productRepository.Create(entity);
        }

        public void Create(Product entity, int[] categoryIds)
        {
            productRepository.Create(entity, categoryIds);
        }

        public void Update(Product entity)
        {
            productRepository.Update(entity);
        }

        public void Update(Product entity, int[] categoryIds)
        {
            productRepository.Update(entity, categoryIds);
        }

        public void Delete(Product entity)
        {
            productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return productRepository.GetAll();
        }

        public Product GetCategoryDetails(int id)
        {
            return productRepository.GetCategoryDetails(id);
        }

        public Product GetOne(int id)
        {
            return productRepository.GetOne(id);
        }

        public List<Product> TakeCategoryProducts(string name, int page, int pagesize)
        {
            return productRepository.TakeCategoryProducts(name, page, pagesize);
        }

        public int FetchByCategoryProducts(string category)
        {
            return productRepository.FetchByCategoryProducts(category);
        }

        public List<Product> HomePageProducts()
        {
            return productRepository.HomePageProducts();
        }

        public List<Product> GetSearchResult(string searchtext)
        { 
            return productRepository.GetSearchResult(searchtext);
        }

        public Product GetOneWithCategories(int id)
        {
            return productRepository.GetOneWithCategories(id);
        }
    }
}
