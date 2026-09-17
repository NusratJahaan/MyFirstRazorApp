using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<SystemUser> _passwordHasher;

        public UserService(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<SystemUser>();
        }

        public async Task<bool> RegisterAsync(SystemUser user, string plainPassword)
        {
            try
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, plainPassword);
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = "System";
                user.UpdatedDate = DateTime.Now;
                user.UpdatedBy = "System";

                await _context.SystemUsers.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering user: {ex.Message}");
            }
        }

        public async Task<SystemUser?> ValidateUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.SystemUsers
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null) return null;

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                return result == PasswordVerificationResult.Success ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error validating user: {ex.Message}");
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                return await _context.SystemUsers.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking email: {ex.Message}");
            }
        }
    }
}