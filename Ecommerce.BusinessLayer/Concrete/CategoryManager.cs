using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.DataLayer.Abstract;
using Ecommerce.DataLayer.Concrete;
using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryBusiness
    {
        private ICategoryRepository categoryRepository;

        public CategoryManager(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public void Create(Category entity)
        {
            categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            categoryRepository.Delete(entity);
        }

        public List<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public Category GetOne(int id)
        {
            return categoryRepository.GetOne(id);
        }

        public void Update(Category entity)
        {
            categoryRepository.Update(entity);
        }

        public Category GetCategoryWithProducts(int categoryId)
        {
            return categoryRepository.GetCategoryWithProducts(categoryId);
        }

        public void DeleteFromCategory(int Id, int CategoryId)
        {
            categoryRepository.DeleteFromCategory(Id, CategoryId);
        }
    }
}
