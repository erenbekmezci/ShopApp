using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopApp.business.Abstract;
using ShopApp.webui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductService _productService;
        public HomeController(ILogger<HomeController> logger , IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        public IActionResult Index()
        {

            var productViewModel = new ProductList()
            {
                Products = _productService.GetHomePageProducts()
            };

            return View(productViewModel);
        }

       
    

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
