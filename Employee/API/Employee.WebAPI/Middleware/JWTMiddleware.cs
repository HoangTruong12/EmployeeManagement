﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Modal.Dto;
using Employee.Services.Interface;
using Employee.Data.Repository;
using Employee.Modal.Entities;

namespace Employee.WebAPI.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachAccountToContext(context, token);

            await _next(context);
        }

        private void AttachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Issuer"]
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var sub = jwtToken.Claims.First(x => x.Type == "sub").Value;
                //var pass = jwtToken.Claims.First(x => x.Type == "nameid").Value;

               

                if (sub != null)
                {
                    // check username trong token co ton tai trong db hay khong?


                    //
                    //context.Items["User"] =
                    //    new LoginDto { Username = sub, Password = pass};

                    context.Items["User"] = sub;
                }

            }
            catch (Exception ex)
            {
                context.Items["User"] = null;
            }
        }
    }
}
