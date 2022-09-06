using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ExcelController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmployeeController> _logger;

        public ExcelController(ILogger<EmployeeController> logger, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> DownloadFormatExcel()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                client.DefaultRequestHeaders.Authorization =
                         new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = _configuration["ApiUrl"] + "api/Excel/download";

                var result = await client.GetStringAsync(url);
                var convert = Convert.FromBase64String(result);

                byte[] fileContents = convert;

                return File(
                    fileContents,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Employees-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx"
                    );
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportFileExcel(IFormFile file)
        {
            try
             {
                if (file == null)
                {
                    TempData["FailMessage"] = "Please select the file to import";
                    return RedirectToAction("Index", "Employee");
                }

                using (var client = new HttpClient())
                {
                    string fileName = Path.GetFileName(file.FileName);
                   
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                    }

                    using var form = new MultipartFormDataContent();
                    using var fileContent = new ByteArrayContent(await System.IO.File.ReadAllBytesAsync(filePath));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(fileContent, "file", Path.GetFileName(filePath));

                    client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                    client.DefaultRequestHeaders.Authorization =
                         new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                    var url = _configuration["ApiUrl"] + "api/Excel/import";

                    HttpResponseMessage response = await client.PostAsync(url, form);
                    var message = response.Content.ReadAsStringAsync().Result;

                    if(message == "Invalid file extension")
                    {
                        TempData["FailMessage"] = "Invalid file extension";
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Import file excel successfully";
                        return RedirectToAction("Index", "Employee");
                    }

                    return RedirectToAction("Index", "Employee");
                }
            }
            catch (Exception ex) 
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index", "Employee");
        }
    }
}
