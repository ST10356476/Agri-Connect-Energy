using Agri_Energy_Connect.Models;

namespace Agri_Energy_Connect.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public Farmer Farmer { get; set; }
        public IEnumerable<Product> RecentProducts { get; set; }
    }
}
