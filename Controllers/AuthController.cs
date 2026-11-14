using LogiTrack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _usermanager = userManager;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _usermanager.FindByNameAsync(request.Username);
            // Implement login logic here
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            var isPasswordValid = await _usermanager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var roles = await _usermanager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? request.Username)
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signingCredentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials);

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresAt = expiresAt
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _usermanager.CreateAsync(
                new IdentityUser { UserName = request.Username, Email = request.Email },
                request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(error => error.Description)
                });
            }

            return Ok(new { Message = "User registered successfully" });
        }
    }

