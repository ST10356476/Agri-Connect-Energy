// Controllers/EnergySolutionController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Services;
using Agri_Energy_Connect.ViewModels;

namespace Agri_Energy_Connect.Controllers
{
    public class EnergySolutionController : Controller
    {
        private readonly IEnergySolutionService _energySolutionService;
        private readonly IProductCategoryService _categoryService;

        public EnergySolutionController(
            IEnergySolutionService energySolutionService,
            IProductCategoryService categoryService)
        {
            _energySolutionService = energySolutionService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Display the main energy solutions page with filtering options
        /// </summary>
        public async Task<IActionResult> Index(EnergySolutionFilterModel filter, int page = 1, int pageSize = 12)
        {
            // Get all solutions
            var allSolutions = await _energySolutionService.GetAllSolutionsAsync();

            // Apply filters
            var filteredSolutions = ApplyFilters(allSolutions, filter);

            // Apply sorting
            filteredSolutions = ApplySorting(filteredSolutions, filter.SortBy);

            // Get categories for filter dropdown
            var categories = await _energySolutionService.GetAllCategoriesAsync();
            var providers = await _energySolutionService.GetAllProvidersAsync();

            // Group solutions by category
            var solutionsByCategory = allSolutions
                .GroupBy(s => s.Category.CategoryName)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            // Calculate category counts
            var categoryCounts = solutionsByCategory
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count());

            // Create view model
            var viewModel = new EnergySolutionsViewModel
            {
                AllSolutions = filteredSolutions,
                FeaturedSolutions = await GetFeaturedSolutions(),
                SolutionsByCategory = solutionsByCategory,
                Categories = categories,
                Providers = providers,
                Filter = filter,
                TotalSolutions = allSolutions.Count(),
                CategoryCounts = categoryCounts
            };

            return View(viewModel);
        }

        /// <summary>
        /// Display detailed information about a specific energy solution
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var solution = await _energySolutionService.GetSolutionByIdAsync(id);
            if (solution == null)
            {
                return NotFound();
            }

            // Get related solutions from the same category
            var relatedSolutions = await _energySolutionService.GetSolutionsByCategoryIdAsync(solution.CategoryId);
            relatedSolutions = relatedSolutions.Where(s => s.SolutionId != id).Take(3);

            var viewModel = new EnergySolutionDetailViewModel
            {
                Solution = solution,
                RelatedSolutions = relatedSolutions,
                Provider = solution.Provider,
                Reviews = new List<SolutionReview>(), // Placeholder for future feature
                AverageRating = 0, // Placeholder for future feature
                QuoteRequest = new QuoteRequestModel { SolutionId = id }
            };

            return View(viewModel);
        }

        /// <summary>
        /// Show quote request form
        /// </summary>
        public async Task<IActionResult> RequestQuote(int? solutionId)
        {
            var model = new QuoteRequestModel();

            if (solutionId.HasValue)
            {
                model.SolutionId = solutionId.Value;
            }

            return View(model);
        }

        /// <summary>
        /// Process quote request
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestQuote(QuoteRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                // If there's a specific solution, get its details for the form
                if (model.SolutionId > 0)
                {
                    var solution = await _energySolutionService.GetSolutionByIdAsync(model.SolutionId);
                    ViewBag.SolutionName = solution?.SolutionName;
                }
                return View(model);
            }

            // TODO: Process quote request
            // 1. Save to database
            // 2. Send notification to providers
            // 3. Send confirmation email to user

            // Generate confirmation number
            var confirmationNumber = GenerateConfirmationNumber();

            // Create confirmation view model
            var confirmationViewModel = new QuoteConfirmationViewModel
            {
                QuoteRequest = model,
                QuoteRequestId = 1, // Replace with actual ID after saving
                ConfirmationNumber = confirmationNumber,
                ExpectedContactTime = "within 24 hours",
                NextSteps = new List<string>
        {
            "Our team will review your request",
            "Providers will contact you with quotes",
            "Schedule on-site assessments",
            "Begin installation process"
        }
            };

