using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<User> Users { get; set; }
    }
}
