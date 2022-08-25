using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class NotificationService: BaseService, INotificationService
    {
        private readonly IRepository<Notification> _notificationRepo;
        private readonly IRepository<Employee> _empRepo;

        public NotificationService(IUnitOfWork unitOfWork, IRepository<Notification> notificationRepo, IRepository<Employee> empRepo) : base(unitOfWork)
        {
            _notificationRepo = notificationRepo;
            _empRepo = empRepo;
        }

        public async Task<IEnumerable<Notification>> GetAllNotification()
        {
            try
            {
                var notifications = _notificationRepo.GetAll();
                return notifications;
            }
            catch(Exception ex)
            {
                throw ex;
            }    
        }

        public async Task<Notification> GetNotification(int id)
        {
            try
            {
                var noti = await _notificationRepo.Get(id);
                return noti;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IEnumerable<Notification>> GetNotificationByReciver(string reciver)
        {
            try
            {
                var result = _notificationRepo.GetAll().Where(x => x.Reciver == reciver || x.Reciver == "all");

                return result.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<bool> Create(Notification notification)
        {
            try
            {
                //CheckExistsEmployee(employee);

                var result = new Notification
                {
                    Title = notification.Title,
                    Content = notification.Content,
                    Sender = notification.Sender,
                    Reciver = notification.Reciver,
                    DateCreate = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
                };

                UnitOfWork.BeginTransaction();
                await _notificationRepo.Add(result);
                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
