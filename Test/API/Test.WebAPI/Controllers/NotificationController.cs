using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.WebAPI.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("getAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var notifications = await _notificationService.GetAllNotification();
            return Ok(notifications);
        }

        [HttpGet("getNotification/{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var noti = await _notificationService.GetNotification(id);
            if (noti == null)
                return NotFound($"Notification with Id: {id} was not found");
            return Ok(noti);
        }

        // Show notification
        [HttpGet("getNotificationByReciver/{reciver}")]
        public async Task<IActionResult> GetNotificationByReciver(string reciver)
        {
            var result = await _notificationService.GetNotificationByReciver(reciver);
            return Ok(result);
        }

        [HttpPost("sendNotification")]
        public async Task<IActionResult> Create(Notification notification)
        {
            var result = await _notificationService.Create(notification);
            return CreatedAtAction("GetNotification", new { Id = notification.Id }, result);
        }

    }
}
