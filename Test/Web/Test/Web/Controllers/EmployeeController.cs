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
using Web.Filter;
using Web.Models;

namespace Web.Controllers
{
    //[Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        HttpClient client = new HttpClient();

        // GET: EmployeeController
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Index()
        //{
        //    client.BaseAddress = new Uri(_configuration["ApiUrl"]);

        //    client.DefaultRequestHeaders.Authorization =
        //          new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

        //    string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee");
        //    var res = JsonConvert.DeserializeObject<List<Employee>>(jsonStr).ToList();

        //    return View(res);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(string employeeId, string name)
        {
            ViewData["GetId"] = employeeId;
            ViewData["GetName"] = name;
 
            client.BaseAddress = new Uri(_configuration["ApiUrl"]);

            //client.DefaultRequestHeaders.Authorization =
            //      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

            string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee?employeeId=" + employeeId + "&name=" + name);
            var res = JsonConvert.DeserializeObject<List<ResponseViewModel>>(jsonStr).ToList();
           

            return View(res);
        }

        // GET: EmployeeController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee/getId/" + id);

                var res = JsonConvert.DeserializeObject<ResponseViewModel>(jsonStr);

                if (res == null)
                {
                    return NotFound();
                }

                return View(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
        }

        // GET: EmployeeController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = _configuration["ApiUrl"] + "api/Employee/create";

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
                    TempData["SuccessMessage"] = "Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);
        }

        // GET: EmployeeController/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int? id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id)
                {
                    return NotFound();
                }


                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = "api/Employee/update/" + id;
                var stringContent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);

        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            client.BaseAddress = new Uri(_configuration["ApiUrl"]);

            client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

            var deleteTask = client.DeleteAsync(_configuration["ApiUrl"] + "api/Employee/delete/" + id.ToString());

            var result = deleteTask.Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Deleted Successfully";
                return Json(new { msg= "Deleted Successfully", type= "Success" });
            }
            return Json(new { msg = "Deleted Failed", type = "Failed" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
