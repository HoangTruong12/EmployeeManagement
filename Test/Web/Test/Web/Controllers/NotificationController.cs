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
using Web.Hubs;
using Web.Models;

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

        public async Task<IActionResult> SendNoficationToAll()
        {
            ViewBag.Username = HttpContext.Items["User"];
            return View("SendNoficationToAll");
        }

        [HttpPost]
        public async Task<IActionResult> SendNoficationToAll(Notification notification)
        {
            try
            {
                notification.Sender = (string)HttpContext.Items["User"];
                notification.Reciver = "all";
                notification.DateCreate = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

                var result = await CreateNotification(notification);

                string notificationJson = JsonConvert.SerializeObject(notification);

                if (result == "Ok")
                {
                    await _hubContext.Clients.All.SendAsync("sendToUser", notificationJson);
                    TempData["SuccessMessage"] = "Send Notification Successfully";
                    return RedirectToAction("Index", "Employee");
                }
                return RedirectToAction("Create", "Notification");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> SendNotificationToSpecificUser()
        {
            ViewBag.Username = HttpContext.Items["User"];

            var list = await GetListUsername();
            if (list != null)
            {
                ViewBag.listUsername = list;
                return View("SendNotificationToSpecificUser");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationToSpecificUser(Notification notification)
        {
            try
            {
                notification.Sender = (string)HttpContext.Items["User"];
                //notification.Reciver = ;
                notification.DateCreate = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                var result = await CreateNotification(notification);

                string notificationJson = JsonConvert.SerializeObject(notification);

                if (result == "Ok")
                {
                    await _hubContext.Clients.Groups(notification.Reciver).SendAsync("sendToUser", notificationJson);
                    TempData["SuccessMessage"] = "Send Notification Successfully";
                    return RedirectToAction("Index", "Employee");
                }
                return RedirectToAction("Create", "Notification");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> CreateNotification(Notification notification)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                var url = _configuration["ApiUrl"] + "api/Notification/sendNotification";

                var stringContent = new StringContent(JsonConvert.SerializeObject(notification), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    return "Ok";
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        private async Task<List<string>> GetListUsername()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);

                client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());

                string url = _configuration["ApiUrl"] + "api/Employee/getListUsername";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    List<string> username = JsonConvert.DeserializeObject<List<string>>(res);
                    return username;
                }

                return null;
            }
        }
    }
}
