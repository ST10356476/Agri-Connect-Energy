using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.ViewModels
{
    public class FarmerViewModel
    {
        [Required(ErrorMessage = "Farm name is required")]
        [StringLength(150, ErrorMessage = "Farm name must be between {2} and {1} characters", MinimumLength = 2)]
        [Display(Name = "Farm Name")]
        public string FarmName { get; set; }

        [StringLength(50)]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Established Date")]
        public DateTime? EstablishedDate { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Province { get; set; }

        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Farm Size")]
        public decimal? FarmSize { get; set; }

        [StringLength(20)]
        [Display(Name = "Farm Size Unit")]
        public string FarmSizeUnit { get; set; }

        [StringLength(100)]
        [Display(Name = "Farming Type")]
        public string FarmingType { get; set; }

        [StringLength(200)]
        [Display(Name = "Main Crops")]
        public string MainCrops { get; set; }

        [StringLength(200)]
        [Display(Name = "Main Livestock")]
        public string MainLivestock { get; set; }

        [Display(Name = "Sustainability Practices")]
        public string SustainabilityPractices { get; set; }

        [Display(Name = "Profile Description")]
        public string ProfileDescription { get; set; }

        [Display(Name = "Energy Needs")]
        public string EnergyNeeds { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }
    }
}
