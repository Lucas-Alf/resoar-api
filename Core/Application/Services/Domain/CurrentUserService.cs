using System.Security.Claims;
using Application.Interfaces.Services.Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Domain
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Returns the current user id
        /// </summary>
        public int GetId()
        {
            return Convert.ToInt32(_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        /// <summary>
        /// Returns the current user name
        /// </summary>
        public string? GetName()
        {
            return _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }

        /// <summary>
        /// Returns the current user email
        /// </summary>
        public string? GetEmail()
        {
            return _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// Returns the value of a specific claimType
        /// </summary>
        /// <param name="claimType"></param>
        public string? GetClaim(string claimType)
        {
            return _httpContextAccessor?.HttpContext?.User.FindFirstValue(claimType);
        }
    }
}