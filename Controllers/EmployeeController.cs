using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Agri_Energy_Connect.Controllers
{
    [Authorize(Roles = "Employee,Administrator")]
    public class EmployeeController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IProductCategoryService _categoryService;
        private readonly IAuthService _authService;

        public EmployeeController(
            IFarmerService farmerService,
            IProductService productService,
            IUserService userService,
            IProductCategoryService categoryService,
            IAuthService authService)
        {
            _farmerService = farmerService;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _authService = authService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var farmers = await _farmerService.GetAllFarmersAsync();
            var products = await _productService.GetAllProductsAsync();

            var viewModel = new EmployeeDashboardViewModel
            {
                FarmerCount = farmers.Count(),
                ProductCount = products.Count(),
                RecentFarmers = farmers.Take(5),
                RecentProducts = products.Take(5)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Farmers()
        {
            var farmers = await _farmerService.GetAllFarmersAsync();
            return View(farmers);
        }

        public async Task<IActionResult> FarmerDetails(int id)
        {
            var farmer = await _farmerService.GetFarmerByIdAsync(id);

            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        public IActionResult AddFarmer()
        {
            return View(new FarmerRegistrationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(FarmerRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _authService.RegisterUserAsync(user, model.Password, "Farmer");
            if (!result.success)
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }

            var farmer = new Farmer
            {
                UserId = user.UserId,
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
                ProfileDescription = model.ProfileDescription
            };

            // Handle profile image upload
            if (model.ProfileImage != null)
            {
                // This is a placeholder for demonstration purposes
                farmer.ProfileImageUrl = "/images/farmers/default.png";
            }

            await _farmerService.AddFarmerAsync(farmer);

            TempData["SuccessMessage"] = "Farmer added successfully.";
            return RedirectToAction("Farmers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyFarmer(int id)
        {
            var farmer = await _farmerService.GetFarmerByIdAsync(id);

            if (farmer == null)
            {
                return NotFound();
            }

            farmer.IsVerified = true;
            await _farmerService.UpdateFarmerAsync(farmer);

            TempData["SuccessMessage"] = "Farmer verified successfully.";
            return RedirectToAction("FarmerDetails", new { id = id });
        }

        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> FilterProducts()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var farmers = await _farmerService.GetAllFarmersAsync();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            ViewBag.Farmers = new SelectList(farmers, "FarmerId", "FarmName");

            return View(new FilterProductsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterProducts(FilterProductsViewModel model)
        {
            var products = await _productService.GetAllProductsAsync();

            // Apply filters
            if (model.FarmerId.HasValue)
            {
                products = products.Where(p => p.FarmerId == model.FarmerId.Value);
            }

            if (model.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == model.CategoryId.Value);
            }

            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= model.StartDate.Value && p.ProductionDate <= model.EndDate.Value);
            }
            else if (model.StartDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= model.StartDate.Value);
            }
            else if (model.EndDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate <= model.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(model.ProductName))
            {
                products = products.Where(p => p.ProductName.Contains(model.ProductName, StringComparison.OrdinalIgnoreCase));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            var farmers = await _farmerService.GetAllFarmersAsync();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            ViewBag.Farmers = new SelectList(farmers, "FarmerId", "FarmName");

            model.Products = products;

            return View(model);
        }

        public async Task<IActionResult> Reports()
        {
            // Placeholder for future reporting functionality
            return View();
        }

        public async Task<IActionResult> GenerateReport(string reportType)
        {
            // Placeholder for future report generation functionality
            switch (reportType)
            {
                case "farmers":
                    // Generate farmers report
                    break;
                case "products":
                    // Generate products report
                    break;
                case "sustainability":
                    // Generate sustainability report
                    break;
                default:
                    return NotFound();
            }

            TempData["SuccessMessage"] = "Report generated successfully.";
            return RedirectToAction("Reports");
        }

        public async Task<IActionResult> Settings()
        {
            // Placeholder for future settings functionality
            return View();
        }
    }
}
