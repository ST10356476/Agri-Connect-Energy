using Agri_Energy_Connect.Models;

namespace Agri_Energy_Connect.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public int FarmerCount { get; set; }
        public int ProductCount { get; set; }
        public IEnumerable<Farmer> RecentFarmers { get; set; }
        public IEnumerable<Product> RecentProducts { get; set; }
    }
}
