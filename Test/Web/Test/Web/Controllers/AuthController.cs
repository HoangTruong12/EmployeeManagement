using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Login(Login user)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(user);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                    var response = await client.PostAsync(_configuration["ApiUrl"] + "api/Auth/login", data);

                    var result = await response.Content.ReadAsStringAsync();

                    if (result == "Invalid Credentials")
                    {
                        TempData["Message"] = "Incorrect username or password";
                        return Redirect("~/Auth/Index");
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        //var options = new JsonSerializerOptions
                        //{
                        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        //    WriteIndented = true
                        //};

                        //var dataJson = System.Text.Json.JsonSerializer.Deserialize<string>(result, options);

                        HttpContext.Session.SetString("AccessToken", result);
                        HttpContext.Response.Cookies.Append("AccessToken", result, new CookieOptions
                        {
                            Expires = null,
                            HttpOnly = true,
                            Secure = true,
                        });
                    }

                    //if (!string.IsNullOrEmpty(result))
                    //{
                    //    string jsonSave = JsonConvert.SerializeObject(result);
                    //    var session = HttpContext.Session;
                    //    session.SetString("AccessToken", jsonSave);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
            return Redirect("~/Employee/Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Employee employee)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                    var url = _configuration["ApiUrl"] + "api/Auth/register";

                    var stringContent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, stringContent);

                    var message = response.Content.ReadAsStringAsync().Result;

                    if (message == "Username already exist")
                        TempData["FailMessage"] = "Username already exist";

                    if (message == "Email already exist")
                        TempData["FailMessage"] = "Email already exist";

                    if (message == "Phone number already exist")
                        TempData["FailMessage"] = "Phone number already exist";

                    if (response.IsSuccessStatusCode)
                    {
                        //TempData["SuccessMessage"] = "Register Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(employee);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Auth/Index");
        }


    }
}
