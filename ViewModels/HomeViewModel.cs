using Agri_Energy_Connect.Models;

namespace Agri_Energy_Connect.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IEnumerable<Farmer> FeaturedFarmers { get; set; } = new List<Farmer>();
        public IEnumerable<EnergySolution> FeaturedSolutions { get; set; } = new List<EnergySolution>();
    }
}
