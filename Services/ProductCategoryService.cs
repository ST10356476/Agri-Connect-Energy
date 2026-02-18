using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Services
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<ProductCategory>> GetActiveCategoriesAsync();
        Task<ProductCategory> GetCategoryByIdAsync(int id);
        Task<ProductCategory> GetCategoryByNameAsync(string name);
        Task<ProductCategory> AddCategoryAsync(ProductCategory category);
        Task<ProductCategory> UpdateCategoryAsync(ProductCategory category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> DeactivateCategoryAsync(int id);
        Task<bool> ActivateCategoryAsync(int id);
        Task<IEnumerable<ProductCategory>> GetCategoriesWithProductsAsync();
        Task<int> GetProductCountByCategoryAsync(int categoryId);
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly AgriEnergyConnectContext _context;

        public ProductCategoryService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            return await _context.ProductCategories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetActiveCategoriesAsync()
        {
            return await _context.ProductCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<ProductCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.ProductCategories
                .Include(c => c.ParentCategory)
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<ProductCategory> GetCategoryByNameAsync(string name)
        {
            return await _context.ProductCategories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }

        public async Task<ProductCategory> AddCategoryAsync(ProductCategory category)
        {
            // Check if category with the same name already exists
            var existing = await GetCategoryByNameAsync(category.CategoryName);
            if (existing != null)
                return null;

            await _context.ProductCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ProductCategory> UpdateCategoryAsync(ProductCategory category)
        {
            var existingCategory = await _context.ProductCategories.FindAsync(category.CategoryId);
            if (existingCategory == null)
                return null;

            // Check if we're trying to update to a name that already exists (excluding this category)
            var nameExists = await _context.ProductCategories
                .AnyAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower() &&
                          c.CategoryId != category.CategoryId);
            if (nameExists)
                return null;

            // Update properties
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            existingCategory.ParentCategoryId = category.ParentCategoryId;
            existingCategory.IsActive = category.IsActive;

            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
                return false;

            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return false;

            // Check if category has subcategories
            var hasSubcategories = await _context.ProductCategories.AnyAsync(c => c.ParentCategoryId == id);
            if (hasSubcategories)
                return false;

            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateCategoryAsync(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
                return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateCategoryAsync(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
                return false;

            category.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategoriesWithProductsAsync()
        {
            return await _context.ProductCategories
                .Include(c => c.Products)
                .Where(c => c.Products.Any())
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<int> GetProductCountByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .CountAsync(p => p.CategoryId == categoryId);
        }
    }
}
