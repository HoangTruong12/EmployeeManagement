using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmployeeController> _logger;

        public DepartmentController(ILogger<EmployeeController> logger, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        HttpClient client = new HttpClient();
       [HttpGet]
       public JsonResult GetJsonDepartmentAsync(int id)
        {
            var list = new List<Department>();
            ViewBag.DepartmentList = list;

            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                HttpResponseMessage response;

                response = client.GetAsync(_configuration["ApiUrl"] + "api/Department/GetDepartment/" + id).Result;
                response.EnsureSuccessStatusCode();

                string json = response.Content.ReadAsStringAsync().Result;
                List<Department> departmentList = System.Text.Json.JsonSerializer.Deserialize<List<Department>>(json).ToList();

                if(!object.Equals(departmentList, null))
                {
                    var departments = departmentList.ToList();
                    foreach (var item in departments)
                    {
                        list.Add(new Department
                        {
                            Value = item.Id,
                            Text = item.DepartmentName
                        }); ;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(new SelectList(list, "Value", "Text"), new JsonSerializerSettings());
        }
    }
}
