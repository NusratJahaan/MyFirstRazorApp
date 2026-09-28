using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IJudgementService
    {
        Task<List<Judgement>> GetJudgementsByComplaintIdAsync(int complaintId);
        Task<List<Judgement>> GetJudgementsByOffenceIdAsync(int offenceId);
        Task<Judgement?> GetJudgementByIdAsync(int id);
        Task<bool> AddJudgementAsync(Judgement judgement);
        Task<bool> UpdateJudgementAsync(Judgement judgement);
        Task<bool> DeleteJudgementAsync(int id);
        Task<bool> FinalizeJudgementAsync(int id, string userName);
    }
}