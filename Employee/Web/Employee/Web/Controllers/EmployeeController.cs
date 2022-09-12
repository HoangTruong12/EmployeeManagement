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
    [Authorize]
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
        public async Task<IActionResult> Index(string username, string name)
        {
            using(var client = new HttpClient())
            {
                ViewBag.Username = HttpContext.Items["User"];
                ViewData["GetUsername"] = username;
                ViewData["GetName"] = name;

                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                string jsonStr = await client.GetStringAsync(_configuration["ApiUrl"] + "api/Employee?username=" + username + "&name=" + name);
                var res = JsonConvert.DeserializeObject<List<ResponseViewModel>>(jsonStr).ToList();

                return View(res);
            }
        }

        // GET: EmployeeController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    ViewBag.Username = HttpContext.Items["User"];
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
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: EmployeeController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Username = HttpContext.Items["User"];

            var list = await GetListDepartment();
            if (list != null)
            {
                ViewBag.listDepartment = list;
                return View();
            }
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                using (var client = new HttpClient())
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
                            //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Index", null) });
                            return RedirectToAction("Index", "Employee");
                        }
                    }
                    //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", employee) });
                    return RedirectToAction("Create", "Employee");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: EmployeeController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    ViewBag.Username = HttpContext.Items["User"];

                    if (id == null)
                    {
                        return NotFound();
                    }

                    client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                    client.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                    string jsonStr = await client.GetStringAsync("api/Employee/getId/" + id);

                    var res = JsonConvert.DeserializeObject<EmployeeUpdateRequest>(jsonStr);

                    if (res == null)
                    {
                        return NotFound();
                    }
                    var list = await GetListDepartment();

                    if (list != null)
                    {
                        ViewBag.listDepartment = list;
                        return View(res);
                    }

                    return View(res);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EmployeeUpdateRequest request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ModelState.IsValid)
                    {
                        if (id != request.Id)
                        {
                            return NotFound();
                        }

                        client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                        client.DefaultRequestHeaders.Authorization =
                              new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                        var url = "api/Employee/update/" + id;
                        var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync(url, stringContent);

                        var message = response.Content.ReadAsStringAsync().Result;

                        if (message == "Phone number already exist")
                            TempData["FailMessage"] = "Phone number already exist - Update Fail";

                        if (message == "Email already exist")
                            TempData["FailMessage"] = "Email already exist - Update Fail";

                        if(message == "DepartmentId was not found")
                        {
                            TempData["FailMessage"] = "DepartmentId was not found - Please choose again";
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Updated Successfully";
                            //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", null) });
                            return RedirectToAction("Index", "Employee");
                        }
                    }

                    //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", employee)});
                    return RedirectToAction("Edit", "Employee");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var deleteTask = client.DeleteAsync(_configuration["ApiUrl"] + "api/Employee/delete/" + id.ToString());

                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Deleted Successfully";
                    return Json(new { msg = "Deleted Successfully", type = "Success" });
                }
                return Json(new { msg = "Deleted Failed", type = "Failed" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<Department>> GetListDepartment()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                string url = _configuration["ApiUrl"] + "api/Department/getListDepartment";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    List<Department> department = JsonConvert.DeserializeObject<List<Department>>(res);
                    return department;
                }

                return null;
            }

        }
    }
}
