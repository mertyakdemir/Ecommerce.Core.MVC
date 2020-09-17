using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.Entities;
using Ecommerce.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Controllers
{
    public class StoreController : Controller
    {
        private IProductBusiness productBusiness;

        public StoreController(IProductBusiness _productBusiness)
        {
            this.productBusiness = _productBusiness;
        }

        public IActionResult Index(string category, int page=1)
        {
            const int pagesize = 3;

            var productView = new ProductViewModel()
            {
                Pagination = new Pagination()
                {
                    SumItems = productBusiness.FetchByCategoryProducts(category),
                    CurrentPage = page,
                    ItemsForPage = pagesize,
                    CurrentCategory = category
                },

                Products = productBusiness.TakeCategoryProducts(category, page, pagesize)
            };
            return View(productView);
        }

        public IActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            Product product = productBusiness.GetCategoryDetails((int)id);

            if(id==null)
            {
                return NotFound();
            }
            return View(new ProductCategoryModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });
        }

        public IActionResult Search(string word)
        {
            var productView = new ProductViewModel()
            {
               Products = productBusiness.GetSearchResult(word)
            };
            return View(productView);
        }
    }
}