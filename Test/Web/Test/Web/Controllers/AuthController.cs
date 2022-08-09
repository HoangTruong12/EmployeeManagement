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
            //var person = new Login 
            //{
            //    Username = "",
            //    Password = "test1"
            //};

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

                //if (!string.IsNullOrEmpty(result))
                //{
                //    var options = new JsonSerializerOptions
                //    {
                //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                //    };

                //    var dataJson = JsonSerializer.Deserialize<LoginResponse>(result, options);

                //    HttpContext.Session.SetString("AccessToken", dataJson.Token);

                //}

                if (!string.IsNullOrEmpty(result))
                {
                    HttpContext.Session.SetString("AccessToken", result);
                }
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
                    TempData["SuccessMessage"] = "Register Successfully";

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
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(employee);
        }
    }
}
