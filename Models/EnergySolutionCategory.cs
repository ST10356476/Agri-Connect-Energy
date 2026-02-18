using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class EnergySolutionCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<EnergySolution> Solutions { get; set; }
    }
}
