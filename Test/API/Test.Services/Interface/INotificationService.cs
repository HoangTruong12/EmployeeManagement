using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Modal.Entities;

namespace Test.Services.Interface
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotification();
        Task<Notification> GetNotification(int id);
        Task<IEnumerable<Notification>> GetNotificationByReciver(string reciver);
        Task<bool> Create(Notification notification);
    }
}
