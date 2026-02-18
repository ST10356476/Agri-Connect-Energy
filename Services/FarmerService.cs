using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Services
{
    public interface IFarmerService
    {
        Task<IEnumerable<Farmer>> GetAllFarmersAsync();
        Task<Farmer> GetFarmerByIdAsync(int id);
        Task<Farmer> GetFarmerByUserIdAsync(int userId);
        Task<Farmer> AddFarmerAsync(Farmer farmer);
        Task<Farmer> UpdateFarmerAsync(Farmer farmer);
        Task<bool> DeleteFarmerAsync(int id);
    }

    public class FarmerService : IFarmerService
    {
        private readonly AgriEnergyConnectContext _context;

        public FarmerService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            return await _context.Farmers
                .Include(f => f.User)
                .Where(f => f.FarmName != null && f.User != null) // avoid nulls
                .OrderBy(f => f.FarmName)
                .ToListAsync();
        }


        public async Task<Farmer> GetFarmerByIdAsync(int id)
        {
            return await _context.Farmers
                .Include(f => f.User)
                .Include(f => f.Products)
                    .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(f => f.FarmerId == id);
        }

        public async Task<Farmer> GetFarmerByUserIdAsync(int userId)
        {
            return await _context.Farmers
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<Farmer> AddFarmerAsync(Farmer farmer)
        {
            farmer.CreatedDate = DateTime.Now;
            await _context.Farmers.AddAsync(farmer);
            await _context.SaveChangesAsync();
            return farmer;
        }

        public async Task<Farmer> UpdateFarmerAsync(Farmer farmer)
        {
            var existingFarmer = await _context.Farmers.FindAsync(farmer.FarmerId);
            if (existingFarmer == null)
                return null;

            // Update properties
            _context.Entry(existingFarmer).CurrentValues.SetValues(farmer);
            existingFarmer.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingFarmer;
        }

        public async Task<bool> DeleteFarmerAsync(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
                return false;

            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
