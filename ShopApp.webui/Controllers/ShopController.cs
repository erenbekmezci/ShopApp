using Microsoft.AspNetCore.Mvc;
using ShopApp.business.Abstract;
using ShopApp.webui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(string category,int page = 1)
        {
            const int pageSize = 3;
            var productViewModel = new ProductList()
            {
                pageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProducsByCategory(category , page , pageSize)
            };

            return View(productViewModel);
        }

        public IActionResult Search(string q)
        {

            var productViewModel = new ProductList()
            {

                Products = _productService.GetSearchProducts(q)
            };

            return View(productViewModel);
        }

        public IActionResult Details(string url)
        {
            if(url == null)
            {
                return NotFound();
            }
            var product = _productService.GetProducDetail(url); //burada ccategory bilgileri left join yapıldı

            if(product == null)
            {
                return NotFound();
            }

            return View(new ProductDetailModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            }) ;
        }
    }
}
