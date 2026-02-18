using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual ProductCategory ParentCategory { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductCategory> Subcategories { get; set; }
    }
}
