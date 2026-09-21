using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class ComplaintService : IComplaintService
    {
        private readonly AppDbContext _context;

        public ComplaintService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            try
            {
                return await _context.Complaints
                    .OrderByDescending(c => c.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting complaints: {ex.Message}");
            }
        }

        public async Task<Complaint?> GetComplaintByIdAsync(int id)
        {
            try
            {
                return await _context.Complaints.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting complaint: {ex.Message}");
            }
        }

        public async Task<List<Complaint>> SearchComplaintsAsync(string? searchTerm)
        {
            try
            {
                var query = _context.Complaints.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(c =>
                        c.ComplaintNumber.Contains(searchTerm) ||
                        c.DefendantName.Contains(searchTerm) ||
                        c.DefendantPhone.Contains(searchTerm));
                }

                return await query.OrderByDescending(c => c.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching complaints: {ex.Message}");
            }
        }

        public async Task<bool> AddComplaintAsync(Complaint complaint)
        {
            try
            {
                if (string.IsNullOrEmpty(complaint.ComplaintNumber))
                {
                    complaint.ComplaintNumber = await GenerateComplaintNumberAsync();
                }

                await _context.Complaints.AddAsync(complaint);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding complaint: {ex.Message}");
            }
        }

        public async Task<bool> UpdateComplaintAsync(Complaint complaint)
        {
            try
            {
                var existing = await _context.Complaints.FindAsync(complaint.Id);
                if (existing == null) return false;

                existing.DefendantName = complaint.DefendantName;
                existing.DefendantPhone = complaint.DefendantPhone;
                existing.DefendantAddress = complaint.DefendantAddress;
                existing.Description = complaint.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating complaint: {ex.Message}");
            }
        }

        public async Task<bool> DeleteComplaintAsync(int id)
        {
            try
            {
                var complaint = await _context.Complaints.FindAsync(id);
                if (complaint == null) return false;

                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting complaint: {ex.Message}");
            }
        }

        public async Task<string> GenerateComplaintNumberAsync()
        {
            try
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var prefix = $"CR-{year}-{month:D2}-";

                var lastNumber = await _context.Complaints
                    .Where(c => c.ComplaintNumber.StartsWith(prefix))
                    .OrderByDescending(c => c.ComplaintNumber)
                    .Select(c => c.ComplaintNumber)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (!string.IsNullOrEmpty(lastNumber))
                {
                    var lastPart = lastNumber.Substring(prefix.Length);
                    if (int.TryParse(lastPart, out int lastNum))
                    {
                        nextNumber = lastNum + 1;
                    }
                }

                return $"{prefix}{nextNumber:D4}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating complaint number: {ex.Message}");
            }
        }
    }
}