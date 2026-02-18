using Agri_Energy_Connect.Models;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.ViewModels
{
    public class FilterProductsViewModel
    {
        [Display(Name = "Farmer")]
        public int? FarmerId { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
