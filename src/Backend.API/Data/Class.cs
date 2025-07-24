using System.Security.Claims;

namespace Backend.API.Data
{
    public interface IUserContext
    {
        string? UserId { get; }
        string? UserName { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sid");

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public IEnumerable<Claim> GetAllClaims()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();
        }
    }
}
