using Identity.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var verifyPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Name, user.Email)
            };
            var description = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddHours(1)

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenCreator = tokenHandler.CreateJwtSecurityToken(description);
            var token = tokenHandler.WriteToken(tokenCreator);
            return new LoginResponseDto
            {
                AccessToken = token,
                EmailAddress = user.Email,
                FullName = user.UserName,
                IsTwoFAEnabled = false,
                Role = claims[1].Value.ToString()
            };

        }
    }
}
