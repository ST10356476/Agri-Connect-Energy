// ViewModels/EnergySolutionsViewModel.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri_Energy_Connect.Models;

namespace Agri_Energy_Connect.ViewModels
{


    /// <summary>
    /// View model for displaying energy solutions on the energy solutions page
    /// </summary>
    public class EnergySolutionsViewModel
    {
        /// <summary>
        /// List of all available energy solutions
        /// </summary>
        public IEnumerable<EnergySolution> AllSolutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Featured energy solutions to highlight on the page
        /// </summary>
        public IEnumerable<EnergySolution> FeaturedSolutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Energy solutions grouped by category
        /// </summary>
        public Dictionary<string, IEnumerable<EnergySolution>> SolutionsByCategory { get; set; } = new Dictionary<string, IEnumerable<EnergySolution>>();

        /// <summary>
        /// List of energy solution categories for filtering
        /// </summary>
        public IEnumerable<EnergySolutionCategory> Categories { get; set; } = new List<EnergySolutionCategory>();

        /// <summary>
        /// List of energy solution providers
        /// </summary>
        public IEnumerable<EnergySolutionProvider> Providers { get; set; } = new List<EnergySolutionProvider>();

        /// <summary>
        /// Filter criteria for searching solutions
        /// </summary>
        public EnergySolutionFilterModel Filter { get; set; } = new EnergySolutionFilterModel();

        /// <summary>
        /// Total count of all energy solutions
        /// </summary>
        public int TotalSolutions { get; set; }

        /// <summary>
        /// Statistics about solutions by category
        /// </summary>
        public Dictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>();
    }

    /// <summary>
    /// Model for filtering energy solutions
    /// </summary>
    public class EnergySolutionFilterModel
    {
        /// <summary>
        /// Filter by category
        /// </summary>
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Filter by provider
        /// </summary>
        [Display(Name = "Provider")]
        public int? ProviderId { get; set; }

        /// <summary>
        /// Minimum price range filter
        /// </summary>
        [Display(Name = "Minimum Price")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Maximum price range filter
        /// </summary>
        [Display(Name = "Maximum Price")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Search text for solution name or description
        /// </summary>
        [Display(Name = "Search")]
        public string SearchText { get; set; }

        /// <summary>
        /// Filter by specific application areas
        /// </summary>
        [Display(Name = "Application")]
        public string ApplicationArea { get; set; }

        /// <summary>
        /// Filter by availability
        /// </summary>
        [Display(Name = "Available only")]
        public bool AvailableOnly { get; set; } = true;

        /// <summary>
        /// Sort order for results
        /// </summary>
        [Display(Name = "Sort by")]
        public SolutionSortOrder SortBy { get; set; } = SolutionSortOrder.Name;
    }

    /// <summary>
    /// Enum for sorting energy solutions
    /// </summary>
    public enum SolutionSortOrder
    {
        [Display(Name = "Name")]
        Name,
        [Display(Name = "Price (Low to High)")]
        PriceLowToHigh,
        [Display(Name = "Price (High to Low)")]
        PriceHighToLow,
        [Display(Name = "Category")]
        Category,
        [Display(Name = "Provider")]
        Provider,
        [Display(Name = "Newest")]
        Newest
    }

    /// <summary>
    /// View model for energy solution details
    /// </summary>
    public class EnergySolutionDetailViewModel
    {
        /// <summary>
        /// The energy solution details
        /// </summary>
        public EnergySolution Solution { get; set; }

        /// <summary>
        /// Related solutions from the same category
        /// </summary>
        public IEnumerable<EnergySolution> RelatedSolutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Information about the provider
        /// </summary>
        public EnergySolutionProvider Provider { get; set; }

        /// <summary>
        /// User-submitted reviews (for future feature)
        /// </summary>
        public IEnumerable<SolutionReview> Reviews { get; set; } = new List<SolutionReview>();

        /// <summary>
        /// Average rating (for future feature)
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Quote request form
        /// </summary>
        public QuoteRequestModel QuoteRequest { get; set; } = new QuoteRequestModel();
    }

    /// <summary>
    /// Model for requesting a quote
    /// </summary>
    public class QuoteRequestModel
    {
        /// <summary>
        /// Energy solution being quoted
        /// </summary>
        public int SolutionId { get; set; }

        /// <summary>
        /// Requester's name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        /// <summary>
        /// Requester's email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Requester's phone number
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Farm size for solution sizing
        /// </summary>
        [Display(Name = "Farm Size (hectares)")]
        public decimal? FarmSize { get; set; }

        /// <summary>
        /// Current energy consumption
        /// </summary>
        [Display(Name = "Current Energy Usage (kWh/month)")]
        public decimal? CurrentEnergyUsage { get; set; }

        /// <summary>
        /// Additional requirements or questions
        /// </summary>
        [Display(Name = "Additional Requirements")]
        public string AdditionalRequirements { get; set; }

        /// <summary>
        /// Preferred contact method
        /// </summary>
        [Display(Name = "Preferred Contact Method")]
        public ContactMethod PreferredContact { get; set; } = ContactMethod.Email;

        /// <summary>
        /// When to contact
        /// </summary>
        [Display(Name = "Best Time to Contact")]
        public string ContactTime { get; set; }
    }

    /// <summary>
    /// Enum for contact preferences
    /// </summary>
    public enum ContactMethod
    {
        [Display(Name = "Email")]
        Email,
        [Display(Name = "Phone")]
        Phone,
        [Display(Name = "SMS")]
        SMS,
        [Display(Name = "In Person")]
        InPerson
    }

    /// <summary>
    /// Model for solution reviews (future feature)
    /// </summary>
    public class SolutionReview
    {
        public int ReviewId { get; set; }
        public int SolutionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsVerified { get; set; }
    }

    /// <summary>
    /// View model for comparing energy solutions
    /// </summary>
    public class CompareSolutionsViewModel
    {
        /// <summary>
        /// Maximum number of solutions that can be compared
        /// </summary>
        public const int MaxSolutionsToCompare = 3;

        /// <summary>
        /// Solutions selected for comparison
        /// </summary>
        public IEnumerable<EnergySolution> SelectedSolutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Available solutions to add to comparison
        /// </summary>
        public IEnumerable<EnergySolution> AvailableSolutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Features to compare across solutions
        /// </summary>
        public List<string> ComparisonFeatures { get; set; } = new List<string>
        {
            "Price Range",
            "Installation Requirements",
            "Maintenance",
            "ROI Estimate",
            "Specifications",
            "Application Areas"
        };
    }
}