using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri_Energy_Connect.Models
{
    public class EnergySolution
    {
        [Key]
        public int SolutionId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [ForeignKey("ProviderId")]
        public virtual EnergySolutionProvider Provider { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual EnergySolutionCategory Category { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Solution Name")]
        public string SolutionName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        [Display(Name = "Installation Requirements")]
        public string InstallationRequirements { get; set; }

        [Display(Name = "Maintenance Information")]
        public string MaintenanceInfo { get; set; }

        [StringLength(100)]
        [Display(Name = "Cost Estimate")]
        public string CostEstimate { get; set; }

        [Display(Name = "Price Range Minimum")]
        public decimal? PriceRangeMin { get; set; }

        [Display(Name = "Price Range Maximum")]
        public decimal? PriceRangeMax { get; set; }

        [StringLength(3)]
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; } = "ZAR";

        [StringLength(100)]
        [Display(Name = "ROI Estimate")]
        public string ROIEstimate { get; set; }

        [Display(Name = "Application Areas")]
        public string ApplicationAreas { get; set; }

        [StringLength(500)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        [Display(Name = "Brochure")]
        public string BrochureUrl { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedDate { get; set; }
    }
}
