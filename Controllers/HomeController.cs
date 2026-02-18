using System.Diagnostics;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IFarmerService _farmerService;
        private readonly IEnergySolutionService _energySolutionService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            IFarmerService farmerService,
            IEnergySolutionService energySolutionService)
        {
            _logger = logger;
            _productService = productService;
            _farmerService = farmerService;
            _energySolutionService = energySolutionService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _productService.GetProductByIdAsync(6);
            var featuredFarmers = await _farmerService.GetFarmerByIdAsync(3);
            var featuredSolutions = await _energySolutionService.GetFeaturedSolutionsAsync(3);

            var viewModel = new HomeViewModel
            {
                // If featuredProducts is a single Product
                FeaturedProducts = featuredProducts is Product singleProduct
                    ? new List<Product> { singleProduct }
                    : (IEnumerable<Product>)featuredProducts,

                // If featuredFarmers is a single Farmer
                FeaturedFarmers = featuredFarmers is Farmer singleFarmer
                    ? new List<Farmer> { singleFarmer }
                    : (IEnumerable<Farmer>)featuredFarmers,

                FeaturedSolutions = featuredSolutions
            };

            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
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

/*References
Dot Net Tutorials.2019. Singleton Pattern in C# with Example. [online] Available at: https://dotnettutorials.net/lesson/singleton-design-pattern/. [Accessed 12 May 2025]
*/
