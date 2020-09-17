using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ecommerce.UI.Models;
using Ecommerce.UI.ViewModel;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.Entities;

namespace Ecommerce.UI.Controllers
{
    public class HomeController : Controller
    {
        private IProductBusiness productBusiness;

        public HomeController(IProductBusiness _productBusiness)
        {
            this.productBusiness = _productBusiness;
        }

        public IActionResult Index()
        {
            var productView = new ProductViewModel()
            {
                Products = productBusiness.HomePageProducts()
            };

            return View(productView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
