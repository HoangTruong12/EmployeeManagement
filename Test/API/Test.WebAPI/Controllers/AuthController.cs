using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Modal.Dto;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.WebAPI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        public AuthController(ILoginService loginService, IRegisterService registerService)
        {
            _loginService = loginService;
            _registerService = registerService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                var user = _loginService.Authenticate(request);

                if (user != null)
                {
                    var result = await _loginService.Login(request);

                    return Ok(result);
                }

                return BadRequest("Invalid Credentials");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkUsername = _registerService.CheckExistsUsernameEmployee(employee.Username);
                    if (checkUsername)
                    {
                        return BadRequest("Username already exist");
                    }

                    var checkEmail = _registerService.CheckExistsEmailEmployee(employee.Email);
                    if (checkEmail)
                    {
                        return BadRequest("Email already exist");
                    }

                    var checkPhoneNumber = _registerService.CheckExistsPhoneNumberEmployee(employee.PhoneNumber);
                    if (checkPhoneNumber)
                    {
                        return BadRequest("Phone number already exist");
                    }

                    await _registerService.Register(employee);
                    return Ok(employee);
                }
                return new JsonResult("Register Error") { StatusCode = 500 };
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}