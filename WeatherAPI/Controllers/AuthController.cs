using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using WeatherAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using WeatherAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly WeatherAPIDbContext _context;

        

        public AuthController(IConfiguration configuration, WeatherAPIDbContext weatherAPIDbContext, ILogger<AuthController> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
            _context = weatherAPIDbContext;
        }


        [HttpGet, Authorize]
        public ActionResult<string> GetDetails()
        {
            var email = _userService.GetDetails();
            return Ok(email);
        }



        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = request.UserName,
                Email = request.Email,
                Roles = request.Roles,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        
        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = _context.Users.ToList().Where(x => x.Email == request.Email).FirstOrDefault();
            if (user == null) return BadRequest("User not found");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        
        private string CreateToken(User UserDto)
        {
            var email = _userService.GetDetails();
            var user = _context.Users.Where(x => x.Email == UserDto.Email).FirstOrDefault();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Roles)

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        
    }
}
