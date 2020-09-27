using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.Entities;
using Ecommerce.UI.Identity;
using Ecommerce.UI.Models;
using Ecommerce.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductBusiness productBusiness;
        private ICategoryBusiness categoryBusiness;
        private UserManager<User> userManager;

        public AdminController(IProductBusiness _productBusiness, ICategoryBusiness _categoryBusiness, UserManager<User> _userManager)
        {
            productBusiness = _productBusiness;
            categoryBusiness = _categoryBusiness;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View(new ProductViewModel()
            {
                Products = productBusiness.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = categoryBusiness.GetAll();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel product, int[] categoryIds, IFormFile file)
        {
            var model = new Product()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                IsOnline = product.IsOnline,
                IsMain = product.IsMain,
                ProductImage = file.FileName
            };

            var extension = Path.GetExtension(file.FileName);
            var randomName = string.Format($"{Guid.NewGuid()}{extension}");
            model.ProductImage = randomName;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", randomName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);


            productBusiness.Create(model, categoryIds);

            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = productBusiness.GetOneWithCategories((int)id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ProductImage = product.ProductImage,
                IsOnline = product.IsOnline,
                IsMain = product.IsMain,
                SelectedCategories = product.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = categoryBusiness.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel product, int[] categoryIds, IFormFile file)
        {
            var model = productBusiness.GetOne(product.Id);

            if (model == null)
            {
                return NotFound();
            }

            model.ProductName = product.ProductName;
            model.ProductDescription = product.ProductDescription;
            model.ProductPrice = product.ProductPrice;
            model.IsOnline = product.IsOnline;
            model.IsMain = product.IsMain;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                model.ProductImage = randomName;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", randomName);
                using FileStream stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            productBusiness.Update(model, categoryIds);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int Id)
        {
            var product = productBusiness.GetOne(Id);

            if (product != null)
            {
                productBusiness.Delete(product);
            }

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryViewModel()
            {
                Categories = categoryBusiness.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel category)
        {
            var model = new Category()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                Url = category.Url,
            };

            categoryBusiness.Create(model);

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = categoryBusiness.GetCategoryWithProducts((int)id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = category.Id,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                Url = category.Url,
                Products = category.CategoryProducts.Select(p => p.Product).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel category)
        {
            var model = categoryBusiness.GetOne(category.CategoryId);

            if (model == null)
            {
                return NotFound();
            }

            model.CategoryName = category.CategoryName;
            model.CategoryDescription = category.CategoryDescription;
            model.Url = category.Url;

            categoryBusiness.Update(model);

            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int Id)
        {
            var category = categoryBusiness.GetOne(Id);

            if (category != null)
            {
                categoryBusiness.Delete(category);
            }

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int Id, int CategoryId)
        {
            categoryBusiness.DeleteFromCategory(Id, CategoryId);
            return Redirect("/admin/categories/" + CategoryId);
        }

    }
}