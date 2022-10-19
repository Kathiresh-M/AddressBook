using AutoMapper;
using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace AddressBookAPI.Services
    {
        public class JWTManagerServices : IJWTManagerServices
        {
            private readonly string _secret;
            private readonly string _expDate;

            public JWTManagerServices(IConfiguration config)
            {
                _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
                _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
            }

            public string GenerateSecurityToken(Profiles login)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                    new Claim(ClaimTypes.Name, login.User_Name)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);

            }

            public int? ValidateJwtToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var name = int.Parse(jwtToken.Claims.First(x => x.Type == "name").Value);
                    return name;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
