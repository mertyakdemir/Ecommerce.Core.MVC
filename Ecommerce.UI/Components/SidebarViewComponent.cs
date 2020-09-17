using Ecommerce.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private ICategoryBusiness categoryBusiness;

        public SidebarViewComponent(ICategoryBusiness _categoryBusiness)
        {
            this.categoryBusiness = _categoryBusiness;
        }

        public IViewComponentResult Invoke()
        {
            return View(categoryBusiness.GetAll());
        }
    }
}
