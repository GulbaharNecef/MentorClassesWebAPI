using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication3.Auth;
using WebApplication3.DTOs;
using WebApplication3.Services.Abstraction;
using static System.Net.WebRequestMethods;

namespace WebApplication3.Services.Implementation
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        
        public TokenHandler(UserManager<AppUser> userManager, IConfiguration configuration)
        {

            _userManager = userManager;
            _configuration = configuration;

        }
        public string GetRefreshTokenAsync()
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]);
            var tokenDescription = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshToken = handler.CreateToken(tokenDescription);
            return handler.WriteToken(refreshToken);
        }

        public async Task<TokenDTO> GetTokenAsync(AppUser appUser)
        {
            TokenDTO tokenDTO = new TokenDTO();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.FirstName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id)

            };

            var roles = await _userManager.GetRolesAsync(appUser);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}
            claims.AddRange(roles.Select(x=>new Claim(ClaimTypes.Role, x)));

            tokenDTO.ExpirationTime = DateTime.UtcNow.AddMinutes(1);//configurasiyadan

            JwtSecurityToken securityToken = new JwtSecurityToken(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires:tokenDTO.ExpirationTime,
                notBefore:DateTime.UtcNow,
                signingCredentials: credentials,
                claims:claims
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            tokenDTO.Token = handler.WriteToken(securityToken);
            tokenDTO.RefershToken = GetRefreshTokenAsync();
            return tokenDTO;

        }

         

    }
}
