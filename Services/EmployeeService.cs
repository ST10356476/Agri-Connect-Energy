using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> GetEmployeeByUserIdAsync(int userId);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department);
        Task<bool> VerifyFarmerAsync(int farmerId);
        Task<IDictionary<string, int>> GetDashboardStatisticsAsync();
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly AgriEnergyConnectContext _context;

        public EmployeeService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.User)
                .OrderBy(e => e.User.LastName)
                .ThenBy(e => e.User.FirstName)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Supervisor)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetEmployeeByUserIdAsync(int userId)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            employee.HireDate = DateTime.Now;
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);
            if (existingEmployee == null)
                return null;

            // Update properties
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Where(e => e.Department == department)
                .OrderBy(e => e.User.LastName)
                .ThenBy(e => e.User.FirstName)
                .ToListAsync();
        }

        public async Task<bool> VerifyFarmerAsync(int farmerId)
        {
            var farmer = await _context.Farmers.FindAsync(farmerId);
            if (farmer == null)
                return false;

            farmer.IsVerified = true;
            farmer.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IDictionary<string, int>> GetDashboardStatisticsAsync()
        {
            var stats = new Dictionary<string, int>();

            // Get total farmers count
            stats["FarmerCount"] = await _context.Farmers.CountAsync();

            // Get total products count
            stats["ProductCount"] = await _context.Products.CountAsync();

            // Get today's registrations
            var today = DateTime.Today;
            stats["TodayRegistrations"] = await _context.Farmers
                .Where(f => f.CreatedDate.Date == today)
                .CountAsync();

            // Get today's products
            stats["TodayProducts"] = await _context.Products
                .Where(p => p.CreatedDate.Date == today)
                .CountAsync();

            // Get verified farmers count
            stats["VerifiedFarmers"] = await _context.Farmers
                .Where(f => f.IsVerified)
                .CountAsync();

            // Get pending verification count
            stats["PendingVerification"] = await _context.Farmers
                .Where(f => !f.IsVerified)
                .CountAsync();

            // Get energy solutions count
            stats["EnergySolutionsCount"] = await _context.EnergySolutions.CountAsync();

            // Get energy providers count
            stats["EnergyProvidersCount"] = await _context.EnergySolutionProviders.CountAsync();

            return stats;
        }
    }
}
