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
        public string CreateRefreshToken()//bunun async olmasi icinde neyese tesir ede biler
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Token:SecretKey"]);//ASCII
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(refreshToken);
        }

        public async Task<TokenDTO> CreateAccessTokenAsync(AppUser appUser)
        {
            try
            {
                TokenDTO tokenDTO = new TokenDTO();
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecretKey"]));

                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.FirstName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id)

            };

                //Role verirem tokene
                var roles = await _userManager.GetRolesAsync(appUser);
                //foreach (var role in roles)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, role));
                //}
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                tokenDTO.ExpirationTime = DateTime.UtcNow.AddMinutes(1);//configurasiyadan

                JwtSecurityToken securityToken = new JwtSecurityToken(
                    audience: _configuration["Token:Audience"],
                    issuer: _configuration["Token:Issuer"],
                    expires: tokenDTO.ExpirationTime,
                    notBefore: DateTime.UtcNow,//islemeye baslayacagi vaxt
                    signingCredentials: credentials,
                    claims: claims
                    );

                //token yaradiriq
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                tokenDTO.Token = handler.WriteToken(securityToken);
                tokenDTO.RefershToken =  CreateRefreshToken();
                return tokenDTO;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
