using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.business.Abstract;
using ShopApp.entites;
using ShopApp.webui.Identity;
using ShopApp.webui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Controllers
{
 
   [Authorize(Roles = "admin")]//mutlaka yetkilendirilimiş kullanıcı lazım yani login olmuş
    public class AdminController : Controller
    {
        private IProductService _productService;

        private ICategoryService _categoryService;

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,IProductService productService , ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            var _user = _userManager.Users.ToList();

            foreach (var user in _user)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)
                                ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NoMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }





                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("RoleEdit");
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;
                return View(new UserDetailModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    Lastname = user.FirstName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailModel model , string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!= null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.Lastname;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/admin/user/list");
                    }
                }
                var roles1 = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles1;
                return View(model);
            }
            var roles = _roleManager.Roles.Select(i => i.Name);
            ViewBag.Roles = roles;
            return View(model);
        }
        public IActionResult ProductList()
        {
            var products = new ProductList()
            {
                Products = _productService.GetAll()
            };

            return View(products.Products);
        }

        public IActionResult CategoryList()
        {
            var categories = new CategoryList()
            {
                Categories = _categoryService.GetAll()
            };

            return View(categories.Categories);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
           

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    Description = model.Description
                };

                if (_productService.Create(entity))
                {
                    TempData["message"] = $"{entity.Name} isimli ürün eklendi";
                    return RedirectToAction("ProductList");
                }

                TempData["message"] = _productService.ErrorMessage;
                return View(model);
                
            }

            return View(model);
           
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {


            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url

                };

                _categoryService.Create(entity);

                TempData["message"] = $"{entity.Name} isimli category eklendi";

                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int? id) 
        {
            if (id == null)
                return NotFound();
            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
                return NotFound();

            var model = new ProductModel()
            { 
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                isApproved = entity.isApproved,
                isHome = entity.isHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds , IFormFile file)
        {
            if (ModelState.IsValid)
            {
               var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                    return NotFound();

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {
                    
                    TempData["message"] = $"{entity.Name} isimli ürün güncellendi";
                    return RedirectToActionPermanent("ProductList");
                }
                TempData["message"] = _productService.ErrorMessage;

            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
                return NotFound();
            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
                return NotFound();

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(i => i.Product).ToList()
                
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                    return NotFound();

                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);
                TempData["message"] = $"{entity.Name} isimli kategori güncellendi";
                return RedirectToActionPermanent("CategoryList");
            }
            return View(model);
           
        }

        [HttpPost]
        public IActionResult DeleteProduct(int? productId)
        {
            if (productId == null)
                return NotFound();
            var entity = _productService.GetById((int)productId);
            if (entity == null)
                return NotFound();
            _productService.Delete(entity);
            TempData["message"] = $"{entity.Name} isimli ürün silindi";
            return RedirectToAction("ProductList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? categoryId)
        {
            if (categoryId == null)
                return NotFound();
            var entity = _categoryService.GetById((int)categoryId);
            if (entity == null)
                return NotFound();
            _categoryService.Delete(entity);
            TempData["message"] = $"{entity.Name} isimli kategori silindi";
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId , int productId)
        {
           
            _categoryService.DeleteFromCategory(categoryId , productId);
        
           
            
            return Redirect("/admin/categories/"+categoryId);
        }


    }
}
