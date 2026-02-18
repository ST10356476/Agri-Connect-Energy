using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public virtual Farmer Farmer { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory Category { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        [Display(Name = "Quantity")]
        public decimal? Quantity { get; set; }

        [StringLength(30)]
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [StringLength(3)]
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; } = "ZAR";

        [Display(Name = "Sustainability Features")]
        public string SustainabilityFeatures { get; set; }

        [Display(Name = "Organic Certified")]
        public bool OrganicCertified { get; set; } = false;

        [StringLength(500)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedDate { get; set; }
    }
}
