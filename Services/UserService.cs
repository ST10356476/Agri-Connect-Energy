// Services/UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Services;

namespace Agri_Energy_Connect.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
        Task<User> CreateUserAsync(User user, string password);
        Task<User> UpdateUserAsync(User user);
        Task<bool> UpdateUserPasswordAsync(int userId, string newPassword);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ActivateUserAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly AgriEnergyConnectContext _context;
        private readonly IAuthService _authService;

        public UserService(AgriEnergyConnectContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == roleName)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return null;

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return null;

            // Generate salt and hash password
            string salt;
            user.PasswordHash = _authService.HashPassword(password, out salt);
            user.PasswordSalt = salt;
            user.CreatedDate = DateTime.Now;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
                return null;

            // Check if new username conflicts with existing username
            if (existingUser.Username != user.Username &&
                await _context.Users.AnyAsync(u => u.Username == user.Username && u.UserId != user.UserId))
                return null;

            // Check if new email conflicts with existing email
            if (existingUser.Email != user.Email &&
                await _context.Users.AnyAsync(u => u.Email == user.Email && u.UserId != user.UserId))
                return null;

            // Update properties
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.RoleId = user.RoleId;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Generate salt and hash password
            string salt;
            user.PasswordHash = _authService.HashPassword(newPassword, out salt);
            user.PasswordSalt = salt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}