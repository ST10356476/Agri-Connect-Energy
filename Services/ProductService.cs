using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByFarmerIdAsync(int farmerId);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly AgriEnergyConnectContext _context;

        public ProductService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerIdAsync(int farmerId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.FarmerId == farmerId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate)
                .OrderByDescending(p => p.ProductionDate)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            product.CreatedDate = DateTime.Now;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null)
                return null;

            // Update properties
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            existingProduct.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
