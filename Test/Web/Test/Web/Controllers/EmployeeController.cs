using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        HttpClient client = new HttpClient();

        // GET: EmployeeController
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Index()
        {
            client.BaseAddress = new Uri(_configuration["ApiUrl"]);
            client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
            string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee");
            var res = JsonConvert.DeserializeObject<List<Employee>>(jsonStr).ToList();

            return View(res);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            client.BaseAddress = new Uri(_configuration["ApiUrl"]);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

            string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee/getId/" + id);

            var res = JsonConvert.DeserializeObject<Employee>(jsonStr);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                TempData["SuccessMessage"] = "Created Successfully";

                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = _configuration["ApiUrl"] + "api/Employee/";

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
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);
        }

        // GET: EmployeeController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            client.BaseAddress = new Uri(_configuration["ApiUrl"]);

            client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

            string jsonStr = await client.GetStringAsync("api/Employee/getId/" + id);

            var res = JsonConvert.DeserializeObject<Employee>(jsonStr);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id)
                {
                    return NotFound();
                }


                client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                TempData["SuccessMessage"] = "Updated Successfully";

                client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = "api/Employee/" + id;
                var stringContent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);

        }

        // POST: EmployeeController/Delete/5
        //[HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            client.BaseAddress = new Uri(_configuration["ApiUrl"]);
            TempData["SuccessMessage"] = "Deleted Successfully";

            client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

            var deleteTask = client.DeleteAsync(_configuration["ApiUrl"] + "api/Employee/delete/" + id);

            var result = deleteTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
