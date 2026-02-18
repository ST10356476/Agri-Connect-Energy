using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.ViewModels;
using Agri_Energy_Connect.Services;

namespace Agri_Energy_Connect.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IFarmerService _farmerService;

        public AccountController(IAuthService authService, IFarmerService farmerService)
        {
            _authService = authService;
            _farmerService = farmerService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
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
                farmer.ProfileImageUrl = "/images/farmers/default.png";
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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = model.RememberMe });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Redirect based on role (ajcvickers, 2024).
            if (user.Role.RoleName == "Farmer")
            {
                var farmer = await _farmerService.GetFarmerByUserIdAsync(user.UserId);
                if (farmer != null)
                {
                    return RedirectToAction("Dashboard", "Farmer");
                }
                return RedirectToAction("Create", "Farmer");
            }
            else if (user.Role.RoleName == "Employee")
            {
                return RedirectToAction("Dashboard", "Employee");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
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

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}


/*References
ajcvickers (2024). Identity model customization in ASP.NET Core. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0. [Accessed 12 May 2025]
*/
