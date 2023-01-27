using System.Security.Claims;

namespace WeatherAPI.Services.User_Service
{

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public UserService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }
         
        string IUserService.GetDetails()
        {
            var result = string.Empty;
            var email = string.Empty;
            var role = string.Empty;

            if (_httpContextAccesor.HttpContext != null)
            {
                result = _httpContextAccesor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            return result;


        }
    }
}
