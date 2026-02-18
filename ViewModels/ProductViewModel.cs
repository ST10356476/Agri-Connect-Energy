using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(150, ErrorMessage = "Product name must be between {2} and {1} characters", MinimumLength = 2)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Production date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; } = DateTime.Today;

        [Display(Name = "Quantity")]
        public decimal? Quantity { get; set; }

        [StringLength(30)]
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Sustainability Features")]
        public string SustainabilityFeatures { get; set; }

        [Display(Name = "Organic Certified")]
        public bool OrganicCertified { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile ProductImage { get; set; }
    }
}
