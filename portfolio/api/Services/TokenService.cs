using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration config) {
            this.config = config;
            this.key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(config["JWT:SigninKey"]));
            
        }
            
        
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>{
                
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new  Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Issuer = config["JWT:Issuer"],
                Audience = config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}