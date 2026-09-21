using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class OffenceLookUpService : IOffenceLookUpService
    {
        private readonly AppDbContext _context;

        public OffenceLookUpService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OffenceLookUp>> GetAllAsync()
        {
            try
            {
                return await _context.OffenceLookUps.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting offence lookups: {ex.Message}");
            }
        }

        public async Task<OffenceLookUp?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.OffenceLookUps.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting offence lookup: {ex.Message}");
            }
        }
    }
}