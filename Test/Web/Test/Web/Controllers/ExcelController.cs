using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
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
    }
}
