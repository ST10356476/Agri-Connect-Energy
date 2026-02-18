using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class EnergySolutionProvider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Province { get; set; }

        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Year Established")]
        public int? YearEstablished { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(500)]
        [Display(Name = "Logo")]
        public string LogoUrl { get; set; }

        [Display(Name = "Verified")]
        public bool IsVerified { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<EnergySolution> Solutions { get; set; }
    }
}
