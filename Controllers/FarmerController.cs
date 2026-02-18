// Controllers/FarmerController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.ViewModels;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;


namespace Agri_Energy_Connect.Controllers
{
    [Authorize(Roles = "Farmer,Employee,Administrator")]
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IProductCategoryService _categoryService;

        public FarmerController(
            IFarmerService farmerService,
            IProductService productService,
            IUserService userService,
            IProductCategoryService categoryService)
        {
            _farmerService = farmerService;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var products = await _productService.GetProductsByFarmerIdAsync(farmer.FarmerId);

            var viewModel = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                RecentProducts = products
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create(FarmerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var farmer = new Farmer
            {
                UserId = userId,
                FarmName = model.FarmName,
                RegistrationNumber = model.RegistrationNumber,
                EstablishedDate = model.EstablishedDate,
                Address = model.Address,
                City = model.City,
                Province = model.Province,
                PostalCode = model.PostalCode,
                FarmSize = model.FarmSize,
                FarmSizeUnit = model.FarmSizeUnit,
                FarmingType = model.FarmingType,
                MainCrops = model.MainCrops,
                MainLivestock = model.MainLivestock,
                SustainabilityPractices = model.SustainabilityPractices,
                ProfileDescription = model.ProfileDescription,
                EnergyNeeds = model.EnergyNeeds
            };

            // Handle profile image upload
            if (model.ProfileImage != null)
            {

                // This is a placeholder for demonstration purposes
                farmer.ProfileImageUrl = "/images/defaultimage.jpg";
            }

            await _farmerService.AddFarmerAsync(farmer);

            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var model = new FarmerViewModel
            {
                FarmName = farmer.FarmName,
                RegistrationNumber = farmer.RegistrationNumber,
                EstablishedDate = farmer.EstablishedDate,
                Address = farmer.Address,
                City = farmer.City,
                Province = farmer.Province,
                PostalCode = farmer.PostalCode,
                FarmSize = farmer.FarmSize,
                FarmSizeUnit = farmer.FarmSizeUnit,
                FarmingType = farmer.FarmingType,
                MainCrops = farmer.MainCrops,
                MainLivestock = farmer.MainLivestock,
                SustainabilityPractices = farmer.SustainabilityPractices,
                ProfileDescription = farmer.ProfileDescription,
                EnergyNeeds = farmer.EnergyNeeds
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(FarmerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            farmer.FarmName = model.FarmName;
            farmer.RegistrationNumber = model.RegistrationNumber;
            farmer.EstablishedDate = model.EstablishedDate;
            farmer.Address = model.Address;
            farmer.City = model.City;
            farmer.Province = model.Province;
            farmer.PostalCode = model.PostalCode;
            farmer.FarmSize = model.FarmSize;
            farmer.FarmSizeUnit = model.FarmSizeUnit;
            farmer.FarmingType = model.FarmingType;
            farmer.MainCrops = model.MainCrops;
            farmer.MainLivestock = model.MainLivestock;
            farmer.SustainabilityPractices = model.SustainabilityPractices;
            farmer.ProfileDescription = model.ProfileDescription;
            farmer.EnergyNeeds = model.EnergyNeeds;

            // Handle profile image upload
            if (model.ProfileImage != null)
            {
                // This is a placeholder for demonstration purposes
                farmer.ProfileImageUrl = "/images/farmers/default.png";
            }

            await _farmerService.UpdateFarmerAsync(farmer);

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Products()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var products = await _productService.GetProductsByFarmerIdAsync(farmer.FarmerId);
            return View(products);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
                return View(model);
            }

            var product = new Product
            {
                FarmerId = farmer.FarmerId,
                ProductName = model.ProductName,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ProductionDate = model.ProductionDate,
                Quantity = model.Quantity,
                UnitOfMeasure = model.UnitOfMeasure,
                Price = model.Price,
                CurrencyCode = "ZAR",
                SustainabilityFeatures = model.SustainabilityFeatures,
                OrganicCertified = model.OrganicCertified,
                IsAvailable = true
            };

            // Handle product image upload
            if (model.ProductImage != null)
            {
                // This is a placeholder for demonstration purposes
                product.ImageUrl = "/images/products/default.png";
            }

            await _productService.AddProductAsync(product);

            TempData["SuccessMessage"] = "Product added successfully.";
            return RedirectToAction("Products");
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null || product.FarmerId != farmer.FarmerId)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Description = product.Description,
                ProductionDate = product.ProductionDate,
                Quantity = product.Quantity,
                UnitOfMeasure = product.UnitOfMeasure,
                Price = product.Price,
                SustainabilityFeatures = product.SustainabilityFeatures,
                OrganicCertified = product.OrganicCertified
            };

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> EditProduct(ProductViewModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
                return View(model);
            }

            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product == null || product.FarmerId != farmer.FarmerId)
            {
                return NotFound();
            }

            product.ProductName = model.ProductName;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.ProductionDate = model.ProductionDate;
            product.Quantity = model.Quantity;
            product.UnitOfMeasure = model.UnitOfMeasure;
            product.Price = model.Price;
            product.SustainabilityFeatures = model.SustainabilityFeatures;
            product.OrganicCertified = model.OrganicCertified;

            // Handle product image upload
            if (model.ProductImage != null)
            {
                // This is a placeholder for demonstration purposes
                product.ImageUrl = "/images/products/default.png";
            }

            await _productService.UpdateProductAsync(product);

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);

            if (farmer == null)
            {
                return RedirectToAction("Create");
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null || product.FarmerId != farmer.FarmerId)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToAction("Products");
        }
    }
}
