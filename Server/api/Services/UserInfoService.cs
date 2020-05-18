using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace api.Services
{
    public interface IUserInfoService
    {
        string UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Role { get; set; }
    }

    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var currentContext = _httpContextAccessor.HttpContext;
            if (currentContext == null || !currentContext.User.Identity.IsAuthenticated) {
                UserId = "";
                FirstName = "";
                LastName = "";
                return;
            }

            UserId = currentContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;
            FirstName = currentContext.User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? string.Empty;
            LastName = currentContext.User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? string.Empty;
            Role = currentContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? string.Empty;
        }
    }
}