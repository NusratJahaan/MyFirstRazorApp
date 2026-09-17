using Microsoft.AspNetCore.Http;
using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface IAuthService
    {
        Task<List<Claim>> SetupAuthClaims(SystemUser user, HttpContext context);
    }
}