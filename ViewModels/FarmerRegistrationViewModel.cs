using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.ViewModels
{
    public class FarmerRegistrationViewModel
    {
        // User information
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, ErrorMessage = "Username must be between {2} and {1} characters", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        // Farmer information
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

        [Display(Name = "Profile Description")]
        public string ProfileDescription { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }
    }
}