            TempData["SuccessMessage"] = "Quote request submitted successfully!";
            return View("QuoteConfirmation", confirmationViewModel);
        }

        /// <summary>
        /// Generate a unique confirmation number
        /// </summary>
        private string GenerateConfirmationNumber()
        {
            var random = new Random();
            return $"QR{DateTime.Now:yyyyMMdd}{random.Next(1000, 9999)}";
        }

        /// <summary>
        /// Compare multiple energy solutions
        /// </summary>
        public async Task<IActionResult> Compare(int[] solutionIds)
        {
            if (solutionIds == null || solutionIds.Length == 0)
            {
                return RedirectToAction("Index");
            }

            if (solutionIds.Length > CompareSolutionsViewModel.MaxSolutionsToCompare)
            {
                TempData["ErrorMessage"] = $"You can only compare up to {CompareSolutionsViewModel.MaxSolutionsToCompare} solutions at a time.";
                return RedirectToAction("Index");
            }

            var selectedSolutions = new List<EnergySolution>();
            foreach (var id in solutionIds)
            {
                var solution = await _energySolutionService.GetSolutionByIdAsync(id);
                if (solution != null)
                {
                    selectedSolutions.Add(solution);
                }
            }

            // Get all solutions for the "add to comparison" dropdown
            var allSolutions = await _energySolutionService.GetAllSolutionsAsync();
            var availableSolutions = allSolutions.Where(s => !solutionIds.Contains(s.SolutionId));

            var viewModel = new CompareSolutionsViewModel
            {
                SelectedSolutions = selectedSolutions,
                AvailableSolutions = availableSolutions
            };

            return View(viewModel);
        }

        /// <summary>
        /// Display solutions by category
        /// </summary>
        public async Task<IActionResult> Category(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return RedirectToAction("Index");
            }

            var category = await _energySolutionService.GetAllCategoriesAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower()));

            if (category == null)
            {
                return NotFound();
            }

            var solutions = await _energySolutionService.GetSolutionsByCategoryIdAsync(category.CategoryId);

            ViewData["CategoryName"] = category.CategoryName;
            ViewData["CategoryDescription"] = category.Description;

            return View(solutions);
        }

        /// <summary>
        /// Provider portal (for future implementation)
        /// </summary>
        [Authorize(Roles = "EnergyProvider,Employee,Administrator")]
        public async Task<IActionResult> ProviderDashboard()
        {
            // TODO: Implement provider dashboard
            return View();
        }

        /// <summary>
        /// Add new energy solution (for providers)
        /// </summary>
        [Authorize(Roles = "EnergyProvider,Administrator")]
        public async Task<IActionResult> AddSolution()
        {
            var categories = await _energySolutionService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        /// <summary>
        /// Process new energy solution
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EnergyProvider,Administrator")]
        public async Task<IActionResult> AddSolution(EnergySolution model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _energySolutionService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
                return View(model);
            }

            // TODO: Set provider ID based on current user
            // model.ProviderId = GetCurrentProviderId();

            await _energySolutionService.AddSolutionAsync(model);

            TempData["SuccessMessage"] = "Energy solution added successfully.";
            return RedirectToAction("Details", new { id = model.SolutionId });
        }

        #region Private Methods

        /// <summary>
        /// Apply filters to the energy solutions collection
        /// </summary>
        private IEnumerable<EnergySolution> ApplyFilters(IEnumerable<EnergySolution> solutions, EnergySolutionFilterModel filter)
        {
            if (filter == null)
                return solutions;

            // Filter by category
            if (filter.CategoryId.HasValue)
            {
                solutions = solutions.Where(s => s.CategoryId == filter.CategoryId.Value);
            }

            // Filter by provider
            if (filter.ProviderId.HasValue)
            {
                solutions = solutions.Where(s => s.ProviderId == filter.ProviderId.Value);
            }

            // Filter by price range
            if (filter.MinPrice.HasValue)
            {
                solutions = solutions.Where(s => s.PriceRangeMin >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                solutions = solutions.Where(s => s.PriceRangeMax <= filter.MaxPrice.Value);
            }

            // Filter by search text
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var searchText = filter.SearchText.ToLower();
                solutions = solutions.Where(s =>
                    s.SolutionName.ToLower().Contains(searchText) ||
                    s.Description.ToLower().Contains(searchText) ||
                    s.ApplicationAreas.ToLower().Contains(searchText)
                );
            }

            // Filter by application area
            if (!string.IsNullOrEmpty(filter.ApplicationArea))
            {
                solutions = solutions.Where(s =>
                    s.ApplicationAreas.ToLower().Contains(filter.ApplicationArea.ToLower())
                );
            }

            // Filter by availability
            if (filter.AvailableOnly)
            {
                solutions = solutions.Where(s => s.IsAvailable);
            }

            return solutions;
        }

        /// <summary>
        /// Apply sorting to the energy solutions collection
        /// </summary>
        private IEnumerable<EnergySolution> ApplySorting(IEnumerable<EnergySolution> solutions, SolutionSortOrder sortBy)
        {
            return sortBy switch
            {
                SolutionSortOrder.Name => solutions.OrderBy(s => s.SolutionName),
                SolutionSortOrder.PriceLowToHigh => solutions.OrderBy(s => s.PriceRangeMin),
                SolutionSortOrder.PriceHighToLow => solutions.OrderByDescending(s => s.PriceRangeMax),
                SolutionSortOrder.Category => solutions.OrderBy(s => s.Category.CategoryName),
                SolutionSortOrder.Provider => solutions.OrderBy(s => s.Provider.CompanyName),
                SolutionSortOrder.Newest => solutions.OrderByDescending(s => s.CreatedDate),
                _ => solutions.OrderBy(s => s.SolutionName)
            };
        }

        /// <summary>
        /// Get featured energy solutions for the home page
        /// </summary>
        private async Task<IEnumerable<EnergySolution>> GetFeaturedSolutions()
        {
            var allSolutions = await _energySolutionService.GetAllSolutionsAsync();
            return allSolutions
                .Where(s => s.IsAvailable)
                .OrderByDescending(s => s.CreatedDate)
                .Take(6);
        }

        /// <summary>
        /// Request quotes for multiple solutions at once
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestMultipleQuotes(int[] solutionIds)
        {
            if (solutionIds == null || solutionIds.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select at least one solution to request quotes.";
                return RedirectToAction("Compare");
            }

            // Create a quote request with multiple solutions
            var viewModel = new MultipleQuoteRequestViewModel
            {
                SolutionIds = solutionIds.ToList(),
                Solutions = new List<EnergySolution>()
            };

            // Load solution details
            foreach (var id in solutionIds)
            {
                var solution = await _energySolutionService.GetSolutionByIdAsync(id);
                if (solution != null)
                {
                    viewModel.Solutions.Add(solution);
                }
            }

            return View("RequestMultipleQuotes", viewModel);
        }

        #endregion
    }
}