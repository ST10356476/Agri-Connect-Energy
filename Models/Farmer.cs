using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Agri_Energy_Connect.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Farm Name")]
        public string FarmName { get; set; }

        [StringLength(50)]
        [Display(Name = "Registration Number")]
        public string? RegistrationNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Established Date")]
        public DateTime? EstablishedDate { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }


        [Display(Name = "Farm Size")]
        public decimal? FarmSize { get; set; }

        [StringLength(20)]
        [Display(Name = "Farm Size Unit")]
        public string? FarmSizeUnit { get; set; }

        [StringLength(100)]
        [Display(Name = "Farming Type")]
        public string? FarmingType { get; set; }

        [StringLength(200)]
        [Display(Name = "Main Crops")]
        public string? MainCrops { get; set; }

        [StringLength(200)]
        [Display(Name = "Main Livestock")]
        public string? MainLivestock { get; set; }

        [Display(Name = "Sustainability Practices")]
        public string? SustainabilityPractices { get; set; }

        [Display(Name = "Profile Description")]
        public string? ProfileDescription { get; set; }

        [StringLength(500)]
        [Display(Name = "Profile Image")]
        public string? ProfileImageUrl { get; set; }

        [Display(Name = "Energy Needs")]
        public string? EnergyNeeds { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedDate { get; set; }

        public bool IsVerified { get; set; } = false;

        // Navigation property
        public virtual ICollection<Product>? Products { get; set; }
    }
}
