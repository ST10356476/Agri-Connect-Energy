// Data/SeedData.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.Data;


namespace Agri_Energy_Connect.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AgriEnergyConnectContext>();
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                    // Apply pending migrations
                    await context.Database.MigrateAsync();

                    // Seed data
                    await SeedRolesAsync(context);
                    await SeedUsersAsync(context, authService);
                    await SeedProductCategoriesAsync(context);
                    await SeedFarmersAsync(context);
                    await SeedProductsAsync(context);
                    await SeedEnergySolutionCategoriesAsync(context);
                    await SeedEnergySolutionProvidersAsync(context);
                    await SeedEnergySolutionsAsync(context);
                }

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedRolesAsync(AgriEnergyConnectContext context)
        {
            if (await context.Roles.AnyAsync())
                return;

            var roles = new List<Role>
            {
                new Role
                {
                    RoleName = "Administrator",
                    Description = "System administrator with full access",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Role
                {
                    RoleName = "Employee",
                    Description = "Staff member with management capabilities",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Role
                {
                    RoleName = "Farmer",
                    Description = "Agricultural producer with product management capabilities",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Role
                {
                    RoleName = "EnergyProvider",
                    Description = "Provider of green energy solutions",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(AgriEnergyConnectContext context, IAuthService authService)
        {
            if (await context.Users.AnyAsync())
                return;

            // Get roles
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Administrator");
            var employeeRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Employee");
            var farmerRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Farmer");

            // Create admin user
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@agrienergyconnect.co.za",
                FirstName = "System",
                LastName = "Administrator",
                PhoneNumber = "+27123456789",
                RoleId = adminRole.RoleId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            string salt1;
            adminUser.PasswordHash = authService.HashPassword("Admin@123", out salt1);
            adminUser.PasswordSalt = salt1;

            // Create employee user
            var employeeUser = new User
            {
                Username = "employee",
                Email = "employee@agrienergyconnect.co.za",
                FirstName = "Test",
                LastName = "Employee",
                PhoneNumber = "+27123456780",
                RoleId = employeeRole.RoleId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            string salt2;
            employeeUser.PasswordHash = authService.HashPassword("Employee@123", out salt2);
            employeeUser.PasswordSalt = salt2;

            // Fix for CS1620: Ensure the 'out' keyword is used for the second argument in the HashPassword method calls (dotnet.bot, 2025).

            var farmerUsers = new List<User>
            {
                new User
                {
                    Username = "farmer1",
                    Email = "farmer1@example.com",
                    FirstName = "Thabo",
                    LastName = "Nkosi",
                    PhoneNumber = "+27781234567",
                    RoleId = farmerRole.RoleId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = "SampleSalt", // In a real app, would use authService.GenerateSalt()
                    PasswordHash = authService.HashPassword("Farmer@123", out var salt3)
                },
                new User
                {
                    Username = "farmer2",
                    Email = "farmer2@example.com",
                    FirstName = "Lerato",
                    LastName = "Molefe",
                    PhoneNumber = "+27821234567",
                    RoleId = farmerRole.RoleId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = salt1, // Reuse the salt generated above
                    PasswordHash = authService.HashPassword("Farmer@123", out var salt4)
                },
                new User
                {
                    Username = "farmer3",
                    Email = "farmer3@example.com",
                    FirstName = "Daniel",
                    LastName = "van der Merwe",
                    PhoneNumber = "+27731234567",
                    RoleId = farmerRole.RoleId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = salt2, // Reuse the salt generated above
                    PasswordHash = authService.HashPassword("Farmer@123", out var salt5)
                }
            };

            await context.Users.AddAsync(adminUser);
            await context.Users.AddAsync(employeeUser);
            await context.Users.AddRangeAsync(farmerUsers);
            await context.SaveChangesAsync();
        }



        private static async Task SeedProductCategoriesAsync(AgriEnergyConnectContext context)
        {
            if (await context.ProductCategories.AnyAsync())
                return;

            var categories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    CategoryName = "Fruits",
                    Description = "Fresh fruits and fruit products",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryName = "Vegetables",
                    Description = "Fresh vegetables and vegetable products",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryName = "Grains",
                    Description = "Grains, cereals, and related products",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryName = "Livestock",
                    Description = "Live animals and animal products",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryName = "Dairy",
                    Description = "Milk and dairy products",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryName = "Other",
                    Description = "Miscellaneous agricultural products",
                    IsActive = true
                }
            };

            await context.ProductCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedFarmersAsync(AgriEnergyConnectContext context)
        {
            if (await context.Farmers.AnyAsync())
                return;

            // Get farmer users
            var farmerUsers = await context.Users
                .Where(u => u.Role.RoleName == "Farmer")
                .ToListAsync();

            if (farmerUsers.Count == 0)
                return;

            var farmers = new List<Farmer>
            {
                new Farmer
                {
                    UserId = farmerUsers[0].UserId,
                    FarmName = "Sunrise Organic Farm",
                    RegistrationNumber = "F12345",
                    EstablishedDate = new DateTime(2010, 3, 15),
                    Address = "123 Farm Road",
                    City = "Polokwane",
                    Province = "Limpopo",
                    PostalCode = "0700",
                    FarmSize = 75.5m,
                    FarmSizeUnit = "hectares",
                    FarmingType = "Organic Farming",
                    MainCrops = "Tomatoes, Spinach, Maize",
                    MainLivestock = "",
                    SustainabilityPractices = "We use companion planting, crop rotation, and natural pest control methods. Our farm is completely free from synthetic pesticides and fertilizers.",
                    ProfileDescription = "Sunrise Organic Farm is a family-owned farm committed to sustainable agricultural practices. We specialize in growing high-quality organic vegetables for local markets.",
                    ProfileImageUrl = "/images/farmers/farmer1.jpg",
                    EnergyNeeds = "We currently use diesel generators for irrigation pumps and are looking for solar alternatives to reduce our carbon footprint and operational costs.",
                    CreatedDate = DateTime.Now,
                    IsVerified = true
                },
                new Farmer
                {
                    UserId = farmerUsers[1].UserId,
                    FarmName = "Green Valley Farm",
                    RegistrationNumber = "F23456",
                    EstablishedDate = new DateTime(2015, 6, 10),
                    Address = "456 Valley Road",
                    City = "Stellenbosch",
                    Province = "Western Cape",
                    PostalCode = "7600",
                    FarmSize = 125.0m,
                    FarmSizeUnit = "hectares",
                    FarmingType = "Mixed Farming",
                    MainCrops = "Grapes, Olives, Vegetables",
                    MainLivestock = "Cattle, Sheep",
                    SustainabilityPractices = "We implement water conservation through drip irrigation and rainwater harvesting. Our vineyard uses integrated pest management and minimal chemical inputs.",
                    ProfileDescription = "Green Valley Farm is situated in the heart of wine country, focusing on sustainable wine grape production alongside olive groves and mixed vegetables. We also raise grass-fed cattle and sheep.",
                    ProfileImageUrl = "/images/farmers/farmer2.jpg",
                    EnergyNeeds = "We are interested in biogas solutions for our livestock waste and solar power for our processing facilities.",
                    CreatedDate = DateTime.Now,
                    IsVerified = true
                },
                new Farmer
                {
                    UserId = farmerUsers[2].UserId,
                    FarmName = "Highland Livestock Farm",
                    RegistrationNumber = "F34567",
                    EstablishedDate = new DateTime(2008, 9, 20),
                    Address = "789 Mountain View",
                    City = "Harrismith",
                    Province = "Free State",
                    PostalCode = "9880",
                    FarmSize = 850.0m,
                    FarmSizeUnit = "hectares",
                    FarmingType = "Livestock Farming",
                    MainCrops = "Pasture, Corn, Hay",
                    MainLivestock = "Cattle, Sheep, Goats",
                    SustainabilityPractices = "Our animals are grass-fed and raised using rotational grazing methods. We maintain natural grasslands and avoid overgrazing to preserve soil health.",
                    ProfileDescription = "Highland Livestock Farm specializes in free-range, ethically raised livestock. Our cattle, sheep, and goats graze on natural pastures in the scenic highlands of the Free State.",
                    ProfileImageUrl = "/images/farmers/farmer3.jpg",
                    EnergyNeeds = "We need energy solutions for water pumping across large pasture areas and are exploring wind energy options due to our elevated location.",
                    CreatedDate = DateTime.Now,
                    IsVerified = false
                }
            };

            await context.Farmers.AddRangeAsync(farmers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(AgriEnergyConnectContext context)
        {
            if (await context.Products.AnyAsync())
                return;

            // Get farmers and categories
            var farmers = await context.Farmers.ToListAsync();
            var categories = await context.ProductCategories.ToListAsync();

            if (farmers.Count == 0 || categories.Count == 0)
                return;

            var fruitCategory = categories.FirstOrDefault(c => c.CategoryName == "Fruits");
            var vegetableCategory = categories.FirstOrDefault(c => c.CategoryName == "Vegetables");
            var grainsCategory = categories.FirstOrDefault(c => c.CategoryName == "Grains");
            var livestockCategory = categories.FirstOrDefault(c => c.CategoryName == "Livestock");
            var dairyCategory = categories.FirstOrDefault(c => c.CategoryName == "Dairy");

            var products = new List<Product>
            {
                // Products for Farmer 1
                new Product
                {
                    FarmerId = farmers[0].FarmerId,
                    ProductName = "Organic Tomatoes",
                    CategoryId = vegetableCategory.CategoryId,
                    Description = "Fresh, organic tomatoes grown without synthetic pesticides or fertilizers.",
                    ProductionDate = DateTime.Now.AddDays(-5),
                    Quantity = 500.0m,
                    UnitOfMeasure = "kg",
                    Price = 15.50m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown using companion planting and natural pest control methods. Harvested at peak ripeness.",
                    OrganicCertified = true,
                    ImageUrl = "/images/products/tomatoes.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[0].FarmerId,
                    ProductName = "Organic Spinach",
                    CategoryId = vegetableCategory.CategoryId,
                    Description = "Fresh, nutrient-rich organic spinach.",
                    ProductionDate = DateTime.Now.AddDays(-2),
                    Quantity = 200.0m,
                    UnitOfMeasure = "kg",
                    Price = 18.75m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown in soil enriched with natural compost. Uses 40% less water than conventional farming methods.",
                    OrganicCertified = true,
                    ImageUrl = "/images/products/spinach.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[0].FarmerId,
                    ProductName = "Organic Maize",
                    CategoryId = grainsCategory.CategoryId,
                    Description = "Organically grown maize, perfect for milling or cooking.",
                    ProductionDate = DateTime.Now.AddDays(-30),
                    Quantity = 1500.0m,
                    UnitOfMeasure = "kg",
                    Price = 8.25m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown using traditional heirloom seeds and no chemical inputs.",
                    OrganicCertified = true,
                    ImageUrl = "/images/products/maize.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },

                // Products for Farmer 2
                new Product
                {
                    FarmerId = farmers[1].FarmerId,
                    ProductName = "Wine Grapes",
                    CategoryId = fruitCategory.CategoryId,
                    Description = "Premium wine grapes grown in the rich soil of Western Cape.",
                    ProductionDate = DateTime.Now.AddDays(-15),
                    Quantity = 3000.0m,
                    UnitOfMeasure = "kg",
                    Price = 22.50m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown using integrated pest management with minimal chemical inputs.",
                    OrganicCertified = false,
                    ImageUrl = "/images/products/grapes.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[1].FarmerId,
                    ProductName = "Olives",
                    CategoryId = fruitCategory.CategoryId,
                    Description = "Fresh olives for oil production or table consumption.",
                    ProductionDate = DateTime.Now.AddDays(-10),
                    Quantity = 800.0m,
                    UnitOfMeasure = "kg",
                    Price = 35.00m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown in water-efficient groves with drip irrigation systems.",
                    OrganicCertified = false,
                    ImageUrl = "/images/products/olives.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[1].FarmerId,
                    ProductName = "Grass-Fed Beef",
                    CategoryId = livestockCategory.CategoryId,
                    Description = "Premium grass-fed beef from free-range cattle.",
                    ProductionDate = DateTime.Now.AddDays(-7),
                    Quantity = 500.0m,
                    UnitOfMeasure = "kg",
                    Price = 95.00m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Cattle raised on natural pastures without growth hormones or antibiotics.",
                    OrganicCertified = false,
                    ImageUrl = "/images/grassfedbeef",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },

                // Products for Farmer 3
                new Product
                {
                    FarmerId = farmers[2].FarmerId,
                    ProductName = "Free-Range Lamb",
                    CategoryId = livestockCategory.CategoryId,
                    Description = "Tender, free-range lamb raised in the highlands.",
                    ProductionDate = DateTime.Now.AddDays(-3),
                    Quantity = 300.0m,
                    UnitOfMeasure = "kg",
                    Price = 110.00m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Sheep raised using rotational grazing methods that preserve natural grasslands.",
                    OrganicCertified = false,
                    ImageUrl = "/images/products/lamb.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[2].FarmerId,
                    ProductName = "Goat Milk",
                    CategoryId = dairyCategory.CategoryId,
                    Description = "Fresh, raw goat milk from free-range goats.",
                    ProductionDate = DateTime.Now.AddDays(-1),
                    Quantity = 150.0m,
                    UnitOfMeasure = "l",
                    Price = 28.50m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Goats are raised on natural pastures and milked daily by hand.",
                    OrganicCertified = false,
                    ImageUrl = "/images/products/goatmilk.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    FarmerId = farmers[2].FarmerId,
                    ProductName = "Corn for Livestock Feed",
                    CategoryId = grainsCategory.CategoryId,
                    Description = "High-quality corn grown specifically for livestock feed.",
                    ProductionDate = DateTime.Now.AddDays(-45),
                    Quantity = 5000.0m,
                    UnitOfMeasure = "kg",
                    Price = 4.75m,
                    CurrencyCode = "ZAR",
                    SustainabilityFeatures = "Grown using minimal tillage practices to preserve soil health.",
                    OrganicCertified = false,
                    ImageUrl = "/images/products/corn.jpg",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEnergySolutionCategoriesAsync(AgriEnergyConnectContext context)
        {
            if (await context.EnergySolutionCategories.AnyAsync())
                return;

            var categories = new List<EnergySolutionCategory>
            {
                new EnergySolutionCategory
                {
                    CategoryName = "Solar",
                    Description = "Solar power solutions including PV panels and thermal systems",
                    IsActive = true
                },
                new EnergySolutionCategory
                {
                    CategoryName = "Wind",
                    Description = "Wind turbines and wind power systems",
                    IsActive = true
                },
                new EnergySolutionCategory
                {
                    CategoryName = "Biogas",
                    Description = "Biogas digesters and energy systems",
                    IsActive = true
                },
                new EnergySolutionCategory
                {
                    CategoryName = "Hydro",
                    Description = "Small-scale hydroelectric systems",
                    IsActive = true
                },
                new EnergySolutionCategory
                {
                    CategoryName = "Hybrid",
                    Description = "Combined renewable energy systems",
                    IsActive = true
                },
                new EnergySolutionCategory
                {
                    CategoryName = "Energy Efficiency",
                    Description = "Solutions to reduce energy consumption",
                    IsActive = true
                }
            };

            await context.EnergySolutionCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEnergySolutionProvidersAsync(AgriEnergyConnectContext context)
        {
            if (await context.EnergySolutionProviders.AnyAsync())
                return;

            var providers = new List<EnergySolutionProvider>
            {
                new EnergySolutionProvider
                {
                    CompanyName = "SolarTech Solutions",
                    ContactPerson = "John Smith",
                    Email = "info@solartech.co.za",
                    PhoneNumber = "+27111234567",
                    Website = "https://www.solartech.co.za",
                    Address = "123 Solar Street",
                    City = "Johannesburg",
                    Province = "Gauteng",
                    PostalCode = "2000",
                    RegistrationNumber = "2010/123456/07",
                    YearEstablished = 2010,
                    Description = "SolarTech Solutions specializes in solar energy solutions for agricultural applications. We provide customized solar systems for irrigation, lighting, and farm operations.",
                    LogoUrl = "/images/providers/solartech.png",
                    IsVerified = true,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new EnergySolutionProvider
                {
                    CompanyName = "WindPower Africa",
                    ContactPerson = "Sarah Johnson",
                    Email = "info@windpowerafrica.co.za",
                    PhoneNumber = "+27211234567",
                    Website = "https://www.windpowerafrica.co.za",
                    Address = "456 Breeze Avenue",
                    City = "Cape Town",
                    Province = "Western Cape",
                    PostalCode = "8000",
                    RegistrationNumber = "2012/654321/07",
                    YearEstablished = 2012,
                    Description = "WindPower Africa provides wind energy solutions for farms and rural communities. Our wind turbines are designed to work efficiently in South African conditions.",
                    LogoUrl = "/images/providers/windpower.png",
                    IsVerified = true,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new EnergySolutionProvider
                {
                    CompanyName = "BioPower Systems",
                    ContactPerson = "Michael Ndlovu",
                    Email = "info@biopower.co.za",
                    PhoneNumber = "+27311234567",
                    Website = "https://www.biopower.co.za",
                    Address = "789 Green Road",
                    City = "Durban",
                    Province = "KwaZulu-Natal",
                    PostalCode = "4000",
                    RegistrationNumber = "2015/789012/07",
                    YearEstablished = 2015,
                    Description = "BioPower Systems specializes in biogas digesters and energy systems for agricultural waste. We convert farm waste into valuable energy for cooking, heating, and electricity generation.",
                    LogoUrl = "/images/providers/biopower.png",
                    IsVerified = true,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            };

            await context.EnergySolutionProviders.AddRangeAsync(providers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEnergySolutionsAsync(AgriEnergyConnectContext context)
        {
            if (await context.EnergySolutions.AnyAsync())
                return;

            // Get providers and categories
            var providers = await context.EnergySolutionProviders.ToListAsync();
            var categories = await context.EnergySolutionCategories.ToListAsync();

            if (providers.Count == 0 || categories.Count == 0)
                return;

            var solarCategory = categories.FirstOrDefault(c => c.CategoryName == "Solar");
            var windCategory = categories.FirstOrDefault(c => c.CategoryName == "Wind");
            var biogasCategory = categories.FirstOrDefault(c => c.CategoryName == "Biogas");
            var hydroCategory = categories.FirstOrDefault(c => c.CategoryName == "Hydro");
            var hybridCategory = categories.FirstOrDefault(c => c.CategoryName == "Hybrid");
            var efficiencyCategory = categories.FirstOrDefault(c => c.CategoryName == "Energy Efficiency");

            var solarProvider = providers.FirstOrDefault(p => p.CompanyName == "SolarTech Solutions");
            var windProvider = providers.FirstOrDefault(p => p.CompanyName == "WindPower Africa");
            var biogasProvider = providers.FirstOrDefault(p => p.CompanyName == "BioPower Systems");

            var solutions = new List<EnergySolution>
            {
                // Solutions for SolarTech
                new EnergySolution
                {
                    ProviderId = solarProvider.ProviderId,
                    CategoryId = solarCategory.CategoryId,
                    SolutionName = "Agricultural Solar PV System",
                    Description = "A complete solar photovoltaic system designed for farm operations. Ideal for powering irrigation pumps, lighting, and equipment.",
                    Specifications = "Available in 5kW, 10kW, and 25kW configurations. Includes PV panels, inverter, mounting hardware, and monitoring system.",
                    InstallationRequirements = "Requires unshaded roof or ground space. Professional installation included.",
                    MaintenanceInfo = "Annual maintenance check recommended. Panels should be cleaned every 3-6 months depending on local dust conditions.",
                    CostEstimate = "From R60,000 for basic system",
                    PriceRangeMin = 60000m,
                    PriceRangeMax = 250000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "4-6 years depending on energy usage",
                    ApplicationAreas = "Irrigation, barn lighting, equipment power, cold storage",
                    ImageUrl = "/images/solutions/solar-pv.jpg",
                    BrochureUrl = "/documents/solar-pv-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new EnergySolution
                {
                    ProviderId = solarProvider.ProviderId,
                    CategoryId = solarCategory.CategoryId,
                    SolutionName = "Solar Water Pumping System",
                    Description = "Specialized solar-powered water pumping system for agricultural irrigation. Eliminates fuel costs and ensures reliable irrigation even in remote areas.",
                    Specifications = "DC and AC pump options available. Flow rates from 5,000 to 50,000 liters per day. Submersible and surface pump options.",
                    InstallationRequirements = "Requires good solar access and proximity to water source. Professional installation included.",
                    MaintenanceInfo = "Minimal maintenance required. Annual system check recommended.",
                    CostEstimate = "From R40,000 depending on pump size and depth",
                    PriceRangeMin = 40000m,
                    PriceRangeMax = 120000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "3-5 years compared to diesel pumping",
                    ApplicationAreas = "Crop irrigation, livestock watering, aquaculture",
                    ImageUrl = "/images/solutions/solar-pump.jpg",
                    BrochureUrl = "/documents/solar-pump-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },

                // Solutions for WindPower
                new EnergySolution
                {
                    ProviderId = windProvider.ProviderId,
                    CategoryId = windCategory.CategoryId,
                    SolutionName = "Agricultural Wind Turbine",
                    Description = "Small to medium-sized wind turbines designed specifically for farm applications. Harnesses wind energy to generate electricity for farm operations.",
                    Specifications = "Available in 3kW, 5kW, and 10kW models. Hub heights from 15m to 30m. Cut-in wind speed of 3m/s.",
                    InstallationRequirements = "Requires site with average annual wind speed of at least 4.5 m/s. Professional installation and connection to farm electrical system included.",
                    MaintenanceInfo = "Annual inspection and maintenance required. 20-year design life with proper maintenance.",
                    CostEstimate = "From R80,000 to R350,000 depending on size",
                    PriceRangeMin = 80000m,
                    PriceRangeMax = 350000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "6-10 years depending on wind resource and energy usage",
                    ApplicationAreas = "General farm electricity, irrigation pumping, processing facilities",
                    ImageUrl = "/images/solutions/wind-turbine.jpg",
                    BrochureUrl = "/documents/wind-turbine-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new EnergySolution
                {
                    ProviderId = windProvider.ProviderId,
                    CategoryId = hybridCategory.CategoryId,
                    SolutionName = "Wind-Solar Hybrid System",
                    Description = "Integrated system combining wind turbines and solar panels to provide reliable energy throughout the year, compensating for seasonal variations in wind and solar resources.",
                    Specifications = "Customizable configurations with 3-10kW wind capacity and 5-15kW solar capacity. Includes battery storage options.",
                    InstallationRequirements = "Requires assessment of both wind and solar resources. Professional installation and system integration included.",
                    MaintenanceInfo = "Scheduled maintenance for wind components annually. Solar components require cleaning every 3-6 months.",
                    CostEstimate = "From R150,000 for basic system",
                    PriceRangeMin = 150000m,
                    PriceRangeMax = 500000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "5-8 years for typical farm applications",
                    ApplicationAreas = "Full farm electrification, off-grid operations, energy security",
                    ImageUrl = "/images/solutions/hybrid-system.jpg",
                    BrochureUrl = "/documents/hybrid-system-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },

                // Solutions for BioPower
                new EnergySolution
                {
                    ProviderId = biogasProvider.ProviderId,
                    CategoryId = biogasCategory.CategoryId,
                    SolutionName = "Agricultural Biogas Digester",
                    Description = "Biogas digester system designed to convert farm waste into usable energy. Processes animal manure and crop residues to produce biogas for cooking, heating, and electricity generation.",
                    Specifications = "Available in sizes from 6m³ to 50m³. Daily biogas production from 2m³ to 20m³ depending on model and feedstock.",
                    InstallationRequirements = "Requires level ground and proximity to waste sources. Professional installation included.",
                    MaintenanceInfo = "Regular feeding and monitoring required. Annual maintenance check recommended.",
                    CostEstimate = "From R30,000 for small system to R200,000 for large systems",
                    PriceRangeMin = 30000m,
                    PriceRangeMax = 200000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "3-7 years depending on current energy costs and waste volume",
                    ApplicationAreas = "Dairy farms, piggeries, poultry farms, mixed farming operations",
                    ImageUrl = "/images/solutions/biogas-digester.jpg",
                    BrochureUrl = "/documents/biogas-digester-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                },
                new EnergySolution
                {
                    ProviderId = biogasProvider.ProviderId,
                    CategoryId = biogasCategory.CategoryId,
                    SolutionName = "Biogas Generator Set",
                    Description = "Electricity generator specifically designed to run on biogas produced from farm waste. Converts biogas into electrical power for farm operations.",
                    Specifications = "Available in 5kW, 10kW, and 20kW models. Runs on biogas with methane content of 55-70%.",
                    InstallationRequirements = "Requires connection to biogas digester and farm electrical system. Professional installation included.",
                    MaintenanceInfo = "Regular oil changes and filter cleaning required. Scheduled service every 1000 operating hours.",
                    CostEstimate = "From R45,000 depending on size",
                    PriceRangeMin = 45000m,
                    PriceRangeMax = 150000m,
                    CurrencyCode = "ZAR",
                    ROIEstimate = "4-6 years when used with adequate biogas supply",
                    ApplicationAreas = "Electricity generation for farms with livestock operations",
                    ImageUrl = "/images/solutions/biogas-generator.jpg",
                    BrochureUrl = "/documents/biogas-generator-brochure.pdf",
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                }
            };

            await context.EnergySolutions.AddRangeAsync(solutions);
            await context.SaveChangesAsync();
        }
    }
}

/*References
dotnet-bot. 2025. KeyedHashAlgorithm Class (System.Security.Cryptography). [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.keyedhashalgorithm?view=net-9.0 [Accessed 13 May 2025].

‌
*/
