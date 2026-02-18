using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Services
{
    public interface IEnergySolutionService
    {
        Task<IEnumerable<EnergySolution>> GetAllSolutionsAsync();
        Task<IEnumerable<EnergySolution>> GetSolutionsByProviderIdAsync(int providerId);
        Task<IEnumerable<EnergySolution>> GetSolutionsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<EnergySolution>> GetFeaturedSolutionsAsync(int count);
        Task<EnergySolution> GetSolutionByIdAsync(int id);
        Task<EnergySolution> AddSolutionAsync(EnergySolution solution);
        Task<EnergySolution> UpdateSolutionAsync(EnergySolution solution);
        Task<bool> DeleteSolutionAsync(int id);

        Task<IEnumerable<EnergySolutionCategory>> GetAllCategoriesAsync();
        Task<EnergySolutionCategory> GetCategoryByIdAsync(int id);

        Task<IEnumerable<EnergySolutionProvider>> GetAllProvidersAsync();
        Task<EnergySolutionProvider> GetProviderByIdAsync(int id);
        Task<EnergySolutionProvider> AddProviderAsync(EnergySolutionProvider provider);
        Task<EnergySolutionProvider> UpdateProviderAsync(EnergySolutionProvider provider);
    }

    public class EnergySolutionService : IEnergySolutionService
    {
        private readonly AgriEnergyConnectContext _context;

        public EnergySolutionService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnergySolution>> GetAllSolutionsAsync()
        {
            return await _context.EnergySolutions
                .Include(s => s.Provider)
                .Include(s => s.Category)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EnergySolution>> GetSolutionsByProviderIdAsync(int providerId)
        {
            return await _context.EnergySolutions
                .Include(s => s.Category)
                .Where(s => s.ProviderId == providerId)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EnergySolution>> GetSolutionsByCategoryIdAsync(int categoryId)
        {
            return await _context.EnergySolutions
                .Include(s => s.Provider)
                .Where(s => s.CategoryId == categoryId)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EnergySolution>> GetFeaturedSolutionsAsync(int count)
        {
            return await _context.EnergySolutions
                .Include(s => s.Provider)
                .Include(s => s.Category)
                .Where(s => s.IsAvailable)
                .OrderByDescending(s => s.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<EnergySolution> GetSolutionByIdAsync(int id)
        {
            return await _context.EnergySolutions
                .Include(s => s.Provider)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SolutionId == id);
        }

        public async Task<EnergySolution> AddSolutionAsync(EnergySolution solution)
        {
            solution.CreatedDate = DateTime.Now;
            await _context.EnergySolutions.AddAsync(solution);
            await _context.SaveChangesAsync();
            return solution;
        }

        public async Task<EnergySolution> UpdateSolutionAsync(EnergySolution solution)
        {
            var existingSolution = await _context.EnergySolutions.FindAsync(solution.SolutionId);
            if (existingSolution == null)
                return null;

            // Update properties
            _context.Entry(existingSolution).CurrentValues.SetValues(solution);
            existingSolution.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingSolution;
        }

        public async Task<bool> DeleteSolutionAsync(int id)
        {
            var solution = await _context.EnergySolutions.FindAsync(id);
            if (solution == null)
                return false;

            _context.EnergySolutions.Remove(solution);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EnergySolutionCategory>> GetAllCategoriesAsync()
        {
            return await _context.EnergySolutionCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<EnergySolutionCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.EnergySolutionCategories
                .Include(c => c.Solutions)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<IEnumerable<EnergySolutionProvider>> GetAllProvidersAsync()
        {
            return await _context.EnergySolutionProviders
                .Where(p => p.IsActive)
                .OrderBy(p => p.CompanyName)
                .ToListAsync();
        }

        public async Task<EnergySolutionProvider> GetProviderByIdAsync(int id)
        {
            return await _context.EnergySolutionProviders
                .Include(p => p.Solutions)
                    .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(p => p.ProviderId == id);
        }

        public async Task<EnergySolutionProvider> AddProviderAsync(EnergySolutionProvider provider)
        {
            provider.CreatedDate = DateTime.Now;
            await _context.EnergySolutionProviders.AddAsync(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<EnergySolutionProvider> UpdateProviderAsync(EnergySolutionProvider provider)
        {
            var existingProvider = await _context.EnergySolutionProviders.FindAsync(provider.ProviderId);
            if (existingProvider == null)
                return null;

            // Update properties
            _context.Entry(existingProvider).CurrentValues.SetValues(provider);
            existingProvider.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingProvider;
        }
    }
}
