using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [StringLength(50)]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }

        [Display(Name = "Supervisor")]
        public int? SupervisorId { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual Employee Supervisor { get; set; }

        [StringLength(100)]
        [Display(Name = "Office Location")]
        public string OfficeLocation { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Employee> Subordinates { get; set; }
    }
}
