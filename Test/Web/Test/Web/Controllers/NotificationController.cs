using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Test.Modal.Entities;
using Web.Hubs;

namespace Web.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<NotificationController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        //private readonly IConfiguration _configuration;
        public NotificationController(ILogger<NotificationController> logger, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext) : base(configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        HttpClient client = new HttpClient();

        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Create(Notification notification)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("sendToUser", notification.Title, notification.Content);

                notification.Sender = (string)HttpContext.Items["User"];
                notification.Reciver = "all";

                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                var url = _configuration["ApiUrl"] + "api/Notification/sendNotification";

                var stringContent = new StringContent(JsonConvert.SerializeObject(notification), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, stringContent);

                if(response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Send Notification Successfully";
                    return Json(new { msg = "Send Notification Successfully", type = "Success" });
                }
                return RedirectToAction("Index", "Employee");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
