using Agri_Energy_Connect.Data;
using Agri_Energy_Connect.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Agri_Energy_Connect.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<(bool success, string message)> RegisterUserAsync(User user, string password, string role);
        string GenerateJwtToken(User user);
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }

    public class AuthService : IAuthService
    {
        private readonly AgriEnergyConnectContext _context;

        public AuthService(AgriEnergyConnectContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Update last login date
            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<(bool success, string message)> RegisterUserAsync(User user, string password, string roleName)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required");

            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                return (false, "Username is already taken");

            if (await _context.Users.AnyAsync(x => x.Email == user.Email))
                return (false, "Email is already registered");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
                return (false, "Role not found");

            user.RoleId = role.RoleId;
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = HashPassword(password, user.PasswordSalt);
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return (true, "User registered successfully");
        }

        public string GenerateJwtToken(User user)
        {
            // This is a placeholder for demonstration purposes
            return "token-placeholder";
        }

        public string HashPassword(string password, out string salt)
        {
            salt = GenerateSalt();
            return HashPassword(password, salt);
        }

        public string HashPassword(string password, string salt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var computedHash = HashPassword(password, storedSalt);
            return computedHash == storedHash;
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}
