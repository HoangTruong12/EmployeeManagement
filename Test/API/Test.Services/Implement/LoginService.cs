using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Dto;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class LoginService : BaseService, ILoginService
    {
        private readonly IRepository<Employee> _loginRepo;
        private readonly IConfiguration _configuration;

        public LoginService(IUnitOfWork unitOfWork, IRepository<Employee> loginRepo, IConfiguration configuration) : base(unitOfWork)
        {
            _loginRepo = loginRepo;
            _configuration = configuration;
        }

        public async Task<string> Login(LoginDto login)
        {
            //var user = Authenticate(login);

            if(login != null)
            {
                var tokenStr = GenerateJsonWebToken(login);
                return tokenStr;
            }

            return null;
        }

        private string GenerateJsonWebToken(LoginDto login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                //new Claim(JwtRegisteredClaimNames.NameId, login.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
                );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        public LoginDto Authenticate(LoginDto login)
        {

            var listUser = _loginRepo.GetAll();

            var convert = listUser.Select(x => new LoginDto
            {
                Username = x.Username,
                Password = x.Password,
            });

            var user = convert.SingleOrDefault(x => x.Username == login.Username);
            if(user != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

                if (isValidPassword)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
